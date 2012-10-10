#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcFacetedBreps.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System.Collections.Generic;
using Xbim.Ifc.GeometricModelResource;

#endregion

namespace Xbim.XbimExtensions.DataProviders
{
    public class IfcFacetedBreps
    {
        private readonly IModel _model;

        public IfcFacetedBreps(IModel model)
        {
            this._model = model;
        }

        public IEnumerable<IfcFacetedBrep> Items
        {
            get { return this._model.InstancesOfType<IfcFacetedBrep>(); }
        }
    }
}