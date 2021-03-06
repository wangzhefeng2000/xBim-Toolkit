#include "StdAfx.h"
#include "XbimMap.h"

#include "XbimPolyhedron.h"

using namespace System::Collections::Generic;
using namespace Xbim::XbimExtensions;
using namespace Xbim::Ifc2x3::Extensions;
using namespace System::Linq;
using namespace Xbim::Common::Exceptions;
using namespace  System::Threading;

namespace Xbim
{
	namespace ModelGeometry
	{
		namespace OCC
		{

			XbimPolyhedron^ XbimMap::ToPolyHedron(double deflection, double precision, double precisionMax)
			{
				XbimPolyhedron^ poly = _mappedItem->ToPolyHedron(deflection, precision,precisionMax);
				poly->Transform(_transform);
				return poly;
			}
			XbimMap::XbimMap(const TopoDS_Shape& shape,XbimMap^ copy)
			{
				_mappedItem = copy->MappedItem;
				_hasCurvedEdges=copy->HasCurvedEdges;
				_representationLabel = copy->RepresentationLabel;
				_surfaceStyleLabel = copy->SurfaceStyleLabel;
			}
			XbimMap::XbimMap(XbimGeometryModel^ item, IfcAxis2Placement^ origin, IfcCartesianTransformationOperator^ transform, ConcurrentDictionary<int,Object^>^ maps)
			{
				_mappedItem = item;
				_hasCurvedEdges=item->HasCurvedEdges;
				_representationLabel = item->RepresentationLabel;
				_surfaceStyleLabel = item->SurfaceStyleLabel;
				_origin = origin;
				_cartTransform = transform;
		
				
				_transform=XbimMatrix3D::Identity;
				if(origin !=nullptr)
				{
					if(dynamic_cast<IfcAxis2Placement3D^>(origin))
						_transform = Axis2Placement3DExtensions::ToMatrix3D((IfcAxis2Placement3D^)origin,maps);
					else if(dynamic_cast<IfcAxis2Placement2D^>(origin))
						_transform = Axis2Placement2DExtensions::ToMatrix3D((IfcAxis2Placement2D^)origin,maps);
					else
						throw gcnew XbimGeometryException("Invalid IfcAxis2Placement argument");

				}

				if(transform!=nullptr)
					_transform= XbimMatrix3D::Multiply( CartesianTransformationOperatorExtensions::ToMatrix3D(transform, maps),_transform);
				
			}

			XbimGeometryModel^ XbimMap::CopyTo(IfcAxis2Placement^ placement)
			{

				TopoDS_Shape movedShape = *nativeHandle;
				movedShape.Move(XbimGeomPrim::ToLocation(placement));
				XbimMap^ map = gcnew XbimMap(movedShape,this);
				return map;

			}

			XbimTriangulatedModelCollection^ XbimMap::Mesh(double deflection)
			{
				if(theMesh==nullptr)
				{
					Monitor::Enter(_mappedItem);
					try
					{
						theMesh = _mappedItem->Mesh(deflection);	
					}
					finally
					{
						Monitor::Exit(_mappedItem);
					}
				}
				return theMesh;
			}

			void XbimMap::ToSolid(double precision, double maxPrecision) 
			{
				_mappedItem->ToSolid(precision, maxPrecision);
			}
		}
	}
}
