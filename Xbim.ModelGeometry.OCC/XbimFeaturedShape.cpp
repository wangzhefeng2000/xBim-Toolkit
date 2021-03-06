#include "StdAfx.h"
#include "XbimFeaturedShape.h"
#include "XbimGeomPrim.h"
#include "XbimSolid.h"
#include "XbimShell.h"
#include "XbimFacetedShell.h"
#include "XbimBoundingBox.h"
#include "XbimCsg.h"
#include <BRepAlgoAPI_Cut.hxx>
#include <BRepAlgo_Cut.hxx>
#include <BRepAlgoAPI_Fuse.hxx>
#include <BRepAlgoAPI_Common.hxx>
#include <BRepMesh_IncrementalMesh.hxx>
#include <Poly_Array1OfTriangle.hxx>
#include <TColgp_Array1OfPnt.hxx>
#include <TShort_Array1OfShortReal.hxx>
#include <Poly_Triangulation.hxx>
#include <ShapeFix_Solid.hxx>
#include <ShapeFix_Shell.hxx> 
#include <ShapeFix_Shape.hxx> 
#include <ShapeFix_ShapeTolerance.hxx> 
#include <BRepBndLib.hxx> 
#include <BRepLib.hxx> 
#include <BRepCheck_Analyzer.hxx> 
#include <Bnd_BoundSortBox.hxx> 
#include <TColStd_ListOfInteger.hxx> 
#include <Bnd_HArray1OfBox.hxx>
#include <TopTools_HArray1OfShape.hxx> 
using namespace System::Linq;
using namespace Xbim::Common::Exceptions;
using namespace  System::Runtime::ExceptionServices;
namespace Xbim
{
	namespace ModelGeometry
	{
		namespace OCC
		{
			


			XbimFeaturedShape::XbimFeaturedShape(IfcProduct^ product, XbimGeometryModel^ baseShape, XbimGeometryModelCollection^ openings, IEnumerable<XbimGeometryModel^>^ projections)
			{
				if(baseShape==nullptr)
				{
					Logger->Warn("Undefined base shape passed to XbimFeaturedShape");
					return;
				}
				mResultShape = mBaseShape = baseShape;	
				RepresentationLabel= baseShape->RepresentationLabel;
				SurfaceStyleLabel=baseShape->SurfaceStyleLabel;
				_hasCurvedEdges = mBaseShape->HasCurvedEdges;	
				mResultShape =  SubtractFrom(mResultShape, openings, product->ModelOf->ModelFactors->DeflectionTolerance, product->ModelOf->ModelFactors->PrecisionBoolean,product->ModelOf->ModelFactors->PrecisionBooleanMax); //openings work best at .1mm tolerance
				mResultShape->RepresentationLabel= baseShape->RepresentationLabel;
				mResultShape->SurfaceStyleLabel=baseShape->SurfaceStyleLabel;
				
			}

			XbimPolyhedron^ XbimFeaturedShape::ToPolyHedron(double deflection, double precision, double precisionMax)
			{
				throw gcnew NotImplementedException();
			}

			//divides the openings into a list of list of non-intersecting shapes
			XbimGeometryModelCollection^ XbimFeaturedShape::PrepareFeatures(XbimGeometryModelCollection^ features, double precision, double precisionMax)
			{

				XbimGeometryModelCollection^ prepared = gcnew XbimGeometryModelCollection();
				
				for each (XbimGeometryModel^ shape in features)
				{
					shape->ToSolid(precision, precisionMax);
					TopoDS_Shape * occShape = shape->Handle;
					TopAbs_ShapeEnum sType =occShape->ShapeType();
					switch (sType)
					{
					case TopAbs_COMPOUND:
						
						for (TopExp_Explorer sExp(*occShape, TopAbs_SOLID); sExp.More(); sExp.Next())
						{
							XbimSolid^ solid = gcnew XbimSolid(TopoDS::Solid(sExp.Current()),shape->HasCurvedEdges,shape->RepresentationLabel,shape->SurfaceStyleLabel);
							
							double vol = solid->Volume;
							if(vol<0) 
								solid->Handle->Reverse();
							if(vol!=0)
							{
								prepared->Add(solid);
							}
						}
						break;
					case TopAbs_SOLID:
						prepared->Add(shape); //just add it;
						break;
					case TopAbs_COMPSOLID:
					case TopAbs_SHELL:
					case TopAbs_FACE:
					case TopAbs_WIRE:
					case TopAbs_EDGE:
					case TopAbs_VERTEX:
					case TopAbs_SHAPE:
					default:
						Logger->WarnFormat("Unexpected shape type found in opening #{0}. Ignored",shape->RepresentationLabel);
						break;
					}
				}
				prepared->SortDescending();
				return prepared;
				
				
				/*XbimGeometryModelCollection^ nonClashing = gcnew XbimGeometryModelCollection(features->HasCurvedEdges,features->RepresentationLabel,features->SurfaceStyleLabel);
				return ExtractNonClashing(clashing, nonClashing, precision, precisionMax);*/

			}
			///copies non clashing objects from the candidates to nonClashing and returns a list of geometries that still clash
			XbimGeometryModelCollection^ XbimFeaturedShape::ExtractNonClashing(List<XbimGeometryModel^>^ candidates, XbimGeometryModelCollection^ nonClashing, double precision, double precisionMax)
			{

				if(!Enumerable::Any(candidates)) return nonClashing;
				Stack<XbimGeometryModel^>^ unprocessed = gcnew Stack<XbimGeometryModel^>(candidates);
				XbimGeometryModel^ first = unprocessed->Pop();
				nonClashing->Add(first);
				List<List<XbimRect3D>^>^ bounds = gcnew List<List<XbimRect3D>^>();
				List<XbimRect3D>^ bound = gcnew List<XbimRect3D>();
				bound->Add(first->GetBoundingBox());
				bounds->Add(bound);
				while(unprocessed->Count>0)
				{
					XbimGeometryModel^ candidate = unprocessed->Pop();
					XbimGeometryModelCollection^ toReplace = nullptr;
					XbimRect3D candidateBound = candidate->GetBoundingBox();
					int foundAt;
					List<XbimRect3D>^ shapeBound;
					for(foundAt = 0;foundAt<nonClashing->Count;foundAt++)
					{	
						XbimGeometryModel^ shape = nonClashing->Shape(foundAt);
						shapeBound = bounds[foundAt];
						for each (XbimRect3D partBound in shapeBound)
						{
							if(partBound.Intersects(candidateBound)) //any part of shape intersects this one
							{
								//XbimGeometryModel^ join=shape->Union(candidate,precision,precisionMax); //join together and reconsider it as a clash

								if(dynamic_cast<XbimGeometryModelCollection^>(shape)) toReplace = (XbimGeometryModelCollection^)shape;
								else
								{
									toReplace = gcnew XbimGeometryModelCollection();
									toReplace->Add(shape);
								}
								toReplace->Add(candidate);
								shapeBound->Add(candidateBound); //increase bounds
								//toReplace=join;
								
								break; //done
							}
						}
						if(toReplace!=nullptr) break; //skip out
					}
					if(toReplace==nullptr)
					{
						nonClashing->Add(candidate);
						List<XbimRect3D>^ bound = gcnew List<XbimRect3D>();
						bound->Add(candidateBound);
						bounds->Add(bound);
					}
					else
					{
						nonClashing->Replace(foundAt, toReplace);
						
					}

				}	
				for(int i=0;i<nonClashing->Count;i++)
				{
					XbimGeometryModel^ cluster = nonClashing->Shape(i);
					if(dynamic_cast<XbimGeometryModelCollection^>(cluster))
					{
						XbimGeometryModelCollection^ group = (XbimGeometryModelCollection^)cluster;
						XbimGeometryModel^ first = group->FirstOrDefault;
						group->Remove(first);
						XbimGeometryModel^ result = first->Union(group,precision,precisionMax);
						nonClashing->Replace(i,result);
					}
				}
				return nonClashing;
			}

			//Cuts an  opening collection from the base shape
			XbimGeometryModel^ XbimFeaturedShape::SubtractFrom(XbimGeometryModel^ base, XbimGeometryModelCollection^ openings, double deflection, double precision,double precisionMax)
			{		

				if(dynamic_cast<XbimGeometryModelCollection^>(base) && ((XbimGeometryModelCollection^)base)->Count>0) //it is a collection and not a single facettedshell  
				{
					XbimGeometryModelCollection^ result = gcnew	XbimGeometryModelCollection(base->RepresentationLabel,base->SurfaceStyleLabel);
					for each (XbimGeometryModel^ geom in (XbimGeometryModelCollection^)base)
					{
						XbimGeometryModel^ cut = this->SubtractFrom(geom, openings, deflection, precision,precisionMax);
						cut->RepresentationLabel = geom->RepresentationLabel;
						cut->SurfaceStyleLabel = geom->SurfaceStyleLabel;
						result->Add(cut);
					}
					return result;
				}
				else
				{
					
					XbimGeometryModelCollection^ preparedOpenings = PrepareFeatures(openings, precision,precisionMax);
					base->ToSolid(precision,precisionMax);
#if defined USE_CARVE
					if(preparedOpenings->Count<1) //should be quite efficient
					{

						XbimGeometryModel^ result = base;
						for each (XbimGeometryModel^ var in preparedOpenings)
						{
							result = result->Cut(var,precision, precisionMax);
						}
						return result;
					}
					else //use carve for speed
					{

						XbimPolyhedron^ polyBase = base->ToPolyHedron(deflection,precision ,precisionMax);
						XbimPolyhedron^ polyResult = polyBase;
						double currentPolyhedronPrecision = precision;
						XbimCsg^ csg = gcnew XbimCsg(currentPolyhedronPrecision);
						bool warned = false;
						for each (XbimGeometryModel^ nonClashing in preparedOpenings) //do the least number first for performance
						{
							XbimPolyhedron^ polyOpenings = nonClashing->ToPolyHedron(deflection, precision,precisionMax);
TryCutPolyhedron:						
							try
							{
								XbimPolyhedron^ nextResult = csg->Subtract(polyResult,polyOpenings);
								if(nextResult->IsValid)
									polyResult = nextResult;
							}
							catch (XbimGeometryException^ ex) 
							{
								if(currentPolyhedronPrecision<=precisionMax)
								{
									currentPolyhedronPrecision*=10;
									Logger->WarnFormat("Precision adjusted to {0}, declared {1}", currentPolyhedronPrecision,precision);
									csg = gcnew XbimCsg(currentPolyhedronPrecision);
									goto TryCutPolyhedron;
								}
								//return base->Cut(preparedOpenings,precision, precisionMax); //have one last go with open cascade
								if(!warned) Logger->ErrorFormat("Failed to cut opening, exception: {0}.\n Body is #{1}.\nCut ignored. This should not happen.", ex->Message, base->RepresentationLabel);
								warned=true;
							}
						}
						GC::KeepAlive(csg);
						return polyResult;
					}

#else
					XbimGeometryModel^ result = base;
					result = base->Cut(preparedOpenings,precision, precisionMax);
					return result;
#endif
					//					
				}
			}

			void XbimFeaturedShape::ToSolid(double precision, double maxPrecision) 
			{
				throw(gcnew XbimGeometryException("XbimFeaturedShape::ToSolid is not implemented"));
			}

			XbimGeometryModel^ XbimFeaturedShape::Cut(XbimGeometryModel^ shape, double precision, double maxPrecision)
			{

				//BRepAlgoAPI_Cut boolOp(*(mResultShape->Handle),*(shape->Handle));

				//if(boolOp.ErrorStatus() == 0) //find the solid
				//{ 
				//	const TopoDS_Shape & res = boolOp.Shape();
				//	if(res.ShapeType() == TopAbs_SOLID)
				//		return gcnew XbimSolid(TopoDS::Solid(res), HasCurvedEdges);
				//	else if(res.ShapeType() == TopAbs_SHELL)	
				//		return gcnew XbimShell(TopoDS::Shell(res), HasCurvedEdges);
				//	else if(res.ShapeType() == TopAbs_COMPOUND || res.ShapeType() == TopAbs_COMPSOLID)
				//		for (TopExp_Explorer solidEx(res,TopAbs_SOLID) ; solidEx.More(); solidEx.Next())  
				//			return gcnew XbimSolid(TopoDS::Solid(solidEx.Current()), HasCurvedEdges);
				//}
				Logger->Warn("Failed to form difference between two shapes");
				return nullptr;
			}
			XbimGeometryModel^ XbimFeaturedShape::Union(XbimGeometryModel^ shape, double precision, double maxPrecision)
			{
				//BRepAlgoAPI_Fuse boolOp(*(mResultShape->Handle),*(shape->Handle));

				//if(boolOp.ErrorStatus() == 0) //find the solid
				//{ 
				//	const TopoDS_Shape & res = boolOp.Shape();
				//	if(res.ShapeType() == TopAbs_SOLID)
				//		return gcnew XbimSolid(TopoDS::Solid(res), HasCurvedEdges);
				//	else if(res.ShapeType() == TopAbs_SHELL)	
				//		return gcnew XbimShell(TopoDS::Shell(res), HasCurvedEdges);
				//	else if(res.ShapeType() == TopAbs_COMPOUND || res.ShapeType() == TopAbs_COMPSOLID)
				//		for (TopExp_Explorer solidEx(res,TopAbs_SOLID) ; solidEx.More(); solidEx.Next())  
				//			return gcnew XbimSolid(TopoDS::Solid(solidEx.Current()), HasCurvedEdges);
				//}
				Logger->Warn("Failed to form union between two shapes");
				return nullptr;
			}

			XbimGeometryModel^ XbimFeaturedShape::Intersection(XbimGeometryModel^ shape, double precision, double maxPrecision)
			{
				//BRepAlgoAPI_Common boolOp(*(mResultShape->Handle),*(shape->Handle));

				//if(boolOp.ErrorStatus() == 0) //find the solid
				//{ 
				//	const TopoDS_Shape & res = boolOp.Shape();
				//	if(res.ShapeType() == TopAbs_SOLID)
				//		return gcnew XbimSolid(TopoDS::Solid(res), HasCurvedEdges);
				//	else if(res.ShapeType() == TopAbs_SHELL)	
				//		return gcnew XbimShell(TopoDS::Shell(res), HasCurvedEdges);
				//	else if(res.ShapeType() == TopAbs_COMPOUND || res.ShapeType() == TopAbs_COMPSOLID)
				//		for (TopExp_Explorer solidEx(res,TopAbs_SOLID) ; solidEx.More(); solidEx.Next())  
				//			return gcnew XbimSolid(TopoDS::Solid(solidEx.Current()), HasCurvedEdges);
				//}
				Logger->Warn("Failed to form Intersection between two shapes");
				return nullptr;
			}

			XbimFeaturedShape::XbimFeaturedShape(XbimFeaturedShape^ copy, IfcAxis2Placement^ location)
			{
				_representationLabel = copy->RepresentationLabel;
				_surfaceStyleLabel = copy->SurfaceStyleLabel;

				TopoDS_Shape movedShape = *(copy->mResultShape->Handle);
				IfcLocalPlacement^ lp = (IfcLocalPlacement^)location;
				movedShape.Move(XbimGeomPrim::ToLocation(lp->RelativePlacement));

				if(movedShape.ShapeType() == TopAbs_SOLID)
					mResultShape = gcnew XbimSolid(movedShape, HasCurvedEdges,copy->RepresentationLabel,copy->SurfaceStyleLabel);
				else if(movedShape.ShapeType() == TopAbs_COMPOUND || movedShape.ShapeType() == TopAbs_COMPSOLID)
				{
					for (TopExp_Explorer solidEx(movedShape,TopAbs_SOLID) ; solidEx.More(); solidEx.Next())  
					{
						mResultShape = gcnew XbimSolid(TopoDS::Solid(solidEx.Current()), HasCurvedEdges,copy->RepresentationLabel,copy->SurfaceStyleLabel);
						break;
					}
				}
				mBaseShape = copy->mBaseShape;
				mOpenings = copy->mOpenings;
				mProjections = copy->mProjections;
				_hasCurvedEdges = copy->HasCurvedEdges;
				if(mResultShape == nullptr)
					throw(gcnew XbimGeometryException("XbimFeaturedShape::CopyTo has failed to move shape"));

			}

			XbimGeometryModel^ XbimFeaturedShape::CopyTo(IfcAxis2Placement^ placement)
			{
				return gcnew XbimFeaturedShape(this,placement);
			}

			void XbimFeaturedShape::Move(TopLoc_Location location)
			{
				mResultShape->Move(location);
			}

		}
	}
}
