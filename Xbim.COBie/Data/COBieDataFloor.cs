﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.COBie.Rows;
using Xbim.Ifc.ExternalReferenceResource;
using Xbim.Ifc.Kernel;
using Xbim.Ifc.ProductExtension;
using Xbim.Ifc.QuantityResource;
using Xbim.XbimExtensions;
using Xbim.Ifc.Extensions;
using Xbim.Ifc.PropertyResource;

namespace Xbim.COBie.Data
{
    /// <summary>
    /// Class to input data into excel worksheets for the the Floor tab.
    /// </summary>
    public class COBieDataFloor : COBieData<COBieFloorRow>, IAttributeProvider
    {
        
        /// <summary>
        /// Data Floor constructor
        /// </summary>
        /// <param name="model">The context of the model being generated</param>
        public COBieDataFloor(COBieContext context) : base(context)
        {
            
        }

        #region Methods

        /// <summary>
        /// Fill sheet rows for Floor sheet
        /// </summary>
        /// <returns>COBieSheet<COBieFloorRow></returns>
        public override COBieSheet<COBieFloorRow> Fill()
        {
            ProgressIndicator.ReportMessage("Starting Floors...");

            //create new sheet 
            COBieSheet<COBieFloorRow> floors = new COBieSheet<COBieFloorRow>(Constants.WORKSHEET_FLOOR);

            // get all IfcBuildingStory objects from IFC file
            IEnumerable<IfcBuildingStorey> buildingStories = Model.InstancesOfType<IfcBuildingStorey>();

            COBieDataPropertySetValues allPropertyValues = new COBieDataPropertySetValues(buildingStories); //properties helper class
            COBieDataAttributeBuilder attributeBuilder = new COBieDataAttributeBuilder(Context, allPropertyValues);
            attributeBuilder.InitialiseAttributes(ref _attributes);
            
            
            //IfcClassification ifcClassification = Model.InstancesOfType<IfcClassification>().FirstOrDefault();
            //list of attributes to exclude form attribute sheet
            
            //set up filters on COBieDataPropertySetValues for the SetAttributes only
            attributeBuilder.ExcludeAttributePropertyNames.AddRange(Context.Exclude.Floor.AttributesEqualTo);
            attributeBuilder.ExcludeAttributePropertyNamesWildcard.AddRange(Context.Exclude.Floor.AttributesContain);
            attributeBuilder.RowParameters["Sheet"] = "Floor";
            
           

            ProgressIndicator.Initialise("Creating Floors", buildingStories.Count());

            foreach (IfcBuildingStorey ifcBuildingStorey in buildingStories)
            {
                ProgressIndicator.IncrementAndUpdate();

                COBieFloorRow floor = new COBieFloorRow(floors);
                if (string.IsNullOrEmpty(ifcBuildingStorey.Name))
                {
                    ifcBuildingStorey.Name = "Name Unknown " + UnknownCount.ToString();
                    UnknownCount++;
                }

                floor.Name = ifcBuildingStorey.Name.ToString();

                floor.CreatedBy = GetTelecomEmailAddress(ifcBuildingStorey.OwnerHistory);
                floor.CreatedOn = GetCreatedOnDateAsFmtString(ifcBuildingStorey.OwnerHistory);

                floor.Category = GetCategory(ifcBuildingStorey);

                floor.ExtSystem = GetExternalSystem(ifcBuildingStorey);
                floor.ExtObject = ifcBuildingStorey.GetType().Name;
                floor.ExtIdentifier = ifcBuildingStorey.GlobalId;
                floor.Description = GetFloorDescription(ifcBuildingStorey);
                floor.Elevation = (string.IsNullOrEmpty(ifcBuildingStorey.Elevation.ToString())) ? DEFAULT_NUMERIC : string.Format("{0:F4}", (double)ifcBuildingStorey.Elevation);

                floor.Height = GetFloorHeight(ifcBuildingStorey, allPropertyValues);

                floors.AddRow(floor);

                //fill in the attribute information
                attributeBuilder.RowParameters["Name"] = floor.Name;
                attributeBuilder.RowParameters["CreatedBy"] = floor.CreatedBy;
                attributeBuilder.RowParameters["CreatedOn"] = floor.CreatedOn;
                attributeBuilder.RowParameters["ExtSystem"] = floor.ExtSystem;
                attributeBuilder.PopulateAttributesRows(ifcBuildingStorey); //fill attribute sheet rows//pass data from this sheet info as Dictionary
                
            }
            ProgressIndicator.Finalise();

            return floors;
        }
        /// <summary>
        /// Get the floor height
        /// </summary>
        /// <param name="ifcBuildingStorey">IfcBuildingStory object</param>
        /// <param name="allPropertyValues">COBieDataPropertySetValues object holds all the properties for all the IfcBuildingStory </param>
        /// <returns></returns>
        private string GetFloorHeight (IfcBuildingStorey ifcBuildingStorey, COBieDataPropertySetValues allPropertyValues)
        {
            //try for a IfcQuantityLength related property to this building story
            IEnumerable<IfcQuantityLength> qLen = ifcBuildingStorey.IsDefinedByProperties.Select(p => p.RelatedObjects.OfType<IfcQuantityLength>()).FirstOrDefault();
            if (qLen != null && qLen.FirstOrDefault() != null) 
                return qLen.FirstOrDefault().LengthValue.ToString();
            
            //Fall back properties
            //get the property single values for this building story
            allPropertyValues.SetAllPropertySingleValues(ifcBuildingStorey);

            //try and find it in the attached properties of the building story
            string value = allPropertyValues.GetPropertySingleValueValue("StoreyHeight", true);
            if (value == DEFAULT_STRING)
                value = allPropertyValues.GetPropertySingleValueValue("Storey Height", true);
            if (value == DEFAULT_STRING)
                value = allPropertyValues.GetPropertySingleValueValue("FloorHeight", true);
            if (value == DEFAULT_STRING)
                value = allPropertyValues.GetPropertySingleValueValue("Floor Height", true);

            if (value == DEFAULT_STRING)
                return DEFAULT_NUMERIC;
            else
            {
                //check it is a number and then format it
                double dblvalue = 0;
                if (double.TryParse(value, out dblvalue))
                    return string.Format("{0:F4}", dblvalue); 
            }
                
            return DEFAULT_NUMERIC;
	
                

        }

        private string GetFloorDescription(IfcBuildingStorey bs)
        {
            if (bs != null)
            {
                if (!string.IsNullOrEmpty(bs.LongName)) return bs.LongName;
                else if (!string.IsNullOrEmpty(bs.Description)) return bs.Description;
                else if (!string.IsNullOrEmpty(bs.Name)) return bs.Name;
            }
            return DEFAULT_STRING;
        }

        #endregion

        COBieSheet<COBieAttributeRow> _attributes;

        public void InitialiseAttributes(ref COBieSheet<COBieAttributeRow> attributeSheet)
        {
            _attributes = attributeSheet;
        }
    }
}
