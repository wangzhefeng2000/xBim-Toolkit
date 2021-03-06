﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xbim.SceneJSWebViewer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Xbim.Common.Geometry;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BoundingBox
    {
        public bool IsValid = false;
        public XbimPoint3D PointMin = new XbimPoint3D();
        public XbimPoint3D PointMax = new XbimPoint3D();

        public BoundingBox()
        {

        }

        public BoundingBox(XbimPoint3D pMin, XbimPoint3D pMax)
        {
            PointMin = pMin;
            PointMax = pMax;
            IsValid = true;
        }
        public BoundingBox(double srXmin, double srYmin, double srZmin, double srXmax, double srYmax, double srZmax )
        {
            PointMin = new XbimPoint3D(srXmin, srYmin, srZmin);
            PointMax = new XbimPoint3D(srXmax, srYmax, srZmax);
            IsValid = true;
        }
        /// <summary>
        /// Returns a bounding box from the byte array
        /// </summary>
        /// <returns></returns>
        public static BoundingBox FromArray(byte[] array)
        {
            MemoryStream ms = new MemoryStream(array);
            BinaryReader bw = new BinaryReader(ms);

            double minX = bw.ReadDouble();
            double minY = bw.ReadDouble();
            double minZ = bw.ReadDouble();
            double maxX = bw.ReadDouble();
            double maxY = bw.ReadDouble();
            double maxZ = bw.ReadDouble();

            return new BoundingBox(minX, minY, minZ, maxX, maxY, maxZ);

        }
        /// <summary>
        /// Extends to include the specified point; returns true if boundaries were changed.
        /// </summary>
        /// <param name="Point"></param>
        /// <returns></returns>
        internal bool IncludePoint(XbimPoint3D Point)
        {
            if (!IsValid)
            {
                PointMin = Point;
                PointMax = Point;
                IsValid = true;
                return true;
            }
            bool ret = false;

            if (PointMin.X > Point.X)
            { PointMin.X = Point.X; ret = true; }
            if (PointMin.Y > Point.Y)
            { PointMin.Y = Point.Y; ret = true; }
            if (PointMin.Z > Point.Z)
            { PointMin.Z = Point.Z; ret = true; }

            if (PointMax.X < Point.X)
            { PointMax.X = Point.X; ret = true; }
            if (PointMax.Y < Point.Y)
            { PointMax.Y = Point.Y; ret = true; }
            if (PointMax.Z < Point.Z)
            { PointMax.Z = Point.Z; ret = true; }

            IsValid = true;

            return ret;
        }

        internal void IncludeBoundingBox(BoundingBox childBB)
        {
            if (!childBB.IsValid)
                return;
            this.IncludePoint(childBB.PointMin);
            this.IncludePoint(childBB.PointMax);
        }
        
        public BoundingBox TransformBy(XbimMatrix3D m)
        {
            return new BoundingBox(m.Transform(PointMin), m.Transform(PointMax));
        }

        private bool IncludePoint(XbimPoint3D Point, XbimMatrix3D Matrix)
        {
            XbimPoint3D t = XbimPoint3D.Multiply(Point, Matrix);
            return IncludePoint(t);
        }
    }
}