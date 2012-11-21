#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcMaterialLayers.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System.Collections.Generic;
using Xbim.Ifc.MaterialResource;

#endregion

namespace Xbim.XbimExtensions.DataProviders
{
    public class IfcMaterialLayers
    {
        private readonly IModel _model;

        public IfcMaterialLayers(IModel model)
        {
            this._model = model;
        }

        public IEnumerable<IfcMaterialLayer> Items
        {
            get { return this._model.InstancesOfType<IfcMaterialLayer>(); }
        }
    }
}