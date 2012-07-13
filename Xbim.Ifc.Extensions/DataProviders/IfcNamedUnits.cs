#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcNamedUnits.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System.Collections.Generic; using Xbim.XbimExtensions.Interfaces;
using Xbim.Ifc2x3.MeasureResource;

#endregion

namespace Xbim.XbimExtensions.DataProviders
{
    public class IfcNamedUnits
    {
        private readonly IModel _model;

        public IfcNamedUnits(IModel model)
        {
            this._model = model;
        }

        public IEnumerable<IfcNamedUnit> Items
        {
            get { return this._model.InstancesOfType<IfcNamedUnit>(); }
        }

        public IfcContextDependentUnits IfcContextDependentUnits
        {
            get { return new IfcContextDependentUnits(_model); }
        }

        public IfcSIUnits IfcSIUnits
        {
            get { return new IfcSIUnits(_model); }
        }

        public IfcConversionBasedUnits IfcConversionBasedUnits
        {
            get { return new IfcConversionBasedUnits(_model); }
        }
    }
}