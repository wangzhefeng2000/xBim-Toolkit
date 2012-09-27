﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBie.Rows;
using Xbim.Ifc.Extensions;
using Xbim.Ifc.MeasureResource;
using Xbim.Ifc.ProductExtension;
using Xbim.Ifc.QuantityResource;
using Xbim.XbimExtensions;
using System.Collections;
using Xbim.Ifc.PropertyResource;
using Xbim.Ifc.Kernel;
using Xbim.Ifc.ExternalReferenceResource;

namespace Xbim.COBie.Data
{
    /// <summary>
    /// Class to input data into excel worksheets for the the Space tab.
    /// </summary>
    public class COBieDataSpace : COBieData<COBieSpaceRow>, IAttributeProvider
    {
        /// <summary>
        /// Data Space constructor
        /// </summary>
        /// <param name="model">The context of the model being generated</param>
        public COBieDataSpace(COBieContext context) : base(context)
        { }

        #region Methods

        /// <summary>
        /// Fill sheet rows for Space sheet
        /// </summary>
        /// <returns>COBieSheet<COBieSpaceRow></returns>
        public override COBieSheet<COBieSpaceRow> Fill()
        {
            ProgressIndicator.ReportMessage("Starting Spaces...");

            //create new sheet 
            COBieSheet<COBieSpaceRow> spaces = new COBieSheet<COBieSpaceRow>(Constants.WORKSHEET_SPACE);

            // get all IfcBuildingStory objects from IFC file
            List<IfcSpace> ifcSpaces = Model.InstancesOfType<IfcSpace>().OrderBy(ifcSpace => ifcSpace.Name, new CompareIfcLabel()).ToList();
            
            COBieDataPropertySetValues allPropertyValues = new COBieDataPropertySetValues(ifcSpaces.OfType<IfcObject>()); //properties helper class
            COBieDataAttributeBuilder attributeBuilder = new COBieDataAttributeBuilder(allPropertyValues);
            attributeBuilder.InitialiseAttributes(ref _attributes);
            

            //list of attributes and property sets to exclude form attribute sheet
            List<string> excludePropertyValueNames = new List<string> { "Area", "Number", "UsableHeight", "RoomTag", "Room Tag" };
            List<string> excludePropertyValueNamesWildcard = new List<string> { "ZoneName", "Category", "Length", "Width"}; //exclude part names
            List<string> excludePropertSetNames = new List<string>() { "BaseQuantities" };
            //set up filters on COBieDataPropertySetValues
            attributeBuilder.ExcludeAttributePropertyNames.AddRange(excludePropertyValueNames);
            attributeBuilder.ExcludeAttributePropertyNamesWildcard.AddRange(excludePropertyValueNamesWildcard);
            attributeBuilder.ExcludeAttributePropertySetNames.AddRange(excludePropertSetNames);
            attributeBuilder.RowParameters["Sheet"] = "Space";

            ProgressIndicator.Initialise("Creating Spaces", ifcSpaces.Count());

            foreach (IfcSpace ifcSpace in ifcSpaces)
            {
                ProgressIndicator.IncrementAndUpdate();

                COBieSpaceRow space = new COBieSpaceRow(spaces);

                space.Name = ifcSpace.Name;

                space.CreatedBy = GetTelecomEmailAddress(ifcSpace.OwnerHistory);
                space.CreatedOn = GetCreatedOnDateAsFmtString(ifcSpace.OwnerHistory);

                space.Category = GetCategory(ifcSpace);

                space.FloorName = ifcSpace.SpatialStructuralElementParent.Name.ToString();
                space.Description = GetSpaceDescription(ifcSpace);
                space.ExtSystem = GetExternalSystem(ifcSpace);
                space.ExtObject = ifcSpace.GetType().Name;
                space.ExtIdentifier = ifcSpace.GlobalId;
                space.RoomTag = GetRoomTag(ifcSpace, allPropertyValues);
                
                //Do Unit Values
                space.UsableHeight = GetUsableHeight(ifcSpace, allPropertyValues);
                space.GrossArea = GetGrossFloorArea(ifcSpace, allPropertyValues);
                space.NetArea = GetNetArea(ifcSpace, allPropertyValues);

                spaces.Rows.Add(space);

                //----------fill in the attribute information for spaces-----------

                //fill in the attribute information
                attributeBuilder.RowParameters["Name"] = space.Name;
                attributeBuilder.RowParameters["CreatedBy"] = space.CreatedBy;
                attributeBuilder.RowParameters["CreatedOn"] = space.CreatedOn;
                attributeBuilder.RowParameters["ExtSystem"] = space.ExtSystem;
                attributeBuilder.PopulateAttributesRows(ifcSpace); //fill attribute sheet rows//pass data from this sheet info as Dictionary
                
            }
            ProgressIndicator.Finalise();
            return spaces;
        }

        /// <summary>
        /// Get Net Area value
        /// </summary>
        /// <param name="ifcSpace">IfcSpace object</param>
        /// <param name="allPropertyValues">COBieDataPropertySetValues object holds all the properties for all the IfcSpace</param>
        /// <returns>property value as string or default value</returns>
        private string GetNetArea(IfcSpace ifcSpace, COBieDataPropertySetValues allPropertyValues)
        {
            string areaUnit = Context.COBieGlobalValues["AREAUNIT"];//see what the global area unit is
            double areavalue = 0.0;

            IfcAreaMeasure netAreaValue = ifcSpace.GetNetFloorArea();  //this extension has the GSA built in so no need to get again
            if (netAreaValue != null)
            {
                areavalue = ((double)netAreaValue);
                if (areavalue > 0.0)
                {
                    if (areaUnit.ToLower().Contains("milli")) //we are using millimetres
                        areavalue = areavalue / 1000000.0;

                    return areavalue.ToString("F4");
                }
            }

            //Fall back to properties
            //get the property single values for this ifcSpace
            allPropertyValues.SetAllPropertySingleValues(ifcSpace);

            //try and find it in the attached properties of the ifcSpace
            string value = allPropertyValues.GetPropertySingleValueValue("NetFloorArea", true);
            if (value == DEFAULT_STRING)
                value = allPropertyValues.GetPropertySingleValueValue("GSA", true);

            if (value == DEFAULT_STRING)
                return DEFAULT_NUMERIC;
            else
            {
                if (double.TryParse(value, out areavalue))
                {
                    if (areaUnit.ToLower().Contains("milli"))//we are using millimetres
                        areavalue = areavalue / 1000000.0;
                    return areavalue.ToString("F4");
                }
                return value;
            }
        }
        /// <summary>
        /// Get space gross floor area
        /// </summary>
        /// <param name="ifcSpace">IfcSpace object</param>
        /// <param name="allPropertyValues">COBieDataPropertySetValues object holds all the properties for all the IfcSpace</param>
        /// <returns>property value as string or default value</returns>
        private string GetGrossFloorArea(IfcSpace ifcSpace, COBieDataPropertySetValues allPropertyValues)
        {
            string areaUnit = Context.COBieGlobalValues["AREAUNIT"];//see what the global area unit is
            
            //Do Gross Areas 
            IfcAreaMeasure grossAreaValue = ifcSpace.GetGrossFloorArea();
            double areavalue = 0.0;
            if (grossAreaValue != null)
                areavalue = ((double)grossAreaValue);
            else//if we fail on IfcAreaMeasure try GSA keys
            {
                IfcQuantityArea spArea = ifcSpace.GetQuantity<IfcQuantityArea>("GSA Space Areas", "GSA BIM Area");
                if ((spArea is IfcQuantityArea) && (spArea.AreaValue != null))
                    areavalue = ((double)spArea.AreaValue);
            }
            if (areavalue > 0.0)
	        {
                if (areaUnit.ToLower().Contains("milli")) //we are using millimetres
                    areavalue = areavalue / 1000000.0;
                
		         return areavalue.ToString("F4");
	        }
            
            //Fall back to properties
            //get the property single values for this ifcSpace
            allPropertyValues.SetAllPropertySingleValues(ifcSpace);

            //try and find it in the attached properties of the ifcSpace
            string value = allPropertyValues.GetPropertySingleValueValue("GrossFloorArea", true);
            if (value == DEFAULT_STRING)
                value = allPropertyValues.GetPropertySingleValueValue("GSA", true);

            if (value == DEFAULT_STRING)
                return DEFAULT_NUMERIC;
            else
            {
                if (double.TryParse(value, out areavalue))
                {
                    if (areaUnit.ToLower().Contains("milli"))//we are using millimetres
                        areavalue = areavalue / 1000000.0;
                    return areavalue.ToString("F4");
                }
                return value; 
            }
                
            
        }
        /// <summary>
        /// Get space usable height
        /// </summary>
        /// <param name="ifcSpace">IfcSpace object</param>
        /// <param name="allPropertyValues">COBieDataPropertySetValues object holds all the properties for all the IfcSpace</param>
        /// <returns>property value as string or default value</returns>
        private string GetUsableHeight(IfcSpace ifcSpace, COBieDataPropertySetValues allPropertyValues)
        {
            IfcLengthMeasure usableHt = ifcSpace.GetHeight();
            if (usableHt != null)
            return ((double)usableHt).ToString("F3");
            
            //Fall back to properties
            //get the property single values for this ifcSpace
            allPropertyValues.SetAllPropertySingleValues(ifcSpace);

            //try and find it in the attached properties of the ifcSpace
            string value = allPropertyValues.GetPropertySingleValueValue("UsableHeight", true);
            if (value == DEFAULT_STRING)
                value = allPropertyValues.GetPropertySingleValueValue("FinishCeiling", true);
            if (value == DEFAULT_STRING)
                value = allPropertyValues.GetPropertySingleValueValue("FinishCeilingHeight", true);
            if (value == DEFAULT_STRING)
                value = allPropertyValues.GetPropertySingleValueValue("Height", true);
            
            if (value == DEFAULT_STRING)
                return DEFAULT_NUMERIC;
            else
                return value; 
        }
        /// <summary>
        /// Get space description 
        /// </summary>
        /// <param name="ifcSpace">IfcSpace object</param>
        /// <returns>property value as string or default value</returns>
        private string GetSpaceDescription(IfcSpace ifcSpace)
        {
            if (ifcSpace != null)
            {
                if (!string.IsNullOrEmpty(ifcSpace.LongName)) return ifcSpace.LongName;
                else if (!string.IsNullOrEmpty(ifcSpace.Description)) return ifcSpace.Description;
                else if (!string.IsNullOrEmpty(ifcSpace.Name)) return ifcSpace.Name;
            }
            return DEFAULT_STRING;
        }
        /// <summary>
        /// Get space room tag 
        /// </summary>
        /// <param name="ifcSpace">IfcSpace object</param>
        /// <param name="allPropertyValues">COBieDataPropertySetValues object holds all the properties for all the IfcSpace</param>
        /// <returns>property value as string or default value</returns>
        private string GetRoomTag(IfcSpace ifcSpace, COBieDataPropertySetValues allPropertyValues)
        {
            string value = GetSpaceDescription(ifcSpace);
            if (value == DEFAULT_STRING)
            {
                //Fall back to properties
                //get the property single values for this ifcSpace
                allPropertyValues.SetAllPropertySingleValues(ifcSpace);

                //try and find it in the attached properties of the ifcSpace
                value = allPropertyValues.GetPropertySingleValueValue("RoomTag", true);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("Tag", true);
                if (value == DEFAULT_STRING)
                    value = allPropertyValues.GetPropertySingleValueValue("Room_Tag", true);

            }

            return value;
        }
        #endregion

        COBieSheet<COBieAttributeRow> _attributes;

        public void InitialiseAttributes(ref COBieSheet<COBieAttributeRow> attributeSheet)
        {
            _attributes = attributeSheet;
        }
    }
}
