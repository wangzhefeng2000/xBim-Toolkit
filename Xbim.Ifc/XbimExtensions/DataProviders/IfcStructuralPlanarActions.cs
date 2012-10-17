#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcStructuralPlanarActions.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System.Collections.Generic;
using Xbim.Ifc.StructuralAnalysisDomain;

#endregion

namespace Xbim.XbimExtensions.DataProviders
{
    public class IfcStructuralPlanarActions
    {
        private readonly IModel _model;

        public IfcStructuralPlanarActions(IModel model)
        {
            this._model = model;
        }

        public IEnumerable<IfcStructuralPlanarAction> Items
        {
            get { return this._model.InstancesOfType<IfcStructuralPlanarAction>(); }
        }

        public IfcStructuralPlanarActionVaryings IfcStructuralPlanarActionVaryings
        {
            get { return new IfcStructuralPlanarActionVaryings(_model); }
        }
    }
}