﻿using LASI.FileSystem;
using LASI.FileSystem.FileTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AlgorithmAssemblyUnitTestProject
{


    /// <summary>
    ///This is a test class for FileManagerTest and is intended
    ///to contain all FileManagerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FileManagerTest
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
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext) {
            if (Directory.Exists(@"..\..\..\NewProject\input"))
                Directory.Delete(@"..\..\..\NewProject\input", true);
            if (Directory.Exists(@"..\..\..\backup\NewProject"))
                Directory.Delete(@"..\..\..\backup\NewProject", true);

            FileManager.Initialize(@"..\..\..\NewProject");
            foreach (var fileInfo in new DirectoryInfo(@"..\..\..\UnitTests\MockUserFiles").EnumerateFiles()) {
                switch (fileInfo.Extension) {
                    case ".doc":
                        File.Copy(fileInfo.FullName, @"..\..\..\NewProject\input\doc\" + fileInfo.FullName.Substring(fileInfo.FullName.LastIndexOf('\\') + 1), true);
                        break;
                    case ".docx":
                        File.Copy(fileInfo.FullName, @"..\..\..\NewProject\input\docx\" + fileInfo.FullName.Substring(fileInfo.FullName.LastIndexOf('\\') + 1), true);
                        break;
                    case ".txt":
                        File.Copy(fileInfo.FullName, @"..\..\..\NewProject\input\text\" + fileInfo.FullName.Substring(fileInfo.FullName.LastIndexOf('\\') + 1), true);
                        break;
                    case ".tagged":
                        File.Copy(fileInfo.FullName, @"..\..\..\NewProject\input\tagged\" + fileInfo.FullName.Substring(fileInfo.FullName.LastIndexOf('\\') + 1), true);
                        break;
                    default:
                        break;
                }
            }

        }

        ////  Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup() {
            Directory.Delete(@"..\..\..\NewProject\Input", true);
        }
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup() {
        }
        //
        #endregion


        ///// <summary>
        /////a test for AddDocFile
        /////</summary>
        //[TestMethod()]
        //public void AddDocFileTest() {
        //    string sourcePath = @"..\..\..\TestDocs\Draft_Environmental_Assessment.doc";

        //    FileManager.AddDocFile(sourcePath, true);
        //    Assert.IsTrue(File.Exists(FileManager.DocFilesDir + @"\Draft_Environmental_Assessment.doc"));
        //}

        ///// <summary>
        /////a test for AddDocXFile
        /////</summary>
        //[TestMethod()]
        //public void AddDocXFileTest() {
        //    string sourcePath = @"..\..\..\TestDocs\Draft_Environmental_Assessment.docx";
        //    FileManager.AddDocXFile(sourcePath,true);
        //    Assert.IsTrue(File.Exists(FileManager.DocxFilesDir + @"\Draft_Environmental_Assessment.docx"));
        //}

        ///// <summary>
        /////a test for AddTextFile
        /////</summary>
        //[TestMethod()]
        //public void AddTextFileTest() {
        //    string sourcePath = @"..\..\..\TestDocs\Draft_Environmental_Assessment.txt";

        //    FileManager.AddTextFile(sourcePath,true);
        //    Assert.IsTrue(File.Exists(FileManager.TextFilesDir + @"\Draft_Environmental_Assessment.txt"));
        //}

        /// <summary>
        ///a test for BackupProject
        ///</summary>
        [TestMethod()]
        public void BackupProjectTest() {
            FileManager.BackupProject();
            Assert.IsTrue(Directory.Exists(Directory.GetParent(FileManager.ProjectDir).FullName + @"\backup\" + FileManager.ProjectName));
        }

        /// <summary>
        ///a test for ConvertDocFiles
        ///</summary>
        [TestMethod()]
        public void ConvertDocFilesTest() {
            DocFile[] files = (from F in Directory.EnumerateFiles(FileManager.DocFilesDir)
                               select new DocFile(F)).ToArray();
            FileManager.ConvertDocFiles(files);
            foreach (var F in files)
                Assert.IsTrue(File.Exists(FileManager.DocxFilesDir + "\\" + F.NameSansExt + ".docx"));
        }

        /// <summary>
        ///a test for ConvertDocFilesAsync
        ///</summary>
        [TestMethod()]
        public async Task ConvertDocFilesAsyncTest() {
            DocFile[] files = (from F in Directory.EnumerateFiles(FileManager.DocFilesDir)
                               select new DocFile(F)).ToArray();

            await FileManager.ConvertDocFilesAsync(files);

            foreach (var F in files)
                Assert.IsTrue(File.Exists(FileManager.DocxFilesDir + "\\" + F.NameSansExt + ".docx"));
        }
        /// <summary>
        ///a test for ConvertDocxToText
        ///</summary>
        [TestMethod()]
        public void ConvertDocxToTextTest() {
            DocXFile[] files = (from F in Directory.EnumerateFiles(FileManager.DocxFilesDir)
                                select new DocXFile(F)).ToArray();
            FileManager.ConvertDocxToText(files);
            foreach (var F in files)
                Assert.IsTrue(File.Exists(FileManager.TextFilesDir + "\\" + F.NameSansExt + ".txt"));

        }


        /// <summary>
        ///a test for ConvertDocxToTextAsync
        ///</summary>
        [TestMethod()]
        public async Task ConvertDocxToTextAsyncTest() {
            DocXFile[] files = (from F in Directory.EnumerateFiles(FileManager.DocxFilesDir)
                                select new DocXFile(F)).ToArray();

            await FileManager.ConvertDocxToTextAsync(files);

            foreach (var F in files)
                Assert.IsTrue(File.Exists(FileManager.TaggedFilesDir + "\\" + F.NameSansExt + ".tagged"));

        }

        /// <summary>
        ///a test for Initialize
        ///</summary>
        [TestMethod()]
        public void InitializeTest() {
            string projectDir = @"..\..\..\NewProject";
            FileManager.Initialize(projectDir);
            Assert.IsTrue(Directory.Exists(projectDir));
        }

        /// <summary>
        ///a test for TagTextFile
        ///</summary>
        [TestMethod()]
        public void TagTextFilesTest() {
            TextFile[] files = (from F in Directory.EnumerateFiles(FileManager.TextFilesDir)
                                select new TextFile(F)).ToArray();
            FileManager.TagTextFiles(files);
            foreach (var F in files)
                Assert.IsTrue(File.Exists(FileManager.TaggedFilesDir + "\\" + F.NameSansExt + ".tagged"));
        }

        /// <summary>
        ///a test for TagTextFilesAsync
        ///</summary>
        [TestMethod()]
        public async Task TagTextFilesAsyncTest() {
            var files = (from F in Directory.EnumerateFiles(FileManager.TextFilesDir)
                         select new TextFile(F)).ToArray();
            await FileManager.TagTextFilesAsync(files);
            foreach (var F in files) {
                Assert.IsTrue(File.Exists(FileManager.TaggedFilesDir + "\\" + F.NameSansExt + ".tagged"));
            }
        }
        /// <summary>
        ///a test for ProjectName
        ///</summary>
        [TestMethod()]
        public void ProjectNameTest() {
            FileManager.Initialize(@"..\..\..\NewProject");
            string expected = "NewProject";
            string actual;
            actual = FileManager.ProjectName;
            Assert.AreEqual(expected, actual);
        }



    }
}
