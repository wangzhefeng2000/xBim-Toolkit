﻿#region XbimHeader

// The eXtensible Building Information Modelling (xBIM) Toolkit
// Solution:    XbimComplete
// Project:     Xbim.Ifc
// Filename:    IfcBooleanOperand.cs
// Published:   01, 2012
// Last Edited: 9:04 AM on 20 12 2011
// (See accompanying copyright.rtf)

#endregion

#region Directives


#endregion

namespace Xbim.XbimExtensions.SelectTypes
{
    public interface IfcBooleanOperand : ExpressSelectType
    {
        int Dim { get; }
    }
}