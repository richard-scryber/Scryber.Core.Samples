﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Core.UnitTests.Styles
{
    
    
    /// <summary>
    ///This is a test class for PDFOverlayGridStyleTest and is intended
    ///to contain all PDFOverlayGridStyleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFOverlayGridStyleTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for PDFOverlayGridStyle Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void OverlayGrid_ConstructorTest()
        {
            PDFOverlayGridStyle target = new PDFOverlayGridStyle();
            Assert.IsNotNull(target);
            Assert.AreEqual(PDFStyleKeys.OverlayItemKey, target.ItemKey);
        }

        

        /// <summary>
        ///A test for GetPen
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void OverlayGrid_GetPenTest()
        {
            PDFOverlayGridStyle target = new PDFOverlayGridStyle();
            PDFSolidPen expected = null;

            PDFSolidPen actual;
            actual = (PDFSolidPen)target.GetPen();
            Assert.IsNull(actual);

            target.ShowGrid = true;
            expected = new PDFSolidPen(PDFOverlayGridStyle.DefaultGridColor, PDFOverlayGridStyle.DefaultGridPenWidth);
            expected.Opacity = PDFOverlayGridStyle.DefaultGridOpacity;
            actual = (PDFSolidPen)target.GetPen();
            Assert.IsNotNull(actual);

            Assert.AreEqual(expected.Color, actual.Color);
            Assert.AreEqual(expected.Width, actual.Width);
            Assert.AreEqual(expected.Opacity, actual.Opacity);

            target.GridColor = PDFColors.Purple;
            target.GridOpacity = 0.3;

            expected = new PDFSolidPen(PDFColors.Purple, PDFOverlayGridStyle.DefaultGridPenWidth);
            expected.Opacity = 0.3;

            actual = (PDFSolidPen)target.GetPen();
            Assert.AreEqual(expected.Color, actual.Color);
            Assert.AreEqual(expected.Width, actual.Width);
            Assert.AreEqual(expected.Opacity, actual.Opacity);

        }

       

        /// <summary>
        ///A test for GridColor
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void OverlayGrid_GridColorTest()
        {
            PDFOverlayGridStyle target = new PDFOverlayGridStyle();
            PDFColor expected = PDFOverlayGridStyle.DefaultGridColor;
            Assert.AreEqual(expected, target.GridColor);

            expected = PDFColors.Teal;
            target.GridColor = expected;
            Assert.AreEqual(expected, target.GridColor);

            expected = PDFColors.Silver;
            target.GridColor = expected;
            Assert.AreEqual(expected, target.GridColor);

            target.RemoveGridColor();
            expected = PDFOverlayGridStyle.DefaultGridColor;
            Assert.AreEqual(expected, target.GridColor);

            target.GridColor = expected; //Default
            Assert.AreEqual(expected, target.GridColor);
        }

        /// <summary>
        ///A test for GridOpacity
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void OverlayGrid_GridOpacityTest()
        {
            PDFOverlayGridStyle target = new PDFOverlayGridStyle();
            double expected = PDFOverlayGridStyle.DefaultGridOpacity;
            Assert.AreEqual(expected, target.GridOpacity);

            expected = 1.0;
            target.GridOpacity = expected;
            Assert.AreEqual(expected, target.GridOpacity);

            expected = 0.4;
            target.GridOpacity = expected;
            Assert.AreEqual(expected, target.GridOpacity);

            target.RemoveGridOpacity();
            expected = PDFOverlayGridStyle.DefaultGridOpacity;
            Assert.AreEqual(expected, target.GridOpacity);

            target.GridOpacity = expected; //Default
            Assert.AreEqual(expected, target.GridOpacity);
        }

        /// <summary>
        ///A test for GridYOffset
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void OverlayGrid_GridYOffsetTest()
        {
            PDFOverlayGridStyle target = new PDFOverlayGridStyle();
            PDFUnit expected = PDFOverlayGridStyle.DefaultYOffset;
            Assert.AreEqual(expected, target.GridYOffset);

            expected = 20;
            target.GridYOffset = expected;
            Assert.AreEqual(expected, target.GridYOffset);

            expected = 100;
            target.GridYOffset = expected;
            Assert.AreEqual(expected, target.GridYOffset);

            target.RemoveGridYOffset();
            expected = PDFOverlayGridStyle.DefaultYOffset;
            Assert.AreEqual(expected, target.GridYOffset);

            target.GridYOffset = expected; //Default
            Assert.AreEqual(expected, target.GridYOffset);
        }

        /// <summary>
        ///A test for GridXOffset
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void OverlayGrid_GridXOffsetTest()
        {
            PDFOverlayGridStyle target = new PDFOverlayGridStyle();
            PDFUnit expected = PDFOverlayGridStyle.DefaultXOffset;
            Assert.AreEqual(expected, target.GridXOffset);

            expected = 20;
            target.GridXOffset = expected;
            Assert.AreEqual(expected, target.GridXOffset);

            expected = 100;
            target.GridXOffset = expected;
            Assert.AreEqual(expected, target.GridXOffset);

            target.RemoveGridXOffset();
            expected = PDFOverlayGridStyle.DefaultXOffset;
            Assert.AreEqual(expected, target.GridXOffset);

            target.GridXOffset = expected; //Default
            Assert.AreEqual(expected, target.GridXOffset);
        }

        /// <summary>
        ///A test for GridSpacing
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void OverlayGrid_GridSpacingTest()
        {
            PDFOverlayGridStyle target = new PDFOverlayGridStyle();
            PDFUnit expected = PDFOverlayGridStyle.DefaultGridSpacing;
            Assert.AreEqual(expected, target.GridSpacing);

            expected = 20;
            target.GridSpacing = expected;
            Assert.AreEqual(expected, target.GridSpacing);

            expected = 100;
            target.GridSpacing = expected;
            Assert.AreEqual(expected, target.GridSpacing);

            target.RemoveGridSpacing();
            expected = PDFOverlayGridStyle.DefaultGridSpacing;
            Assert.AreEqual(expected, target.GridSpacing);

            target.GridSpacing = expected; //Default
            Assert.AreEqual(expected, target.GridSpacing);
        }

        /// <summary>
        ///A test for HighlightColumns
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void OverlayGrid_HighlightColumnsTest()
        {
            PDFOverlayGridStyle target = new PDFOverlayGridStyle();
            bool expected = false;
            Assert.AreEqual(expected, target.HighlightColumns);

            expected = true;
            target.HighlightColumns = expected;
            Assert.AreEqual(expected, target.HighlightColumns);

            expected = false;
            target.HighlightColumns = expected;
            Assert.AreEqual(expected, target.HighlightColumns);

            target.RemoveHighlightColumns();
            expected = false;
            Assert.AreEqual(expected, target.HighlightColumns);
        }

        /// <summary>
        ///A test for ShowGrid
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void OverlayGrid_ShowGridTest()
        {
            PDFOverlayGridStyle target = new PDFOverlayGridStyle();
            bool expected = false;
            Assert.AreEqual(expected, target.ShowGrid);

            expected = true;
            target.ShowGrid = expected;
            Assert.AreEqual(expected, target.ShowGrid);

            expected = false;
            target.ShowGrid = expected;
            Assert.AreEqual(expected, target.ShowGrid);

            target.RemoveGrid();
            expected = false;
            Assert.AreEqual(expected, target.ShowGrid);
        }
    }
}
