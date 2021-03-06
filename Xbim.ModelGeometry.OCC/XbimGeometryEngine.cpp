#include "StdAfx.h"
#include "XbimGeometryEngine.h"
#include "XbimFeaturedShape.h"
#include "XbimShell.h"
#include "XbimGeometryModelCollection.h"
#include "XbimFacetedShell.h"
#include "XbimMap.h"
#include <BRepBuilderAPI.hxx>
#include "XbimGeomPrim.h"
#include "CartesianTransform.h"
using namespace Xbim::Common::Logging;
using namespace Xbim::ModelGeometry::OCC;
using namespace System::Linq;
using namespace Xbim::Ifc2x3::ProductExtension;
using namespace Xbim::Ifc2x3::SharedComponentElements;
using namespace Xbim::Ifc2x3::PresentationAppearanceResource;
using namespace Xbim::IO;
namespace Xbim
{
	namespace ModelGeometry
	{

		/* Creates a 3D Model geometry for the product based upon the first "Body" ShapeRepresentation that can be found in  
		Products.ProductRepresentation that is within the specified GeometricRepresentationContext, if the Representation 
		context is null the first "Body" ShapeRepresentation is used. Returns null if their is no valid geometric definition
		*/
		IXbimGeometryModel^ XbimGeometryEngine::GetGeometry3D(IfcProduct^ product, ConcurrentDictionary<int, Object^>^ maps)
		{
			return CreateFrom(product,maps,false,XbimLOD::LOD_Unspecified,false);
		}

		IXbimGeometryModel^ XbimGeometryEngine::GetGeometry3D(IfcSolidModel^ solid, ConcurrentDictionary<int, Object^>^ maps)
		{
			return CreateFrom(solid,maps,false,XbimLOD::LOD_Unspecified,false);
		}
		IXbimGeometryModel^ XbimGeometryEngine::GetGeometry3D(IfcProduct^ product)
		{
			return CreateFrom(product,nullptr,false,XbimLOD::LOD_Unspecified,false);
		}

		IXbimGeometryModel^ XbimGeometryEngine::GetGeometry3D(IfcSolidModel^ solid)
		{
			return CreateFrom(solid,nullptr,false,XbimLOD::LOD_Unspecified,false);
		}

		XbimGeometryModel^ XbimGeometryEngine::CreateFrom(IfcProduct^ product, IfcGeometricRepresentationContext^ repContext, ConcurrentDictionary<int, Object^>^ maps, bool forceSolid, XbimLOD lod, bool occOut)
		{
			try
			{
				if(product->Representation == nullptr ||  product->Representation->Representations == nullptr 
					|| dynamic_cast<IfcTopologyRepresentation^>(product->Representation)) 
					return nullptr; //if it doesn't have one do nothing
				//we should cast the shape below to a ShapeRepresentation but using IfcRepresentation means this works for older IFC2x formats  and there is no data loss

				for each(IfcRepresentation^ shape in product->Representation->Representations)
				{

					if(repContext == nullptr || shape->ContextOfItems ==  repContext) 
					{
						if( !shape->RepresentationIdentifier.HasValue ||
							(String::Compare(shape->RepresentationIdentifier.Value, "body" , true)==0)||
							String::Compare(shape->RepresentationIdentifier.Value, "facetation" , true)==0)
							//we have a 3D geometry
						{
							
							
							//srl optimisation openings and projectionss cannot have openings or projection so don't check for them
							if(CutOpenings(product, lod) && !dynamic_cast<IfcFeatureElement^>(product ))
							{
								IfcElement^ element = (IfcElement^) product;
								XbimGeometryModelCollection^ projectionSolids = gcnew XbimGeometryModelCollection();
								XbimGeometryModelCollection^ openingSolids = gcnew XbimGeometryModelCollection();
								for each(IfcRelProjectsElement^ rel in element->HasProjections)
								{
									IfcFeatureElementAddition^ fe = rel->RelatedFeatureElement;
									XbimGeometryModel^ im = CreateFrom(fe, repContext, maps, true,lod, occOut);
									if(im!=nullptr && im->IsValid)
									{	
										if(!dynamic_cast<IfcLocalPlacement^>(fe->ObjectPlacement))
										{
											Logger->ErrorFormat("unhandled opening #{0}, Only IfcObjectPlacements of type IfcLocalPlacement are supported", fe->EntityLabel);
											continue;
										}
										IfcLocalPlacement^ projectionPlacement = (IfcLocalPlacement^)(fe->ObjectPlacement);
										im = im->CopyTo(projectionPlacement->RelativePlacement);
										//the rules say that 
										//The PlacementRelTo relationship of IfcLocalPlacement shall point (if given) 
										//to the local placement of the master IfcElement (its relevant subtypes), 
										//which is associated to the IfcFeatureElement by the appropriate relationship object
										if(product->ObjectPlacement != projectionPlacement->PlacementRelTo)
										{
											if(dynamic_cast<IfcLocalPlacement^>(product->ObjectPlacement))
											{	
												//we need to move the opening into the coordinate space of the product						
												TopLoc_Location prodLoc = XbimGeomPrim::ToLocation(projectionPlacement->RelativePlacement);
												prodLoc= prodLoc.Inverted();
												im->Move(prodLoc);
											}
										}
										projectionSolids->Add(im);
									}
								}
								for each(IfcRelVoidsElement^ rel in element->HasOpenings)
								{
									IfcFeatureElementSubtraction^ fe = rel->RelatedOpeningElement;
									if(fe->Representation!=nullptr)
									{
										if(!dynamic_cast<IfcLocalPlacement^>(fe->ObjectPlacement))
										{
											Logger->ErrorFormat("unhandled opening #{0}, Only IfcObjectPlacements of type IfcLocalPlacement are supported", fe->EntityLabel);
											continue;
										}
										IfcLocalPlacement^ openingPlacement = (IfcLocalPlacement^)(fe->ObjectPlacement);
										XbimGeometryModel^ im = CreateFrom(fe, repContext, maps, true,lod, occOut);
										
										if(im!=nullptr && im->IsValid) //we might have built nothing
										{											
											im = im->CopyTo(openingPlacement->RelativePlacement);
											//
											////the rules say that 
											////The PlacementRelTo relationship of IfcLocalPlacement shall point (if given) 
											////to the local placement of the master IfcElement (its relevant subtypes), 
											////which is associated to the IfcFeatureElement by the appropriate relationship object
											if(product->ObjectPlacement != openingPlacement->PlacementRelTo)
											{
												if(dynamic_cast<IfcLocalPlacement^>(product->ObjectPlacement))
												{	
													//we need to move the opening into the coordinate space of the product							
													TopLoc_Location prodLoc = XbimGeomPrim::ToLocation(openingPlacement->RelativePlacement);
													prodLoc= prodLoc.Inverted();
													im->Move(prodLoc);
												}
											}
											openingSolids->Add(im);
										}
									}

								}
								if(Enumerable::Any(openingSolids) || Enumerable::Any(projectionSolids))
								{

									XbimGeometryModel^ baseShape = CreateFrom(shape, maps, false,lod, occOut);	
									
									
									
									XbimGeometryModel^ fshape = gcnew XbimFeaturedShape(product, baseShape, openingSolids, projectionSolids);
									return fshape;
								}
								else //we have no openings or projections
								{

									XbimGeometryModel^ fshape = CreateFrom(shape, maps, forceSolid,lod, occOut);
									return fshape;
								}
							}
							else
							{

								XbimGeometryModel^ fshape = CreateFrom(shape, maps, forceSolid,lod, occOut);
#ifdef _DEBUG
								if(occOut)
								{
									char fname[512];
									sprintf(fname, "#%d",shape->EntityLabel);
									BRepTools::Write(*(fshape->Handle),fname );
								}
#endif
								return fshape;
							}
						}
					}
				}
			}
			catch(XbimGeometryException^ xbimE)
			{
				Logger->ErrorFormat("Error creating geometry for entity #{0}={1}\n{2}\nThe geometry has been omitted",product->EntityLabel,product->GetType()->Name,xbimE->Message);
			}
			return nullptr;
		}

		XbimGeometryModel^ XbimGeometryEngine::CreateFrom(IfcProduct^ product, ConcurrentDictionary<int, Object^>^ maps, bool forceSolid, XbimLOD lod, bool occOut)
		{
			return CreateFrom(product, nullptr, maps, forceSolid, lod, occOut);
		}

		XbimGeometryModel^ XbimGeometryEngine::CreateFrom(IfcProduct^ product, bool forceSolid, XbimLOD lod, bool occOut)
		{
			// HACK: Ideally we shouldn't need this try-catch handler. This just allows us to log the fault, and raise a managed exception, before the application terminates.
			// Upstream callers should ideally terminate the application ASAP.
			__try
			{
				return CreateFrom(product, nullptr, gcnew ConcurrentDictionary<int, Object^>(), forceSolid,lod,occOut);
			}
			__except(GetExceptionCode() == EXCEPTION_ACCESS_VIOLATION)
			{
				Logger->Fatal("Access Violation in geometry engine. Thireturns may leave the application in an inconsistent state!");
				throw gcnew AccessViolationException(
					"A memory access violation occurred in the geometry engine. The application and geometry may be in an inconsistent state and the process should be terminated.");
			}
		}

		/*
		Create a model geometry for a given shape
		*/
		XbimGeometryModel^ XbimGeometryEngine::CreateFrom(IfcRepresentation^ rep, ConcurrentDictionary<int, Object^>^ maps, bool forceSolid, XbimLOD lod, bool occOut)
		{

			if(rep->Items->Count == 0) //we have nothing to do
				return nullptr;
			else if (rep->Items->Count == 1) //we have a single shape geometry
			{
				IfcRepresentationItem^ repItem = rep->Items->First;
				XbimGeometryModel^ geom = CreateFrom(repItem,maps, forceSolid,lod,occOut);
				return geom;
			}
			else // we have a compound shape
			{
				XbimGeometryModelCollection^ gms = gcnew XbimGeometryModelCollection(rep);
				for each (IfcRepresentationItem^ repItem in rep->Items)
				{
					XbimGeometryModel^  geom= CreateFrom(repItem,maps,false,lod,occOut); // we will make a solid when we have all the bits if necessary
					if(geom!=nullptr) gms->Add(geom); //don't add solids that are empty
				}
				return gms;
			}
		}

		XbimGeometryModel^ XbimGeometryEngine::CreateFrom(IfcRepresentation^ rep, bool forceSolid, XbimLOD lod, bool occOut)
		{
			return CreateFrom(rep, gcnew ConcurrentDictionary<int, Object^>(), forceSolid,lod, occOut);
		}

		XbimGeometryModel^ XbimGeometryEngine::CreateFrom(IfcRepresentationItem^ repItem, bool forceSolid, XbimLOD lod, bool occOut)
		{
			return CreateFrom(repItem, gcnew ConcurrentDictionary<int, Object^>(), forceSolid,lod, occOut);
		}

		XbimGeometryModel^ XbimGeometryEngine::CreateFrom(IfcRepresentationItem^ repItem, ConcurrentDictionary<int, Object^>^ maps, bool forceSolid,XbimLOD lod, bool occOut)
		{
			//look up if we have already created it
			
			Object^ lookup;
			if(maps->TryGetValue(Math::Abs(repItem->EntityLabel),lookup))
				return (XbimGeometryModel^)lookup;
			XbimGeometryModel^ geomModel;
		    if(dynamic_cast<IfcHalfSpaceSolid^>(repItem))
				geomModel =  gcnew XbimSolid((IfcHalfSpaceSolid^)repItem);
			else if(dynamic_cast<IfcCsgPrimitive3D^>(repItem))
				geomModel =  gcnew XbimSolid((IfcCsgPrimitive3D^)repItem);
			else if(dynamic_cast<IfcFacetedBrep^>(repItem)) 
				geomModel =  gcnew XbimFacetedShell((IfcFacetedBrep^)repItem);
			else if(dynamic_cast<IfcShellBasedSurfaceModel^>(repItem)) 
				geomModel =  gcnew XbimFacetedShell((IfcShellBasedSurfaceModel^)repItem);
			else if(dynamic_cast<IfcFaceBasedSurfaceModel^>(repItem)) 
				geomModel =  gcnew XbimFacetedShell((IfcFaceBasedSurfaceModel^)repItem);
			else if(dynamic_cast<IfcSolidModel^>(repItem))	
				geomModel = gcnew XbimSolid((IfcSolidModel^)repItem); 
			else if(dynamic_cast<IfcBooleanResult^>(repItem)) 
				geomModel =  Build((IfcBooleanResult^)repItem,maps);
			else if(dynamic_cast<IfcMappedItem^>(repItem))
			{
				IfcMappedItem^ map = (IfcMappedItem^) repItem;
				IfcRepresentationMap^ repMap = map->MappingSource;
				XbimGeometryModel^ mg;
				
				if(!maps->TryGetValue(Math::Abs(repMap->MappedRepresentation->EntityLabel), lookup)) //look it up
				{
					mg =  CreateFrom(repMap->MappedRepresentation,maps, forceSolid,lod, occOut); //make the first one
					if(mg!=nullptr)
					{
						maps->TryAdd(Math::Abs(repMap->MappedRepresentation->EntityLabel), mg);
					}
				}
				else
					mg= (XbimGeometryModel^)lookup;

				//need to transform all the geometries as below
				if(mg!=nullptr)
					geomModel =  gcnew XbimMap(mg,repMap->MappingOrigin,map->MappingTarget, maps); 
				else
					geomModel =  nullptr;

			} //the below items should build surfaces or topologies and need to be implemented
			else if(dynamic_cast<IfcCurveBoundedPlane^>(repItem))
			{
				geomModel =  nullptr; //surface is not implmented yet
				//return gcnew XbimSolid((IfcVertexPoint^)repItem);
			}
			else if(dynamic_cast<IfcVertexPoint^>(repItem))
			{
				geomModel =  nullptr; //topology is not implmented yet
				//return gcnew XbimSolid((IfcVertexPoint^)repItem);
			}
			else if(dynamic_cast<IfcEdge^>(repItem))
			{
				geomModel =  nullptr; //topology is not implmented yet
				//return gcnew XbimSolid((IfcEdge^)repItem);
			}
			else if(dynamic_cast<IfcGeometricSet^>(repItem))
			{
				geomModel =  nullptr; //this s not a solid object
				//IfcGeometricSet^ gset = (IfcGeometricSet^) repItem;
				//Logger->Warn(String::Format("Support for IfcGeometricSet #{0} has not been implemented", Math::Abs(gset->EntityLabel)));
			}
			else
			{
				geomModel =  nullptr;
				Type ^ type = repItem->GetType();
				Logger->Warn(String::Format("XbimGeometryModel. Could not Build Geometry #{0}, type {1} is not implemented",Math::Abs(repItem->EntityLabel), type->Name));
			}
			
			if(geomModel !=nullptr && !geomModel->IsMap) //no point really in mapping maps
				maps->TryAdd(Math::Abs(repItem->EntityLabel),geomModel);
			return geomModel;
		}

		

		bool XbimGeometryEngine::CutOpenings(IfcProduct^ product, XbimLOD lod)
		{
			if(dynamic_cast<IfcElement^>(product))
			{
				//add in additional types here that you don't want to cut
				if(dynamic_cast<IfcBeam^>(product) ||
					dynamic_cast<IfcColumn^>(product) ||
					dynamic_cast<IfcMember^>(product)||
					dynamic_cast<IfcElementAssembly^>(product)||
					dynamic_cast<IfcPlate^>(product))
				{
					return lod==XbimLOD::LOD400;
				}
				else
					return true;
			}
			else
				return false;
		}

		XbimGeometryModel^ XbimGeometryEngine::Build(IfcBooleanResult^ repItem, ConcurrentDictionary<int, Object^>^ maps)
		{
			double precision = repItem->ModelOf->ModelFactors->Precision;
			double maxPrecision = repItem->ModelOf->ModelFactors->PrecisionMax;
			IfcBooleanOperand^ fOp= repItem->FirstOperand;
			IfcBooleanOperand^ sOp= repItem->SecondOperand;
			XbimGeometryModel^ shape1;
			XbimGeometryModel^ shape2;

			if(dynamic_cast<IfcBooleanResult^>(fOp))
				shape1 = Build((IfcBooleanResult^)fOp, maps);
			else if(dynamic_cast<IfcSolidModel^>(fOp))
				shape1 = CreateFrom((IfcSolidModel^)fOp,maps,false,XbimLOD::LOD_Unspecified,false);
			else if(dynamic_cast<IfcHalfSpaceSolid^>(fOp))
				shape1 = CreateFrom((IfcHalfSpaceSolid^)fOp,maps,false,XbimLOD::LOD_Unspecified,false);	
			else if(dynamic_cast<IfcCsgPrimitive3D^>(fOp))
				shape1 = CreateFrom((IfcCsgPrimitive3D^)fOp,maps,false,XbimLOD::LOD_Unspecified,false);
			else
				throw(gcnew XbimException("XbimGeometryModel. Build(BooleanResult) FirstOperand must be a valid IfcBooleanOperand"));
			try
			{

				if(dynamic_cast<IfcBooleanResult^>(sOp))
					shape2 = Build((IfcBooleanResult^)sOp,maps);
				else if(dynamic_cast<IfcSolidModel^>(sOp))
					shape2 = CreateFrom((IfcSolidModel^)sOp,maps,false,XbimLOD::LOD_Unspecified,false);
				else if(dynamic_cast<IfcHalfSpaceSolid^>(sOp))
					shape2 = CreateFrom((IfcHalfSpaceSolid^)sOp,maps,false,XbimLOD::LOD_Unspecified,false);
				else if(dynamic_cast<IfcCsgPrimitive3D^>(sOp))
					shape2 = CreateFrom((IfcCsgPrimitive3D^)sOp,maps,false,XbimLOD::LOD_Unspecified,false);
				else
					throw(gcnew XbimException("XbimGeometryModel. Build(BooleanResult) FirstOperand must be a valid IfcBooleanOperand"));

				if((*(shape2->Handle)).IsNull())
					return shape1; //nothing to subtract

				switch(repItem->Operator)
				{
				case IfcBooleanOperator::Union:	
					return shape1->Union(shape2,precision,maxPrecision);	
				case IfcBooleanOperator::Intersection:	
					return shape1->Intersection(shape2,precision,maxPrecision);
				case IfcBooleanOperator::Difference:					
					return shape1->Cut(shape2,precision,maxPrecision); //need to be a bit courser for this
				default:
					throw(gcnew InvalidOperationException("XbimGeometryModel. Build(BooleanClippingResult) Unsupported Operation"));
				}
			}
			catch(XbimGeometryException^ xbimE)
			{
				Logger->ErrorFormat("Error performing boolean operation for entity #{0}={1}\n{2}\nA simplified version has been used",repItem->EntityLabel,repItem->GetType()->Name,xbimE->Message);
				return shape1;
			}
		}


		XbimGeometryModel^ XbimGeometryEngine::Build(IfcCsgSolid^ csgSolid, ConcurrentDictionary<int, Object^>^ maps)
			{
				if(dynamic_cast<IfcBooleanResult^>(csgSolid->TreeRootExpression)) 
					return Build((IfcBooleanResult^)csgSolid->TreeRootExpression, maps);
				else if(dynamic_cast<IfcCsgPrimitive3D^>(csgSolid->TreeRootExpression))
					return gcnew XbimSolid((IfcCsgPrimitive3D^)csgSolid->TreeRootExpression);
				else
					throw gcnew NotImplementedException("Build::IfcCsgSolid is not implemented");
		}

	}
}