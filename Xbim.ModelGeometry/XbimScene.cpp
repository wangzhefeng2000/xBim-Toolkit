#include "StdAfx.h"
#include "XbimScene.h"
#include "IXbimGeometryModel.h"
#include "XbimGeometryModel.h"
#include "XbimMap.h"
using namespace System::IO;
using namespace System::Linq;

using namespace Xbim::ModelGeometry::Scene;
using namespace Xbim::Common::Exceptions;
using namespace Xbim::XbimExtensions;
using namespace Xbim::Common;


namespace Xbim
{
	namespace ModelGeometry
	{	
		XbimScene::!XbimScene()
		{
		}

		XbimScene::~XbimScene()
		{
			_maps = gcnew Dictionary<IfcRepresentation^, IXbimGeometryModel^>();
		}

		XbimScene::XbimScene(XbimModel^ model)
		{

			Initialise();
			Logger->Debug("Creating Geometry from IModel..."); 
			_graph = gcnew TransformGraph(model, this);
			_maps = gcnew Dictionary<IfcRepresentation^, IXbimGeometryModel^>();
			_graph->AddProducts(Enumerable::Cast<IfcProduct^>(model->IfcProducts));
		}



		void XbimScene::ConvertGeometry( IEnumerable<IfcProduct^>^ toConvert, ReportProgressDelegate^ progDelegate, bool oCCout)
		{
			IfcProduct^ p = Enumerable::FirstOrDefault(toConvert);
			if(p == nullptr) //nothing to do
				return;
			XbimModel^ model = (XbimModel^)p->ModelOf;

			TransformGraph^ graph = gcnew TransformGraph(model);
			//create a new dictionary to hold maps
			Dictionary<int, Object^>^ maps = gcnew Dictionary<int, Object^>();
			//add everything that may have a representation
			graph->AddProducts(toConvert); //load the products as we will be accessing their geometry

			XbimGeometryCursor^ geomTable = model->GetGeometryTable();


			int tally = 0;
			int percentageParsed=0;
			int total = Enumerable::Count(toConvert);
			XbimLazyDBTransaction transaction = geomTable->BeginLazyTransaction();
			try
			{
				for each(TransformNode^ node in graph->ProductNodes->Values) //go over every node that represents a product
				{
					IfcProduct^ product = node->Product;
					try
					{
						XbimLOD lod = XbimLOD::LOD_Unspecified;
						IXbimGeometryModel^ geomModel = XbimGeometryModel::CreateFrom(product, maps, false, lod,oCCout);
						if (geomModel != nullptr)  //it has no geometry
						{
							List<XbimTriangulatedModel^>^tm;
							Matrix3D m3d = node->WorldMatrix();
							if(dynamic_cast<XbimMap^>(geomModel))
							{
								XbimMap^ map = (XbimMap^)geomModel;
								m3d = Matrix3D::Multiply(map->Transform, m3d);
								Object^ lookup;
								int key = -map->MappedItem->GetHashCode();
								if(maps->TryGetValue(key, lookup))
								{
									tm=(List<XbimTriangulatedModel^>^)lookup;
								}
								else
								{
									tm = geomModel->Mesh(true);
									maps->Add(key,tm);
								}
							}
							else
								tm = geomModel->Mesh(true);
							XbimBoundingBox^ bb = geomModel->GetBoundingBox(true);
							//node->BoundingBox = bb->GetRect3D();
							
							array<Byte>^ matrix = Matrix3DExtensions::ToArray(m3d, true);

							Nullable<short> typeId = IfcMetaData::IfcTypeId(product);

							geomTable->AddGeometry(product->EntityLabel, XbimGeometryType::BoundingBox, typeId.Value, matrix, bb->ToArray(), 0 ,geomModel->SurfaceStyleLabel) ;
							int subPart = 0;
							for each(XbimTriangulatedModel^ b in tm)
							{
								geomTable->AddGeometry(product->EntityLabel, XbimGeometryType::TriangulatedMesh, typeId.Value, matrix, b->Triangles , subPart, b->SurfaceStyleLabel) ;
								subPart++;
							}
							tally++;
							if(progDelegate!=nullptr)
							{
								int newPercentage = Convert::ToInt32((double)tally / total * 100.0);
								if (newPercentage > percentageParsed)
								{
									percentageParsed = newPercentage;
									progDelegate(percentageParsed, "Converted");
								}
							}

							if (tally % 100 == (100 - 1))
							{
								transaction.Commit();
								transaction.Begin();
							}
						}
					}
					catch(Exception^ e1)
					{
						String^ message = String::Format("Error Triangulating product geometry of entity {0} - {1}", 
							product->EntityLabel,
							product->ToString());
						Logger->Warn(message, e1);
					}
				}
				transaction.Commit();
			}
			catch(Exception^ e2)
			{
				Logger->Warn("General Error Triangulating geometry", e2);
			}
			finally
			{
				model->FreeTable(geomTable);
			}



		}
		XbimScene::XbimScene(XbimModel^ model, IEnumerable<IfcProduct^>^ toDraw, bool OCCout)
		{
			Initialise();
			_occOut = OCCout;
			Logger->Debug("Creating Geometry from IModel..."); 
			_graph = gcnew TransformGraph(model, this);
			_maps = gcnew Dictionary<IfcRepresentation^, IXbimGeometryModel^>();
			_graph->AddProducts(toDraw);
		}


		void XbimScene::Close()
		{
			_sceneStream->Close();
			_graph->Close();
		}

	}


}
