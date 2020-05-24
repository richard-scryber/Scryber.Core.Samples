﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Layout;

namespace Scryber.Core.UnitTests.Layout
{
    [TestClass()]
    public class OverflowTests
    {
        const string TestCategoryName = "Layout";

        PDFLayoutDocument layout;

        /// <summary>
        /// Event handler that sets the layout instance variable, after the layout has completed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Doc_LayoutComplete(object sender, PDFLayoutEventArgs args)
        {
            layout = args.Context.DocumentLayout;
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionOverflow()
        {
            const int PageWidth = 200;
            const int PageHeight = 300;

            PDFDocument doc = new PDFDocument();
            PDFSection section = new PDFSection();
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            PDFDiv top = new PDFDiv() { Height = PageHeight - 100 };
            section.Contents.Add(top);

            //div is too big for the remaining space on the page
            PDFDiv tooverflow = new PDFDiv() { Height = 150 };
            section.Contents.Add(tooverflow);

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.ProcessDocument(ms);
            }

            Assert.AreEqual(2,layout.AllPages.Count);


            //Check that the first page has the same dimensions.
            PDFLayoutPage firstpage = layout.AllPages[0];

            Assert.AreEqual(PageWidth, firstpage.Width);
            Assert.AreEqual(PageHeight, firstpage.Height);
            

            //Check that the overflowed page has the same dimensions.
            PDFLayoutPage lastpage = layout.AllPages[1];

            Assert.AreEqual(PageWidth, lastpage.Width);
            Assert.AreEqual(PageHeight, lastpage.Height);
            
            //Check that the block has overflowed to 0 y offset
            PDFLayoutBlock overflowedblock = lastpage.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, overflowedblock.TotalBounds.Y);
            Assert.AreEqual(150, overflowedblock.Height);
        }

        

        
    }
}
