#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcElectricGeneratorTypes.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System.Collections.Generic; using Xbim.XbimExtensions.Interfaces;
using Xbim.Ifc2x3.ElectricalDomain;

#endregion

namespace Xbim.XbimExtensions.DataProviders
{
    public class IfcElectricGeneratorTypes
    {
        private readonly IModel _model;

        public IfcElectricGeneratorTypes(IModel model)
        {
            this._model = model;
        }

        public IEnumerable<IfcElectricGeneratorType> Items
        {
            get { return this._model.InstancesOfType<IfcElectricGeneratorType>(); }
        }
    }
}