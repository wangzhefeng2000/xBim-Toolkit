﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Xbim.COBie.Rows;
using Xbim.XbimExtensions;
using System.Linq;
using System.Reflection;



namespace Xbim.COBie
{ 
	/// <summary>
	/// Interrogates IFC models and builds COBie-format objects from the models
	/// </summary>
    public class COBieBuilder
    {

		private COBieBuilder()
		{
			ResetWorksheets();
		}

		/// <summary>
		/// Constructor which also sets the Context
		/// </summary>
		/// <param name="context"></param>
		public COBieBuilder(COBieContext context) : this()
		{
            Context = context;
            GenerateCOBieData();
		}

        /// <summary>
        /// The context in which this COBie data is being built
        /// </summary>
        /// <remarks>Contains the source models, templates, environmental data and other parameters</remarks>
        public COBieContext Context { get; private set; }

        /// <summary>
        /// The set of COBie worksheets
        /// </summary>
        public COBieWorkbook Workbook { get; private set; }

		private void ResetWorksheets()
		{
            Workbook = new COBieWorkbook();
		}

        
		private void Initialise()
        {
			if (Context == null) { throw new InvalidOperationException("COBieReader can't initialise without a valid Context."); }
			if (Context.Models == null || Context.Models.Count == 0) { throw new ArgumentException("COBieReader context must contain one or more models."); }


            // set all the properties
            COBieQueries cq = new COBieQueries(Context);

            
            //create pick list from the template sheet
            COBiePickListReader pickListReader = new COBiePickListReader(Context.COBieGlobalValues["TEMPLATEFILENAME"]);
            COBieSheet<COBiePickListsRow>  CobiePickLists = pickListReader.Read();

            //fall back to xml file if not in template
            if (CobiePickLists.RowCount == 0)
                CobiePickLists = cq.GetCOBiePickListsSheet("PickLists.xml");// create pick lists from xml
           
            // add to workbook and use workbook for error checking later

            //contact sheet first as it will fill contact information lookups for other sheets
            Workbook.Add(cq.GetCOBieContactSheet());
            Workbook.Add(cq.GetCOBieFacilitySheet()); //moved so it is called earlier as it now sets some global unit values used by other sheets
            Workbook.Add(cq.GetCOBieCoordinateSheet());
            Workbook.Add(cq.GetCOBieZoneSheet()); //we need zone before spaces as it sets a flag on departments property
            Workbook.Add(cq.GetCOBieSpaceSheet());
            Workbook.Add(cq.GetCOBieComponentSheet());
            Workbook.Add(cq.GetCOBieAssemblySheet());
            Workbook.Add(cq.GetCOBieConnectionSheet());
            Workbook.Add(cq.GetCOBieDocumentSheet());
             Workbook.Add(cq.GetCOBieFloorSheet());
            Workbook.Add(cq.GetCOBieImpactSheet());
            Workbook.Add(cq.GetCOBieIssueSheet());
            Workbook.Add(cq.GetCOBieJobSheet());            
            Workbook.Add(cq.GetCOBieResourceSheet());
            Workbook.Add(cq.GetCOBieSpareSheet());
            Workbook.Add(cq.GetCOBieSystemSheet());
            Workbook.Add(cq.GetCOBieTypeSheet());
            //we need to fill attributes last as it is populated by Components, Types etc
            Workbook.Add(cq.GetCOBieAttributeSheet());

            Workbook.Add(CobiePickLists); //Workbook.Add(new COBieSheet<COBiePickListsRow>(Constants.WORKSHEET_PICKLISTS));
           

        }

        private void PopulateErrors()
        {
            try
            {                  
                
                COBieProgress progress = new COBieProgress(Context);
                progress.Initialise("Validating Workbooks", Workbook.Count, 0);

                Workbook.CreateIndices();
               
                for (int i = 0; i < Workbook.Count; i++)
                {

                    progress.IncrementAndUpdate();

                    var sheet = Workbook[i];
                    if (sheet.SheetName != Constants.WORKSHEET_PICKLISTS) //skip validation on picklist
                    {
                        sheet.Validate(Workbook);
                    }
                    
                    
                    
                }

                //ValidateForeignKeys(progress);
                progress.Finalise();
            }
            catch (Exception)
            {
                // TODO: Handle
                throw;
            }
        }

        private int GetCOBieSheetIndexBySheetName(string sheetName)
        {
            for (int i = 0; i < Workbook.Count; i++)
            {
                if (sheetName == Workbook[i].SheetName)
                    return i;
            }
            return -1;
        }

        
        private void GenerateCOBieData()
        {
            Initialise();
           
            PopulateErrors();			
        }

		/// <summary>
		/// Passes this instance of the COBieReader into the provided ICOBieFormatter
		/// </summary>
		/// <param name="formatter">The object implementing the ICOBieFormatter interface.</param>
		public void Export(ICOBieFormatter formatter)
		{
			if (formatter == null) { throw new ArgumentNullException("formatter", "Parameter passed to COBieReader.Export(ICOBieFormatter) must not be null."); }
            
            //remove the pick list sheet
            ICOBieSheet<COBieRow> PickList = Workbook.Where(wb => wb.SheetName == "PickLists").FirstOrDefault();
            if (PickList != null)
                Workbook.Remove(PickList);

            // Passes this 
			formatter.Format(this);
		}


        
    }
}