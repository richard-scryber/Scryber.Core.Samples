using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.SystemTests.vs13.Samples
{
    /// <summary>
    /// Controller for the CodeController.pdfx template
    /// </summary>
    public class CodeController_Controller
    {
        /// <summary>
        /// The document this controller is bound to.
        /// </summary>
        [PDFOutlet("CodeController")]
        public PDFDocument Document;


        // labels for each event - all are required

        [PDFOutlet(Required=true)]
        public PDFLabel InitLabel;

        [PDFOutlet(Required = true)]
        public PDFLabel LoadLabel;

        [PDFOutlet(Required = true)]
        public PDFLabel BindingLabel;

        [PDFOutlet(Required = true)]
        public PDFLabel BoundLabel;

        [PDFOutlet(Required = true)]
        public PDFLabel PreLayoutLabel;

        [PDFOutlet(Required = true)]
        public PDFLabel PostLayoutLabel;

        [PDFOutlet(Required = true)]
        public PDFLabel PreRenderLabel;

        [PDFOutlet(Required = true)]
        public PDFLabel PostRenderLabel;


        // placeholder in the PageGroup content

        [PDFOutlet("dynamicContent", Required=true)]
        public PDFPlaceHolder DynamicContent;


        // set to the page with its own controller

        [PDFOutlet("referencedPage",Required=true)]
        public PDFSection ReferencedPage { get; set; }
        
        //style definition values

        [PDFOutlet]
        public PDFStyleDefn MyStyle { get; set; }
        
        [PDFOutlet]
        public PDFStyleDefn OtherStyle { get; set; }


        //
        //.ctor
        //

        public CodeController_Controller()
        {
        }

        //
        // event handlers
        //

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
        // inner page events
        //

        [PDFAction()]
        public void Page_Initialized(object sender, PDFInitEventArgs args)
        {
            args.Context.TraceLog.Add(TraceLevel.Message, "CodeController", "Executing the Page Initialized event handler code from the controller class");
            this.InitLabel.Text = "This is assigned from the page init event";
        }

        [PDFAction()]
        public void Page_Loaded(object sender, PDFLoadEventArgs args)
        {
            if (null == Document)
                args.Context.TraceLog.Add(TraceLevel.Warning, "CodeController", "This document controller was loaded as a select from an outer document. So our Document property was not set.");

            args.Context.TraceLog.Add(TraceLevel.Message, "CodeController", "Executing the Page Loaded event handler code from the controller class");
            this.LoadLabel.Text = "This is assigned from the page load event";
        }

        [PDFAction()]
        public void Page_Binding(object sender, PDFDataBindEventArgs args)
        {
            args.Context.TraceLog.Add(TraceLevel.Message, "CodeController", "Executing the Page Binding event handler code from the controller class");
            this.BindingLabel.Text = "This is assigned from the page binding event";
        }

        [PDFAction()]
        public void Page_Bound(object sender, PDFDataBindEventArgs args)
        {
            args.Context.TraceLog.Add(TraceLevel.Message, "CodeController", "Executing the Page Bound event handler code from the controller class");
            this.BoundLabel.Text = "This is assigned from the page bound event";
        }

        [PDFAction()]
        public void Page_PreLayout(object sender, PDFLayoutEventArgs args)
        {
            args.Context.TraceLog.Add(TraceLevel.Message, "CodeController", "Executing the Page PreLayout event handler code from the controller class");
            this.PreLayoutLabel.Text = "This is assigned from the page PreLayout event";
        }

        [PDFAction()]
        public void Page_PostLayout(object sender, PDFLayoutEventArgs args)
        {
            args.Context.TraceLog.Add(TraceLevel.Message, "CodeController", "Executing the Page PostLayout event handler code from the controller class");
            this.PostLayoutLabel.Text = "This is assigned from the page PostLayout event";
        }

        [PDFAction()]
        public void Page_PreRender(object sender, PDFRenderEventArgs args)
        {
            args.Context.TraceLog.Add(TraceLevel.Message, "CodeController", "Executing the Page PreRender event handler code from the controller class");
            this.PreRenderLabel.Text = "This is assigned from the page PreRender event";
        }

        [PDFAction()]
        public void Page_PostRender(object sender, PDFRenderEventArgs args)
        {
            args.Context.TraceLog.Add(TraceLevel.Message, "CodeController", "Executing the Page PostRender event handler code from the controller class");
            this.PostRenderLabel.Text = "This is assigned from the page PostRender event";
        }

        //
        // complex content
        //
        
        [PDFAction()]
        public void DynamicImage_Loaded(object sender, PDFLoadEventArgs args)
        {
            PDFImage img = sender as PDFImage;
            if (null != img)
            {
                System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)System.Drawing.Image.FromFile("../../../Resources/scryber_logo_flat.png");
                img.Data = Scryber.Drawing.PDFImageData.LoadImageFromBitmap("DynamicImageFromController", bmp, false);
            }
        }

        [PDFAction()]
        public void EmptyList_Loaded(object sender, PDFLoadEventArgs args)
        {
            PDFList list = sender as PDFList;
            for (int i = 0; i < 5; i++)
            {
                PDFListItem li = new PDFListItem();
                li.Contents.Add(new PDFTextLiteral("Item number " + (1+ i).ToString()));
                list.Items.Add(li);
            }
            
            //cannot modify the parent contents and it is a list that is being enumerated during loading.
            //(list.Parent as PDFPanel).Contents.Add(new PDFLine() { StrokeColor = PDFColors.Aqua, StrokeWidth = 1 });
            //but can reference existing placeholders and add content to them
            PDFPlaceHolder ph;
            if (list.Document.TryFindAComponentByID("ExplicitPlaceholderID", out ph))
            {
                ph.Contents.Add(new PDFLine() { StrokeColor = PDFColors.Aqua, StrokeWidth = 1 });
            }
        }

        [PDFAction()]
        public void PopulatePlaceholder(object sender, PDFLoadEventArgs args)
        {
            PDFSection page = new PDFSection() { OutlineTitle = "Dynamic Pages" };
            this.DynamicContent.Contents.Add(page);



            PDFHead2 sectionHead = new PDFHead2() { StyleClass = "section-heading", Text = "Dynamic Pages" };
            page.Contents.Add(sectionHead);

            PDFParagraph para = new PDFParagraph() { StyleClass = "notes" };

            para.Contents.Add(new PDFTextLiteral("This page has been added dynamically to the document in the code controller for top level. " +
                                                 "Because the controller has a declared Outlet for a placeholder 'dynamicContent', then it is " +
                                                 "automatically assigned to the comonent when parsed. We can then use it in our code."));
            page.Contents.Add(para);

            PDFDiv container = new PDFDiv() { StyleClass = "wide" };
            page.Contents.Add(container);

            PDFHead4 subhead = new PDFHead4() { StyleClass = "group-heading", OutlineTitle = "Dynamic Grid", Text = "Dynamic Grid" };
            container.Contents.Add(subhead);

            PDFTableGrid grid = new PDFTableGrid() { FullWidth = true, Margins = new PDFThickness(10) };
            container.Contents.Add(grid);

            PDFTableHeaderRow headr = new PDFTableHeaderRow();
            headr.BorderColor = PDFColors.Black;
            headr.BorderWidth = 0.5;
            headr.BorderSides = Sides.Bottom;
            grid.Rows.Add(headr);

            for (int j = 0; j < 4; j++)
            {
                PDFTableHeaderCell cell = new PDFTableHeaderCell();
                cell.Contents.Add(new PDFTextLiteral("Head " + j.ToString()));
                headr.Cells.Add(cell);
            }

            for (int i = 0; i < 10; i++)
            {
                PDFTableRow row = new PDFTableRow();
                row.BorderColor = PDFColors.Gray;
                row.BorderWidth = 0.5;
                row.BorderSides = Sides.Bottom;

                grid.Rows.Add(row);


                for (int j = 0; j < 4; j++)
                {
                    PDFTableCell cell = new PDFTableCell();
                    row.Cells.Add(cell);

                    cell.BorderColor = PDFColors.Gray;
                    cell.BorderWidth = 0.5;
                    cell.BorderSides = Sides.Bottom;
                    cell.Padding = new PDFThickness(3);

                    PDFTextLiteral literal = new PDFTextLiteral("Cell " + i.ToString() + ":" + j.ToString());
                    cell.Contents.Add(literal);

                }
            }
        }


        //
        // style definition events
        //

        [PDFAction()]
        public void StyleDataBinding(object sender, PDFDataBindEventArgs args)
        {
            MyStyle.Fill.Color = PDFColors.Red;
        }

        [PDFAction()]
        public void StyleGroupBinding(object sender, PDFDataBindEventArgs args)
        {
            PDFStyleGroup grp = (PDFStyleGroup)sender;

            PDFStyleDefn h2Style = new PDFStyleDefn() { AppliedType = typeof(PDFHead2), AppliedClass="section-heading" };
            h2Style.Border.Width = 3;
            h2Style.Border.Color = Scryber.Drawing.PDFColors.Lime;

            grp.Styles.Add(h2Style);

            PDFStyleDefn h4Style = new PDFStyleDefn() { AppliedType = typeof(PDFHead4), AppliedClass = "group-heading" };
            h4Style.Border.Width = 3;
            h4Style.Border.Color = Scryber.Drawing.PDFColors.Purple;

            grp.Styles.Add(h4Style);
        }

    }
}
