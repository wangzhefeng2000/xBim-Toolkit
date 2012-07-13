#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcCircleProfileDefs.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System.Collections.Generic; using Xbim.XbimExtensions.Interfaces;
using Xbim.Ifc2x3.ProfileResource;

#endregion

namespace Xbim.XbimExtensions.DataProviders
{
    public class IfcCircleProfileDefs
    {
        private readonly IModel _model;

        public IfcCircleProfileDefs(IModel model)
        {
            this._model = model;
        }

        public IEnumerable<IfcCircleProfileDef> Items
        {
            get { return this._model.InstancesOfType<IfcCircleProfileDef>(); }
        }

        public IfcCircleHollowProfileDefs IfcCircleHollowProfileDefs
        {
            get { return new IfcCircleHollowProfileDefs(_model); }
        }
    }
}