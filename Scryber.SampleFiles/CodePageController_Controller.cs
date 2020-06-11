using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber;
using Scryber.Components;
using Scryber.Drawing;

namespace Scryber.SystemTests.vs13.Samples
{
    /// <summary>
    /// Controller for the CodeController.pdfx template
    /// </summary>
    public class CodePageController_Controller
    {
        /// <summary>
        /// The page this controller is bound to.
        /// </summary>
        [PDFOutlet("InnerControlledPage")]
        public PDFPage Page;

        //
        // outlets
        //

        [PDFOutlet(Required=true)]
        public PDFLabel InnerLoadLabel;

        [PDFOutlet("dynamicContent", Required=true)]
        public PDFPlaceHolder DynamicContent;

        

        public CodePageController_Controller()
        {
        }

        /// <summary>
        /// Document Loaded event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [PDFAction()]
        public void CodeController_Loaded(object sender, PDFLoadEventArgs args)
        {
            //Perform any initialization here.
            args.Context.TraceLog.Add(TraceLevel.Message, "CodeController", "Inside the document loaded event");
        }

        //
        // page events
        //

        [PDFAction()]
        public void InnerControlledPage_Load(object sender, PDFLoadEventArgs args)
        {
            
            args.Context.TraceLog.Add(TraceLevel.Message, "CodePageController", "Executing the Page Loaded event handler code from the controller class");
            this.InnerLoadLabel.Text = "This is assigned from the page load event. And we dynamically build and assign a table grid to a placeholder Outlet (with a matching name to the parent templates placeholder) at the same time.";

            this.PopulatePlaceholder();
        }

        public void PopulatePlaceholder()
        {
            PDFTableGrid grid = new PDFTableGrid() { FullWidth = true, Margins = new PDFThickness(10) };
            //Assigning the grid to the contents of the placeholder.
            this.DynamicContent.Contents.Add(grid);

            PDFTableHeaderRow headr = new PDFTableHeaderRow();
            headr.BorderColor = PDFColors.Aqua;
            headr.BorderWidth = 0.5;
            headr.BorderSides = Sides.Bottom;
            grid.Rows.Add(headr);

            for (int j = 0; j < 4; j++)
            {
                PDFTableHeaderCell cell = new PDFTableHeaderCell();
                cell.Contents.Add(new PDFTextLiteral("Head " + j.ToString()));
                headr.Cells.Add(cell);
            }

            for (int i = 0; i < 20; i++)
            {
                PDFTableRow row = new PDFTableRow();
                row.BorderColor = PDFColors.Fuchsia;
                row.BorderWidth = 0.5;
                row.BorderSides = Sides.Bottom;

                grid.Rows.Add(row);


                for (int j = 0; j < 6; j++)
                {
                    PDFTableCell cell = new PDFTableCell();
                    row.Cells.Add(cell);

                    cell.BorderColor = PDFColors.Aqua;
                    cell.BorderWidth = 0.5;
                    cell.BorderSides = Sides.Bottom;
                    cell.Padding = new PDFThickness(3);

                    PDFTextLiteral literal = new PDFTextLiteral("Cell " + i.ToString() + ":" + j.ToString());
                    cell.Contents.Add(literal);

                }
            }
        }

    }
}
