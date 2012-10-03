#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcFeatureElementSubtractions.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System.Collections.Generic; using Xbim.XbimExtensions.Interfaces;
using Xbim.Ifc2x3.ProductExtension;

#endregion

namespace Xbim.XbimExtensions.DataProviders
{
    public class IfcFeatureElementSubtractions
    {
        private readonly IModel _model;

        public IfcFeatureElementSubtractions(IModel model)
        {
            this._model = model;
        }

        public IEnumerable<IfcFeatureElementSubtraction> Items
        {
            get { return this._model.InstancesOfType<IfcFeatureElementSubtraction>(); }
        }

        public IfcOpeningElements IfcOpeningElements
        {
            get { return new IfcOpeningElements(_model); }
        }
    }
}