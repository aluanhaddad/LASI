﻿using LASI.Content;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using System.Linq;
using LASI.UnitTests;

namespace LASI.Content.Tests
{


    /// <summary>
    ///This is a test class for DocXFileTest and is intended
    ///to contain all DocXFileTest Unit Tests
    ///</summary>
    [TestClass]
    public class DocXFileTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext {
            get {
                return testContextInstance;
            }
            set {
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
        //public void MyTestInitialize() {
        //}
        ////
        ////Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup() 
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for DocXFile Constructor
        ///</summary>
        [TestMethod]
        public void DocXFileConstructorTest() {
            string path = @"..\..\..\TestDocs\Draft_Environmental_Assessment.docx";
            DocXFile target = new DocXFile(path);
            Assert.IsTrue(System.IO.File.Exists(path));
            Assert.AreEqual(System.IO.Path.GetFullPath(path), target.FullPath);
        }
        /// <summary>
        ///A test for DocXFile Constructor
        ///</summary>
        [TestMethod]
        [ExpectedFileTypeWrapperMismatchException]
        public void DocXFileConstructorTest1() {
            string path = @"..\..\..\TestDocs\Draft_Environmental_Assessment.txt";
            DocXFile target = new DocXFile(path);
        }
        /// <summary>
        ///A test for DocXFile Constructor
        ///</summary>
        [TestMethod]
        [ExpectedFileNotFoundException]
        public void DocXFileConstructorTest2() {
            string invalidPath = System.IO.Directory.GetCurrentDirectory();//This is should never be valid.
            Assert.IsFalse(System.IO.File.Exists(invalidPath));
            DocXFile target = new DocXFile(invalidPath);
        }
    }
}