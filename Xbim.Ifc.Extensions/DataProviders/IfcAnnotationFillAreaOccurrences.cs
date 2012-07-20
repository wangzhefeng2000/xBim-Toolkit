#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcAnnotationFillAreaOccurrences.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System.Collections.Generic; using Xbim.XbimExtensions.Interfaces;
using Xbim.Ifc2x3.PresentationDefinitionResource;

#endregion

namespace Xbim.XbimExtensions.DataProviders
{
    public class IfcAnnotationFillAreaOccurrences
    {
        private readonly IModel _model;

        public IfcAnnotationFillAreaOccurrences(IModel model)
        {
            this._model = model;
        }

        public IEnumerable<IfcAnnotationFillAreaOccurrence> Items
        {
            get { return this._model.InstancesOfType<IfcAnnotationFillAreaOccurrence>(); }
        }
    }
}