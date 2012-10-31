﻿#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcTShapeProfileDef.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System;
using Xbim.Ifc.MeasureResource;
using Xbim.XbimExtensions;
using Xbim.XbimExtensions.Parser;

#endregion

namespace Xbim.Ifc.ProfileResource
{
    [IfcPersistedEntity, Serializable]
    public class IfcTShapeProfileDef : IfcParameterizedProfileDef
    {
        #region Fields 

        private IfcPositiveLengthMeasure _depth;
        private IfcPositiveLengthMeasure _flangeWidth;
        private IfcPositiveLengthMeasure _webThickness;
        private IfcPositiveLengthMeasure _flangeThickness;
        private IfcPositiveLengthMeasure? _filletRadius;
        private IfcPositiveLengthMeasure? _flangeEdgeRadius;
        private IfcPositiveLengthMeasure? _webEdgeRadius;
        private IfcPlaneAngleMeasure? _webSlope;
        private IfcPlaneAngleMeasure? _flangeSlope;
        private IfcPositiveLengthMeasure? _centreOfGravityInY;

        #endregion

        #region Properties

        [IfcAttribute(4, IfcAttributeState.Mandatory)]
        public IfcPositiveLengthMeasure Depth
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _depth;
            }
            set { ModelManager.SetModelValue(this, ref _depth, value, v => Depth = v, "Depth"); }
        }

        [IfcAttribute(5, IfcAttributeState.Mandatory)]
        public IfcPositiveLengthMeasure FlangeWidth
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _flangeWidth;
            }
            set { ModelManager.SetModelValue(this, ref _flangeWidth, value, v => FlangeWidth = v, "FlangeWidth"); }
        }

        [IfcAttribute(6, IfcAttributeState.Mandatory)]
        public IfcPositiveLengthMeasure WebThickness
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _webThickness;
            }
            set { ModelManager.SetModelValue(this, ref _webThickness, value, v => WebThickness = v, "WebThickness"); }
        }

        [IfcAttribute(7, IfcAttributeState.Mandatory)]
        public IfcPositiveLengthMeasure FlangeThickness
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _flangeThickness;
            }
            set
            {
                ModelManager.SetModelValue(this, ref _flangeThickness, value, v => FlangeThickness = v,
                                           "FlangeThickness");
            }
        }

        [IfcAttribute(8, IfcAttributeState.Optional)]
        public IfcPositiveLengthMeasure? FilletRadius
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _filletRadius;
            }
            set { ModelManager.SetModelValue(this, ref _filletRadius, value, v => FilletRadius = v, "FilletRadius"); }
        }

        [IfcAttribute(9, IfcAttributeState.Optional)]
        public IfcPositiveLengthMeasure? FlangeEdgeRadius
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _flangeEdgeRadius;
            }
            set
            {
                ModelManager.SetModelValue(this, ref _flangeEdgeRadius, value, v => FlangeEdgeRadius = v,
                                           "FlangeEdgeRadius");
            }
        }

        [IfcAttribute(10, IfcAttributeState.Optional)]
        public IfcPositiveLengthMeasure? WebEdgeRadius
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _webEdgeRadius;
            }
            set { ModelManager.SetModelValue(this, ref _webEdgeRadius, value, v => WebEdgeRadius = v, "WebEdgeRadius"); }
        }

        [IfcAttribute(11, IfcAttributeState.Optional)]
        public IfcPlaneAngleMeasure? WebSlope
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _webSlope;
            }
            set { ModelManager.SetModelValue(this, ref _webSlope, value, v => WebSlope = v, "WebSlope"); }
        }

        [IfcAttribute(12, IfcAttributeState.Optional)]
        public IfcPlaneAngleMeasure? FlangeSlope
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _flangeSlope;
            }
            set { ModelManager.SetModelValue(this, ref _flangeSlope, value, v => FlangeSlope = v, "FlangeSlope"); }
        }

        [IfcAttribute(13, IfcAttributeState.Optional)]
        public IfcPositiveLengthMeasure? CentreOfGravityInY
        {
            get
            {
#if SupportActivation
                ((IPersistIfcEntity) this).Activate(false);
#endif
                return _centreOfGravityInY;
            }
            set
            {
                ModelManager.SetModelValue(this, ref _centreOfGravityInY, value, v => CentreOfGravityInY = v,
                                           "CentreOfGravityInY");
            }
        }

        #endregion

        public override void IfcParse(int propIndex, IPropertyValue value)
        {
            switch (propIndex)
            {
                case 0:
                case 1:
                case 2:
                    base.IfcParse(propIndex, value);
                    break;
                case 3:
                    _depth = value.RealVal;
                    break;
                case 4:
                    _flangeWidth = value.RealVal;
                    break;
                case 5:
                    _webThickness = value.RealVal;
                    break;
                case 6:
                    _flangeThickness = value.RealVal;
                    break;
                case 7:
                    _filletRadius = value.RealVal;
                    break;
                case 8:
                    _flangeEdgeRadius = value.RealVal;
                    break;
                case 9:
                    _webEdgeRadius = value.RealVal;
                    break;
                case 10:
                    _webSlope = value.RealVal;
                    break;
                case 11:
                    _flangeSlope = value.RealVal;
                    break;
                case 12:
                    _centreOfGravityInY = value.RealVal;
                    break;

                default:
                    this.HandleUnexpectedAttribute(propIndex, value); break;
            }
        }

        public override string WhereRule()
        {
            string err = "";
            if (_flangeThickness >= _depth)
                err += "WR1 TShapeProfileDef : The flange thickness shall be smaller than the depth\n";
            if (_flangeThickness >= _flangeWidth)
                err += "WR2 TShapeProfileDef : The web thickness shall be smaller than the flange width.\n";
            return err;
        }
    }
}