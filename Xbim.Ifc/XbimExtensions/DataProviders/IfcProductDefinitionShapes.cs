#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcProductDefinitionShapes.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System.Collections.Generic;
using Xbim.Ifc.RepresentationResource;

#endregion

namespace Xbim.XbimExtensions.DataProviders
{
    public class IfcProductDefinitionShapes
    {
        private readonly IModel _model;

        public IfcProductDefinitionShapes(IModel model)
        {
            this._model = model;
        }

        public IEnumerable<IfcProductDefinitionShape> Items
        {
            get { return this._model.InstancesOfType<IfcProductDefinitionShape>(); }
        }
    }
}