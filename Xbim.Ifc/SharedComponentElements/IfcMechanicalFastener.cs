﻿#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcMechanicalFastener.cs
// Published:   05, 2012
// Last Edited: 13:00 PM on 23 05 2012
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System;
using Xbim.Ifc.MeasureResource;
using Xbim.XbimExtensions;
using Xbim.XbimExtensions.Parser;

#endregion

namespace Xbim.Ifc.SharedComponentElements
{
     [IfcPersistedEntity, Serializable]
    public class IfcMechanicalFastener : IfcFastener
    {
        #region Fields
         IfcPositiveLengthMeasure? _nominalDiameter;
         IfcPositiveLengthMeasure? _nominalLength;
        #endregion

         #region Ifc Properties
         /// <summary>
         /// The nominal diameter describing the cross-section size of the fastener.
         /// </summary>
         [IfcAttribute(9, IfcAttributeState.Optional)]
         public IfcPositiveLengthMeasure? NominalDiameter
         {
             get
             {
#if SupportActivation
                 ((IPersistIfcEntity)this).Activate(false);
#endif
                 return _nominalDiameter;
             }
             set { ModelManager.SetModelValue(this, ref _nominalDiameter, value, v => NominalDiameter = v, "NominalDiameter"); }
         }

         /// <summary>
         /// The nominal length describing the longitudinal dimensions of the fastener.
         /// </summary>
         [IfcAttribute(10, IfcAttributeState.Optional)]
         public IfcPositiveLengthMeasure? NominalLength
         {
             get
             {
#if SupportActivation
                 ((IPersistIfcEntity)this).Activate(false);
#endif
                 return _nominalLength;
             }
             set { ModelManager.SetModelValue(this, ref _nominalLength, value, v => NominalLength = v, "NominalLength"); }
         }
         #endregion

        #region IfcParse

         public override void IfcParse(int propIndex, IPropertyValue value)
         {
             switch (propIndex)
             {
                 case 0:
                 case 1:
                 case 2:
                 case 3:
                 case 4:
                 case 5:
                 case 6:
                 case 7:
                     base.IfcParse(propIndex, value);
                     break;
                 case 8:
                     _nominalDiameter = value.RealVal; break;
                 case 9:
                     _nominalLength = value.RealVal; break;
                 default:
                     this.HandleUnexpectedAttribute(propIndex, value);
                     break;
             }
         }
        #endregion
    }
}
