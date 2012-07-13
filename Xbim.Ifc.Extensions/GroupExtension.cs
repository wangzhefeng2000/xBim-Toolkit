﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.Ifc2x3.Kernel;
using Xbim.XbimExtensions;
using Xbim.XbimExtensions.Interfaces;

namespace Xbim.Ifc2x3.Extensions
{
    public static class GroupExtension
    {
        private static IModel GetModel(IfcRoot root)
        {
            IModel model = null;
            IPersistIfcEntity persist = root as IPersistIfcEntity;
            if (persist != null) model = persist.ModelOf;
            if (model == null) model = root.ModelOf;

            return model;
        }

        public static void AddObjectToGroup(this IfcGroup gr, IfcObjectDefinition obj)
        {
            IModel model = GetModel(gr);

            IfcRelAssignsToGroup relation = gr.IsGroupedBy;
            if (gr.IsGroupedBy == null) relation = model.New<IfcRelAssignsToGroup>(rel => rel.RelatingGroup = gr);
            relation.RelatedObjects.Add_Reversible(obj);
        }

        public static void AddObjectToGroup(this IfcGroup gr, IEnumerable<IfcObjectDefinition> objects)
        {
            IModel model = GetModel(gr);

            IfcRelAssignsToGroup relation = gr.IsGroupedBy;
            if (gr.IsGroupedBy == null) relation = model.New<IfcRelAssignsToGroup>(rel => rel.RelatingGroup = gr);
            foreach (var item in objects)
            {
                relation.RelatedObjects.Add_Reversible(item);
            }
        }

        public static IEnumerable<IfcObjectDefinition> GetGroupedObjects(this IfcGroup gr)
        {
            IfcRelAssignsToGroup relation = gr.IsGroupedBy;
            if (gr.IsGroupedBy != null) return relation.RelatedObjects;
            return new List<IfcObjectDefinition>();
        }

        public static IEnumerable<T> GetGroupedObjects<T>(this IfcGroup gr) where T:IfcObjectDefinition
        {
            IfcRelAssignsToGroup relation = gr.IsGroupedBy;
            if (gr.IsGroupedBy != null) return relation.RelatedObjects.OfType<T>();
            return new List<T>();
        }

        public static IEnumerable<IfcGroup> GetParentGroups(this IfcGroup gr)
        {
            IModel model = GetModel(gr);

            IEnumerable<IfcRelAssignsToGroup> relations = model.InstancesWhere<IfcRelAssignsToGroup>(rel => rel.RelatedObjects.Contains(gr));
            foreach (IfcRelAssignsToGroup rel in relations)
            {
                yield return rel.RelatingGroup;
            }
        }

        public static bool RemoveObjectFromGroup(this IfcGroup gr, IfcObjectDefinition obj)
        {
            if (gr == null || obj == null) return false;
            IModel model = GetModel(gr);
            IfcRelAssignsToGroup relation = gr.IsGroupedBy;
            if (gr.IsGroupedBy == null) return false;
            if (!relation.RelatedObjects.Contains(obj)) return false;
            relation.RelatedObjects.Remove_Reversible(obj);
            return true;
        }
    }
}
