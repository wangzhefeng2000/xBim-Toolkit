﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Xbim.Common.Geometry
{
    public struct XbimRect3D
    {
        private static readonly XbimRect3D _empty;

        public static XbimRect3D Empty
        {
            get { return XbimRect3D._empty; }
        } 

        #region Underlying Coordinate properties
        private float _x;
        private float _y;
        private float _z;
        private float _sizeX;
        private float _sizeY;
        private float _sizeZ;
       
        public float SizeX
        {
            get { return _sizeX; }
            set { _sizeX = value; }
        }


        public float SizeY
        {
            get { return _sizeY; }
            set { _sizeY = value; }
        }
        
        public float SizeZ
        {
            get { return _sizeZ; }
            set { _sizeZ = value; }
        }
      

        public XbimPoint3D Location
        {
            get
            {
                return new XbimPoint3D(_x, _y, _z);
            }
            set
            {
                this._x = value.X;
                this._y = value.Y;
                this._z = value.Z;
            }
        }
        
        
        public float X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }
        public float Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }
        public float Z
        {
            get
            {
                return _z;
            }
            set
            {
                _z = value;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return SizeX < 0.0;
            }
        }
        #endregion

        #region Constructors
        public XbimRect3D(float x, float y, float z, float sizeX, float sizeY, float sizeZ)
        {
            _x = x;
            _y = y;
            _z = z;
            _sizeX = sizeX;
            _sizeY = sizeY;
            _sizeZ = sizeZ;
        }

        public XbimRect3D(XbimPoint3D Position, XbimVector3D Size)
        {
            this._x = Position.X;
            this._y = Position.Y;
            this._z = Position.Z;
            this._sizeX = Size.X;
            this._sizeY = Size.Y;
            this._sizeZ = Size.Z;
        }

        public XbimRect3D(XbimPoint3D p1, XbimPoint3D p2)
        {
            this._x = Math.Min(p1.X, p2.X);
            this._y = Math.Min(p1.Y, p2.Y);
            this._z = Math.Min(p1.Z, p2.Z);
            this._sizeX = Math.Max(p1.X, p2.X) - this._x;
            this._sizeY = Math.Max(p1.Y, p2.Y) - this._y;
            this._sizeZ = Math.Max(p1.Z, p2.Z) - this._z;
        }

        static  XbimRect3D()
        {
            _empty = new XbimRect3D { _x = float.PositiveInfinity, _y = float.PositiveInfinity, _z = float.PositiveInfinity, _sizeX = float.NegativeInfinity, _sizeY = float.NegativeInfinity, _sizeZ = float.NegativeInfinity };
        }

        public XbimRect3D(XbimPoint3D highpt)
        {
            _x = highpt.X;
            _y = highpt.Y;
            _z = highpt.Z;
            _sizeX = (float)0.0;
            _sizeY = (float)0.0;
            _sizeZ = (float)0.0;
        }

        public XbimRect3D(XbimVector3D vMin, XbimVector3D vMax)
        {
            this._x = Math.Min(vMin.X, vMax.X);
            this._y = Math.Min(vMin.Y, vMax.Y);
            this._z = Math.Min(vMin.Z, vMax.Z);
            this._sizeX = Math.Max(vMin.X, vMax.X) - this._x;
            this._sizeY = Math.Max(vMin.Y, vMax.Y) - this._y;
            this._sizeZ = Math.Max(vMin.Z, vMax.Z) - this._z;
        }
        
        #endregion

        /// <summary>
        /// Minimum vertex
        /// </summary>
        public XbimPoint3D Min
        {
            get
            {
                return new XbimPoint3D(_x+_sizeX,_y+_sizeY,_z+_sizeZ);
            }
        }
        /// <summary>
        /// Maximum vertex
        /// </summary>
        public XbimPoint3D Max
        {
            get
            {
                return this.Location;
            }
        }

        public double Volume
        {
            get
            {
                return _sizeX * _sizeY * _sizeZ;
            }
        }

        #region Serialisation

        /// <summary>
        /// Reinitialises the rectangle 3D from the byte array
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="array">6 doubles, definine, min and max values of the boudning box</param>
        public static XbimRect3D FromArray(byte[] array)
        {
            MemoryStream ms = new MemoryStream(array);
            BinaryReader bw = new BinaryReader(ms);
            XbimRect3D rect = new XbimRect3D();
            float srXmin = (float)bw.ReadDouble(); //legacy when using windows rect3d and doubles
            float srYmin = (float)bw.ReadDouble();
            float srZmin = (float)bw.ReadDouble();
            rect.Location = new XbimPoint3D(srXmin, srYmin, srZmin);

            float srXSz = (float)bw.ReadDouble(); // all ToArray functions store position and size (bugfix: it was previously reading data as max)
            float srYSz = (float)bw.ReadDouble();
            float srZSz = (float)bw.ReadDouble();
            rect.SizeX = srXSz;
            rect.SizeY = srYSz;
            rect.SizeZ = srZSz;

            return rect;
        }

        /// <summary>
        /// Writes the Bounding Box as 6 doubles.
        /// </summary>
        /// <returns>An array of doubles (Position followed by Size).</returns>
        public byte[] ToDoublesArray()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write((double)_x);
            bw.Write((double)_y);
            bw.Write((double)_z);
            bw.Write((double)_sizeX);
            bw.Write((double)_sizeY);
            bw.Write((double)_sizeZ);
            bw.Close();
            return ms.ToArray();
        }

        /// <summary>
        /// Writes the Bounding Box as 6 floats.
        /// </summary>
        /// <returns>An array of floats (Position followed by Size).</returns>
        public byte[] ToFloatArray()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write(_x);
            bw.Write(_y);
            bw.Write(_z);
            bw.Write(_sizeX);
            bw.Write(_sizeY);
            bw.Write(_sizeZ);
            bw.Close();
            return ms.ToArray();
        }

        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} {1} {2} {3} {4} {5}", 
                _x.ToString("f99").TrimEnd(new char[] {'0', ','}),
                _y.ToString("f99").TrimEnd(new char[] { '0', ',' }),
                _z.ToString("f99").TrimEnd(new char[] { '0', ',' }),
                _sizeX.ToString("f99").TrimEnd(new char[] { '0', ',' }),
                _sizeY.ToString("f99").TrimEnd(new char[] { '0', ',' }),
                _sizeZ.ToString("f99").TrimEnd(new char[] {'0', ','})
                );
        }

        /// <summary>
        /// Imports values from a string
        /// </summary>
        /// <param name="Value">A space-separated string of 6 invariant-culture-formatted floats (x,y,z,sizeX,sizeY,sizeZ)</param>
        /// <returns>True if successful.</returns>
        public bool FromString(string Value)
        {
            string[] itms = Value.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (itms.Length != 6)
                return false;

            double[] vals = new double[6];
            try
            {
                for (int i = 0; i < 6; i++)
                {
                    vals[i] = Convert.ToDouble(itms[i], System.Globalization.CultureInfo.InvariantCulture);    
                }   
                
            }
            catch (Exception)
            {
                return false;
            }

            _x = (float)vals[0];
            _y = (float)vals[1];
            _z = (float)vals[2];
           
            _sizeX = (float)vals[3];
            _sizeY = (float)vals[4];
            _sizeZ = (float)vals[5];

            return true;
        }
        #endregion

        static public XbimRect3D Inflate( double x, double y, double z)
        {
            XbimRect3D rect = new XbimRect3D();
            rect.X -= (float)x; rect.Y -= (float)y; rect.Z -= (float)z;
            rect.SizeX += (float)x * 2; rect.SizeY += (float)y * 2; rect.SizeZ += (float)z * 2;
            return rect;
        }

        static public XbimRect3D Inflate(float x, float y, float z)
        {
            XbimRect3D rect = new XbimRect3D();
            rect.X -= x; rect.Y -= y; rect.Z -= z;
            rect.SizeX += x * 2; rect.SizeY += y * 2; rect.SizeZ += z * 2;
            return rect;
        }

        static public XbimRect3D Inflate( double d)
        {
            XbimRect3D rect = new XbimRect3D();
            rect.X -= (float)d; rect.Y -= (float)d; rect.Z -= (float)d;
            rect.SizeX += (float)d * 2; rect.SizeY += (float)d * 2; rect.SizeZ += (float)d * 2;
            return rect;
        }

        static public XbimRect3D Inflate(float d)
        {
            XbimRect3D rect = new XbimRect3D();
            rect.X -= d; rect.Y -= d; rect.Z -= d;
            rect.SizeX += d * 2; rect.SizeY += d * 2; rect.SizeZ += d * 2;
            return rect;
        }

        /// <summary>
        /// Calculates the centre of the 3D rect
        /// </summary>
        public XbimPoint3D Centroid()
        {
            if (IsEmpty) 
                return new XbimPoint3D(0, 0, 0);
            else
                return new XbimPoint3D((X + SizeX / 2), (Y + SizeY / 2), (Z + SizeZ / 2));
        }


        static public XbimRect3D TransformBy(XbimRect3D rect3d, XbimMatrix3D m)
        {
            XbimPoint3D min = rect3d.Min;
            XbimPoint3D max = rect3d.Max;
            XbimVector3D up = m.Up;
            XbimVector3D right = m.Right;
            XbimVector3D backward = m.Backward;
            var xa = right * min.X;
            var xb = right * max.X;

            var ya = up * min.Y;
            var yb = up * max.Y;

            var za = backward * min.Z;
            var zb = backward * max.Z;

            return new XbimRect3D(
                XbimVector3D.Min(xa, xb) + XbimVector3D.Min(ya, yb) + XbimVector3D.Min(za, zb) + m.Translation,
                XbimVector3D.Max(xa, xb) + XbimVector3D.Max(ya, yb) + XbimVector3D.Max(za, zb) + m.Translation
            );
            
        }

        public void Union(XbimRect3D bb)
        {
            if (IsEmpty)
            {
                this = bb;
            }
            else if (!bb.IsEmpty)
            {
                float numX = Math.Min(X, bb.X);
                float numY = Math.Min(Y, bb.Y);
                float numZ = Math.Min(Z, bb.Z);
                _sizeX = Math.Max((float)(X + _sizeX), (float)(bb.X + bb._sizeX)) - numX;
                _sizeY = Math.Max((float)(Y + _sizeY), (float)(bb.Y + bb._sizeY)) - numY;
                _sizeZ = Math.Max((float)(Z + _sizeZ), (float)(bb.Z + bb._sizeZ)) - numZ;
                X = numX;
                Y = numY;
                Z = numZ;
            }
        }

        public void Union(XbimPoint3D highpt)
        {
            Union(new XbimRect3D(highpt, highpt));
        }

        public bool Contains(double x, double y, double z)
        {
            if (this.IsEmpty)
            {
                return false;
            }
            return this.ContainsCoords((float)x, (float)y, (float)z);
        }

        public bool Contains(XbimPoint3D pt)
        {
            if (this.IsEmpty)
            {
                return false;
            }
            return this.ContainsCoords(pt.X, pt.Y, pt.Z);
        }

        private bool ContainsCoords(float x, float y, float z)
        {
            return (((((x >= this._x) && (x <= (this._x + this._sizeX))) && ((y >= this._y) && (y <= (this._y + this._sizeY)))) && (z >= this._z)) && (z <= (this._z + this._sizeZ)));
  
        }

        public bool Intersects(XbimRect3D rect)
        {
            if (this.IsEmpty || rect.IsEmpty)
            {
                return false;
            }
            return (((((rect._x <= (this._x + this._sizeX)) && ((rect._x + rect._sizeX) >= this._x)) && ((rect._y <= (this._y + this._sizeY)) && ((rect._y + rect._sizeY) >= this._y))) && (rect._z <= (this._z + this._sizeZ))) && ((rect._z + rect._sizeZ) >= this._z));
        }
       

        public bool Contains(XbimRect3D rect)
        {
            if (this.IsEmpty)
                return false;
            return 
                this.ContainsCoords(rect.X, rect.Y, rect.Z) 
                && 
                this.ContainsCoords(rect.X + rect.SizeX, rect.Y + rect.SizeY, rect.Z+rect.SizeZ);
        }

       /// <summary>
       /// Returns the radius of the sphere that contains this bounding box rectangle 3D
       /// </summary>
       /// <returns></returns>
        public float Radius()
        {
            XbimVector3D max = new XbimVector3D(SizeX, SizeY, SizeZ);
            float len = max.Length;
            if (len != 0)
                return  len / 2;
            else
                return 0;
        }

        /// <summary>
        /// Indicative size of the Box along all axis.
        /// </summary>
        /// <returns>Returns the length of the diagonal</returns>
        public float Length()
        {
            XbimVector3D max = new XbimVector3D(SizeX, SizeY, SizeZ);
            return max.Length;
        }

        /// <summary>
        /// Warning: This function assumes no rotation is used for the tranform.
        /// </summary>
        /// <param name="composed">The NON-ROTATING transform to apply</param>
        /// <returns>the transformed bounding box.</returns>
        public XbimRect3D Transform(XbimMatrix3D composed)
        {
            var min = this.Min * composed;
            var max = this.Max * composed;

            return new XbimRect3D(min, max);
        }
    }
}
