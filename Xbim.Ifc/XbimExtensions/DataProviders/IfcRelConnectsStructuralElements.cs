#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcRelConnectsStructuralElements.cs
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
    public class IfcRelConnectsStructuralElements
    {
        private readonly IModel _model;

        public IfcRelConnectsStructuralElements(IModel model)
        {
            this._model = model;
        }

        public IEnumerable<IfcRelConnectsStructuralElement> Items
        {
            get { return this._model.InstancesOfType<IfcRelConnectsStructuralElement>(); }
        }
    }
}