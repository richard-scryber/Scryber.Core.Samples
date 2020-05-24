﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Core.UnitTests.Styles
{
    
    
    /// <summary>
    ///This is a test class for PDFScryberBadgeStyleTest and is intended
    ///to contain all PDFScryberBadgeStyleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFScryberBadgeStyleTest
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
        ///A test for PDFScryberBadgeStyle Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Badge_ConstructorTest()
        {
            PDFScryberBadgeStyle target = new PDFScryberBadgeStyle();
            Assert.IsNotNull(target);
            Assert.AreEqual(PDFStyleKeys.BadgeItemKey, target.ItemKey);
        }

        /// <summary>
        ///A test for Corner
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Badge_CornerTest()
        {
            PDFScryberBadgeStyle target = new PDFScryberBadgeStyle(); 
            
            //Default 

            Corner expected = Corner.BottomRight;
            Assert.AreEqual(expected, target.Corner);

            //Set value

            expected = Corner.BottomLeft;
            Corner actual;
            target.Corner = expected;
            actual = target.Corner;
            Assert.AreEqual(expected, actual);

            // Change Value

            expected = Corner.TopRight;
            target.Corner = expected;
            actual = target.Corner;
            Assert.AreEqual(expected, actual);

            //Remove value

            expected = Corner.BottomRight;
            target.RemoveCorner();
            actual = target.Corner;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for DisplayOption
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Badge_DisplayOptionTest()
        {
            PDFScryberBadgeStyle target = new PDFScryberBadgeStyle(); 
            
            //Default Value

            BadgeType expected = PDFScryberBadgeStyle.DefaultDisplayOption;
            Assert.AreEqual(expected, target.DisplayOption);

            //Set value

            expected = BadgeType.WhiteOnBlack;
            BadgeType actual;
            target.DisplayOption = expected;
            actual = target.DisplayOption;
            Assert.AreEqual(expected, actual);

            // Change Value

            expected = BadgeType.BlackOnWhite;
            target.DisplayOption = expected;
            actual = target.DisplayOption;
            Assert.AreEqual(expected, actual);

            //Remove value

            expected = PDFScryberBadgeStyle.DefaultDisplayOption;
            target.RemoveDisplayOption();
            actual = target.DisplayOption;
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test for XOffset
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Badge_XOffsetTest()
        {
            PDFScryberBadgeStyle target = new PDFScryberBadgeStyle();

            // Default value

            PDFUnit expected = PDFScryberBadgeStyle.DefaultXOffset;
            PDFUnit actual = target.XOffset;
            Assert.AreEqual(expected, actual);

            // Set Value

            expected = 20;
            target.XOffset = expected;
            actual = target.XOffset;
            Assert.AreEqual(expected, actual);

            // Change Value

            expected = new PDFUnit(100, PageUnits.Points);
            target.XOffset = expected;
            actual = target.XOffset;
            Assert.AreEqual(expected, actual);

            // Remove Value

            expected = PDFScryberBadgeStyle.DefaultXOffset;
            target.RemoveXOffset();
            actual = target.XOffset;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for YOffset
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Badge_YOffsetTest()
        {
            PDFScryberBadgeStyle target = new PDFScryberBadgeStyle();

            // Default value

            PDFUnit expected = PDFScryberBadgeStyle.DefaultYOffset;
            PDFUnit actual = target.YOffset;
            Assert.AreEqual(expected, actual);

            // Set Value

            expected = 20;
            target.YOffset = expected;
            actual = target.YOffset;
            Assert.AreEqual(expected, actual);

            // Change Value

            expected = new PDFUnit(100, PageUnits.Points);
            target.YOffset = expected;
            actual = target.YOffset;
            Assert.AreEqual(expected, actual);

            // Remove Value

            expected = PDFScryberBadgeStyle.DefaultYOffset;
            target.RemoveYOffset();
            actual = target.YOffset;
            Assert.AreEqual(expected, actual);
        }
    }
}
