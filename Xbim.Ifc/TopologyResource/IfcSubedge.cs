﻿#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcSubedge.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives

using System;
using Xbim.XbimExtensions;

#endregion

namespace Xbim.Ifc.TopologyResource
{
    [IfcPersistedEntity, Serializable]
    public class IfcSubedge : IfcEdge
    {
        private IfcEdge _parentEdge;

        /// <summary>
        ///   The Edge, or Subedge, which contains the Subedge.
        /// </summary>
        public IfcEdge ParentEdge
        {
            get { return _parentEdge; }
            set { _parentEdge = value; }
        }
    }
}