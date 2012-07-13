#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcObjectPlacements.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System.Collections.Generic; using Xbim.XbimExtensions.Interfaces;
using Xbim.Ifc2x3.GeometricConstraintResource;

#endregion

namespace Xbim.XbimExtensions.DataProviders
{
    public class IfcObjectPlacements
    {
        private readonly IModel _model;

        public IfcObjectPlacements(IModel model)
        {
            this._model = model;
        }

        public IEnumerable<IfcObjectPlacement> Items
        {
            get { return this._model.InstancesOfType<IfcObjectPlacement>(); }
        }

        public IfcLocalPlacements IfcLocalPlacements
        {
            get { return new IfcLocalPlacements(_model); }
        }

        public IfcGridPlacements IfcGridPlacements
        {
            get { return new IfcGridPlacements(_model); }
        }
    }
}