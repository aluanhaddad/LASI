﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LASI.FileSystem.FileTypes;
namespace LASI.FileSystem
{
    /// <summary>
    /// a static class which encapsulates the operations necessary to manage the working directory of the current user progress.
    /// Client code must call the Initialialize method prior to using any of the rhs methods in this class. 
    /// </summary>
    public static class FileManager
    {
        #region Methods

        /// <summary>
        /// Initializes the FileManager, setting its project directory to the given value.
        /// Automatically loads existing files and sets up input paths
        /// </summary>
        /// <param name="projectDir">The realRoot directory of the current project</param>
        public static void Initialize(string projectDir) {
            ProjectName = projectDir.Substring(projectDir.LastIndexOf('\\') + 1);
            ProjectDir = projectDir;
            InitializeDirProperties();
            CheckProjectDirs();
            Initialized = true;
        }

        private static void InitializeDirProperties() {
            InputFilesDir = ProjectDir + @"\input";
            DocFilesDir = InputFilesDir + @"\doc";
            DocxFilesDir = InputFilesDir + @"\docx";
            TextFilesDir = InputFilesDir + @"\text";
            TaggedFilesDir = InputFilesDir + @"\tagged";
            AnalysisDir = ProjectDir + @"\analysis";
            ResultsDir = ProjectDir + @"\results";
        }

        /// <summary>
        /// Checks the existing contents of the current project directory and automatically loads the files it finds. Called by initialize
        /// </summary>
        private static void CheckProjectDirs() {
            CheckProjectDirExistence();
            CheckForInputDirectories();
        }

        /// <summary>
        /// Checks for the existence of the extension statiffied input file project subject-directories and creates them if they do not exist.
        /// </summary>
        private static void CheckForInputDirectories() {
            foreach (var docPath in Directory.EnumerateFiles(DocFilesDir, "*.doc"))
                docFiles.Add(new DocFile(docPath));
            foreach (var docPath in Directory.EnumerateFiles(DocxFilesDir, "*.docx"))
                docXFiles.Add(new DocXFile(docPath));
            foreach (var docPath in Directory.EnumerateFiles(TextFilesDir, "*.txt"))
                textFiles.Add(new TextFile(docPath));
            foreach (var docPath in Directory.EnumerateFiles(TaggedFilesDir, "*.tagged"))
                taggedFiles.Add(new TaggedFile(docPath));
        }
        /// <summary>
        /// Checks for the existence of the project subject-directories and creates them if they do not exist.
        /// </summary>
        private static void CheckProjectDirExistence() {
            //if (Directory.Exists(ProjectDir)) {
            //    BackupProject();
            //}
            foreach (var path in new[] { 
                ProjectDir,
                InputFilesDir,
                AnalysisDir, 
                ResultsDir, 
                DocFilesDir, 
                DocxFilesDir, 
                TaggedFilesDir, 
                TextFilesDir, 
            }) {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
        }
        /// <summary>
        /// Returns a value indicating whether a document with the same name as 
        /// the that indicated by the given newPath is already part of the project. 
        /// </summary>
        /// <param name="filePath">A partial or full, extensionless or extensionful, file newPath containing the name of the file to check.</param>
        /// <returns>Talse if a file with the same name, irrespective of its extension, is part of the project. False otherwise.</returns>
        public static bool FileInProject(string filePath) {
            var fileName = new string(
                filePath.Reverse().
                SkipWhile(c => c != '.').
                Skip(1).TakeWhile(c => c != '\\').
                Reverse().ToArray());
            return !FileInProjectSet(fileName);
        }
        /// <summary>
        /// Returns a value indicating whether a file with the same name as that of the given InputFile, irrespective of its extension, is part of the project. 
        /// </summary>
        /// <param name="filePath">An an Instance of the InputFile class or one of its descendents.</param>
        /// <returns>Talse if a file with the same name, irrespective of it's extension, is part of the project. False otherwise.</returns>
        public static bool FileInProject(InputFile inputFile) {
            return !FileInProjectSet(inputFile.NameSansExt);
        }

        private static bool FileInProjectSet(string fileName) {
            return localDocumentNames.Contains(fileName);
        }



        /// <summary>
        /// Performs the necessary conversions, based on the format of all files within the project.
        /// </summary>
        public static void ConvertAsNeeded() {
            ConvertDocFiles();
            ConvertDocxToText();
            TagTextFiles();
        }

        /// <summary>
        /// Asynchronously performs the necessary conversions, based on the format of all files within the project.
        /// </summary>
        public static async Task ConvertAsNeededAsync() {
            //await ConvertDocFilesAsync();
            await ConvertDocxToTextAsync();
        }



        #region List Insertion Overloads

        static void AddToTypedList(DocXFile file) {
            docXFiles.Add(file);

        }
        static void AddToTypedList(DocFile file) {
            docFiles.Add(file);


        }
        static void AddToTypedList(TextFile file) {
            textFiles.Add(file);

        }
        static void AddToTypedList(TaggedFile file) {
            taggedFiles.Add(file);

        }
        #endregion

        /// <summary>
        /// Removes all files, regardless of extension, whose names do not match any of the names in the provided collection of file path strings.
        /// </summary>
        /// <param name="filesToKeep">collction of file path strings indicating which files are not to be culled. All others will summarilly executed.</param>
        public static void RemoveAllNotIn(IEnumerable<string> filesToKeep) {
            RemoveAllNotIn(from f in filesToKeep
                           select f.IndexOf('.') > 0 ? WrapperMap[f.Substring(f.LastIndexOf('.'))](f) : new TextFile(f));
        }
        /// <summary>
        /// Removes all files, regardless of extension, whose names do not match any of the names in the provided collection of InputFile objects.
        /// </summary>
        /// <param name="filesToKeep">collection of InputFile objects indicating which files are not to be culled. All others will summarilly executed.</param>
        public static void RemoveAllNotIn(IEnumerable<InputFile> filesToKeep) {
            var toRemove = from f in localDocumentNames
                           where (from k in filesToKeep
                                  where f == k.NameSansExt
                                  select k).Count() > 0
                           select f;
            foreach (var f in toRemove) {
                RemoveAllAlikeFiles(f);
            }
        }

        private static void RemoveAllAlikeFiles(string fileName) {
            textFiles.RemoveAll(f => f.NameSansExt == fileName);
            docFiles.RemoveAll(f => f.NameSansExt == fileName);
            docXFiles.RemoveAll(f => f.NameSansExt == fileName);
            taggedFiles.RemoveAll(f => f.NameSansExt == fileName);
        }
        /// <summary>
        /// Adds the document indicated by the specified path string to the project
        /// </summary>
        /// <param name="path">The path string of the document file to add to the project</param>
        /// <param name="overwrite">True to overwrite existing documents within the project with the same name, False otherwise. Defaults to False</param>
        /// <returns>An InputFile object which acts as a wrapper around the project relative path of the newly added file.</returns>
        public static InputFile AddFile(string path, bool overwrite = false) {
            var ext = path.Substring(path.LastIndexOf('.')).ToLower();
            try {
                var originalFile = FileManager.WrapperMap[ext](path);
                var newPath =
                    ext == ".docx" ? DocxFilesDir :
                    ext == ".doc" ? DocFilesDir :
                    ext == ".txt" ? TextFilesDir :
                    ext == ".tagged" ? TaggedFilesDir : "";

                newPath += "\\" + originalFile.Name;

                File.Copy(originalFile.FullPath, newPath, overwrite);
                var newFile = WrapperMap[ext](newPath);
                localDocumentNames.Add(newFile.NameSansExt);
                AddToTypedList(newFile as dynamic);
                return originalFile;
            } catch (KeyNotFoundException ex) {
                throw new UnsupportedFileTypeAddedException(ext, ex);
            }
        }
        /// <summary>
        /// Removes the document represented by an absolute file path string from the project
        /// </summary>
        /// <param name="fullFilePath">The document to remove</param>
        //public static void RemoveFile(string fullFilePath) {
        //    RemoveFile(WrapperMap[fullFilePath.Substring(fullFilePath.LastIndexOf('.'))](fullFilePath));
        //}
        /// <summary>
        /// Removes the document represented by InputFile object from the project
        /// </summary>
        /// <param name="file">The document to remove</param>
        public static void RemoveFile(InputFile file) {
            RemoveAllAlikeFiles(file.NameSansExt);

            RemoveFile(file as dynamic);
        }
        public static void RemoveFile(string file) {
            RemoveAllAlikeFiles(file);
        }

        static void RemoveFile(TextFile file) {
            textFiles.Remove(file);
        }
        static void RemoveFile(DocFile file) {
            docFiles.Remove(file);
        }
        static void RemoveFile(DocXFile file) {
            docXFiles.Remove(file);
        }
        /// <summary>
        /// Converts all of the .doc files it recieves into .docx files
        /// If no arguments are supplied, it will instead convert all yet unconverted .doc files in the project directory
        /// Results are stored in corresponding project directory
        /// </summary>
        /// <param name="files">0 or more instances of the DocFile class which encapsulate .doc files</param>
        public static void ConvertDocFiles(params DocFile[] files) {
            if (files.Length == 0)
                files = docFiles.ToArray();
            foreach (var doc in from d in files
                                where
                                (from dx in docXFiles
                                 where dx.NameSansExt == d.NameSansExt
                                 select dx).Count() == 0
                                select d) {
                var converted = new DocToDocXConverter(doc).ConvertFile();
                AddFile(converted.FullPath);
                File.Delete(converted.FullPath);
            }
        }
        /// <summary>
        /// Asynchronously converts all of the .doc files it recieves into .docx files
        /// If no arguments are supplied, it will instead convert all yet unconverted .doc files in the project directory
        /// Results are stored in corresponding project directory
        /// </summary>
        /// <param name="files">0 or more instances of the DocFile class which encapsulate .doc files</param>
        public static async Task ConvertDocFilesAsync(params DocFile[] files) {
            if (files.Length == 0)
                files = docFiles.ToArray();
            foreach (var doc in from d in files
                                where (from dx in docXFiles
                                       where dx.NameSansExt == d.NameSansExt
                                       select dx).Count() == 0
                                select d) {
                var converted = await new DocToDocXConverter(doc).ConvertFileAsync();
                AddFile(converted.FullPath);
                File.Delete(converted.FullPath);
            }

        }

        /// <summary>
        /// Converts all of the .docx files it recieves into text files
        /// If no arguments are supplied, it will instead convert all yet unconverted .docx files in the project directory
        /// Results are stored in corresponding project directory
        /// </summary>
        /// <param name="files">0 or more instances of the DocXFile class which encapsulate .docx files</param>
        public static void ConvertDocxToText(params DocXFile[] files) {
            if (files.Length == 0)
                files = docXFiles.ToArray();
            foreach (var doc in from d in files
                                where
                                (from dx in textFiles
                                 where dx.NameSansExt == d.NameSansExt
                                 select dx).Count() == 0
                                select d) {
                var converted = new DocxToTextConverter(doc).ConvertFile();
                AddFile(converted.FullPath);
                File.Delete(converted.FullPath);
            }
        }
        /// <summary>
        /// Asynchronously converts all of the .docx files it recieves into text files
        /// If no arguments are supplied, it will instead convert all yet unconverted .docx files in the project directory
        /// Results are stored in corresponding project directory
        /// </summary>
        /// <param name="files">0 or more instances of the DocXFile class which encapsulate .docx files</param>
        public static async Task ConvertDocxToTextAsync(params DocXFile[] files) {
            if (files.Length == 0)
                files = docXFiles.ToArray();
            foreach (var doc in from d in files
                                where
                                (from dx in textFiles
                                 where dx.NameSansExt == d.NameSansExt
                                 select dx).Count() == 0
                                select d) {
                await TextConvertAsync(doc);
            }
        }

        private static async Task TextConvertAsync(DocXFile doc) {
            var converted = await new DocxToTextConverter(doc).ConvertFileAsync();

            AddFile(converted.FullPath);
            File.Delete(converted.FullPath);
        }
        /// <summary>
        /// Invokes the POS tagger on the text files it recieves into storing the newly tagged files
        /// If no arguments are supplied, it will instead convert all yet untagged text files in the project directory
        /// Results are stored in corresponding project directory
        /// </summary>
        /// <param name="files">0 or more instances of the TextFile class which encapsulate text files</param>
        public static void TagTextFiles(params TextFile[] files) {
            if (files.Length == 0)
                files = textFiles.ToArray();
            foreach (var doc in from d in files
                                where (from dx in taggedFiles
                                       where dx.NameSansExt == d.NameSansExt
                                       select dx).Count() == 0
                                select d) {
                var tagger = new SharpNLPTaggingModule.SharpNLPTagger(
                    TaggingOption.TagAndAggregate, doc.FullPath,
                    TaggedFilesDir + "\\" + doc.NameSansExt + ".tagged");
                var tf = tagger.ProcessFile();
                AddFile(tf.FullPath);
            }
        }
        /// <summary>
        ///Asynchronously Invokes the POS tagger on the text files it recieves into storing the newly tagged files
        /// If no arguments are supplied, it will instead convert all yet untagged text files in the project directory
        /// Results are stored in corresponding project directory
        /// </summary>
        /// <param name="files">0 or more instances of the TextFile class which encapsulate text files</param>
        public static async Task TagTextFilesAsync(params TextFile[] files) {
            if (files.Length == 0)
                files = textFiles.ToArray();
            foreach (var doc in from d in files
                                where
                                (from dx in taggedFiles
                                 where dx.NameSansExt == d.NameSansExt
                                 select dx).Count() == 0
                                select d) {
                var tagger = new SharpNLPTaggingModule.SharpNLPTagger(TaggingOption.TagAndAggregate, doc.FullPath, TaggedFilesDir + "\\" + doc.NameSansExt + ".tagged");

                await Task.Run(() => tagger.ProcessFile());

                taggedFiles.Add(new TaggedFile(tagger.OutputFilePath));
            }
        }

        /// <summary>
        /// Copies the entire contents of the current project directory to a predetermined, relative newPath
        /// </summary>
        public static void BackupProject() {
            var projd = new DirectoryInfo(ProjectDir);
            var pard = new DirectoryInfo(projd.Parent.FullName);
            var desitination = Directory.CreateDirectory(pard.FullName + "\\backup\\" + ProjectName);
            foreach (var file in new DirectoryInfo(ProjectDir).GetFiles("*", SearchOption.AllDirectories)) {
                if (!Directory.Exists(file.Directory.Name))
                    desitination.CreateSubdirectory(file.Directory.Parent.Name + "\\" + file.Directory.Name);
                file.CopyTo(desitination.FullName + "\\" + file.Directory.Parent.Name + "\\" + file.Directory.Name + "\\" + file.Name, true);
            }
        }
        public static void DecimateProject() {
            Directory.Delete(ProjectDir, true);
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets the Absolute Path of Current Project Folder of the current project directory
        /// </summary>
        public static string ProjectDir {
            get;
            private set;
        }
        /// <summary>
        /// Gets the name of the current project.
        /// This will be the project name displayed to the user and it corresponds to the project'd top level directory
        /// </summary>
        public static string ProjectName {
            get;
            private set;
        }
        /// <summary>
        /// Gets the realRoot of the input file directory
        /// </summary>
        public static string InputFilesDir {
            get;
            private set;
        }
        /// <summary>
        /// Gets the newPath of the analysis directory which stores temporary files during analysis
        /// </summary>
        public static string AnalysisDir {
            get;
            private set;
        }

        /// <summary>
        /// Gets the result files directory
        /// </summary>
        public static string ResultsDir {
            get;
            private set;
        }

        /// <summary>
        /// Gets the .tagged files directory
        /// </summary>
        public static string TaggedFilesDir {
            get;
            private set;
        }
        /// <summary>
        /// Gets the .doc files directory
        /// </summary>
        public static string DocFilesDir {
            get;
            private set;
        }
        /// <summary>
        /// Gets the .docx files directory
        /// </summary>
        public static string DocxFilesDir {
            get;
            private set;
        }

        /// <summary>
        /// Gets the .txt files directory
        /// </summary>
        public static string TextFilesDir {
            get;
            private set;
        }


        /// <summary>
        /// Gets a list of TextFile instances which represent all *.txt files which are included in the project. 
        /// TextFile instances are wrapper objects which provide discrete accessors to relevant *.txt file properties.
        /// </summary>
        public static IReadOnlyList<TextFile> TextFiles {
            get {
                return FileManager.textFiles;
            }
        }
        /// <summary>
        /// Gets a list of DocXFile instances which represent all *.docx files which are included in the project. 
        /// DocXFile instances are wrapper objects which provide discrete accessors to relevant *.docx file properties.
        /// </summary>
        public static IReadOnlyList<DocXFile> DocXFiles {
            get {
                return FileManager.docXFiles;
            }
        }
        /// <summary>
        /// Gets a list of DocFile instances which represent all *.doc files which are included in the project. 
        /// DocFile instances are wrapper objects which provide discrete accessors to relevant *.doc file properties.
        /// </summary>
        public static IReadOnlyList<DocFile> DocFiles {
            get {
                return FileManager.docFiles;
            }
        }


        public static readonly WrapperDict WrapperMap = new WrapperDict();

        #endregion


        #region Fields

        public static bool Initialized {
            get;
            set;
        }

        static HashSet<string> localDocumentNames = new HashSet<string>();

        static List<DocFile> docFiles = new List<DocFile>();

        static List<DocXFile> docXFiles = new List<DocXFile>();

        static List<TextFile> textFiles = new List<TextFile>();

        static List<TaggedFile> taggedFiles = new List<TaggedFile>();

        public static List<TaggedFile> TaggedFiles {
            get {
                return FileManager.taggedFiles;
            }
            set {
                FileManager.taggedFiles = value;
            }
        }

        #endregion
    }
    #region Exception Types


    [Serializable]
    class UnsupportedFileTypeAddedException : FileSystemException
    {
        public UnsupportedFileTypeAddedException(string unsupportedFormat)
            : this(unsupportedFormat, null) {
        }
        public UnsupportedFileTypeAddedException(string unsupportedFormat, Exception inner)
            : base(
            String.Format(
            "Files of type \"{0}\" are not supported. Supported types are {1}, {2}, {3}, and {4}",
            unsupportedFormat,
            from k in FileManager.WrapperMap.Keys.Take(4)
            select k), inner) {

        }

        public UnsupportedFileTypeAddedException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context) {
        }
    }
    #region Helper Types
    public class WrapperDict : Dictionary<string, Func<string, InputFile>>
    {
        internal WrapperDict()
            : base(
            new Dictionary<string, Func<string, InputFile>> {
                { "txt" , p => new TextFile(p) },
                { "doc" , p => new DocFile(p) },
                { "docx" , p => new DocXFile(p) },
                { "tagged" , p => new TaggedFile(p) }
        }) {
        }

        public new Func<string, InputFile> this[string fileExtension] {
            get {
                return base[fileExtension.Replace(".", "")];
            }
        }

    }

    #endregion
    [Serializable]
    class FileManagerException : FileSystemException
    {

        protected FileManagerException(string message)
            : base(message) {
            CollectDirInfo();
        }

        protected FileManagerException(string message, Exception inner)
            : base(message, inner) {
            CollectDirInfo();
        }


        public FileManagerException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context) {
            CollectDirInfo();
        }

        protected virtual void CollectDirInfo() {
            filesInProjectDirectories = from internalFile in new DirectoryInfo(FileManager.ProjectDir).EnumerateFiles("*", SearchOption.AllDirectories)
                                        select FileManager.WrapperMap[internalFile.Extension](internalFile.FullName);
        }

        private IEnumerable<InputFile> filesInProjectDirectories = new List<InputFile>();

        public IEnumerable<InputFile> FilesInProjectDirectories {
            get {
                return filesInProjectDirectories;
            }
            protected set {
                filesInProjectDirectories = value;
            }
        }

    }
    [Serializable]
    abstract class FileSystemException : Exception
    {

        protected FileSystemException(string message)
            : base(message) {

        }

        protected FileSystemException(string message, Exception inner)
            : base(message, inner) {

        }


        public FileSystemException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context) {
        }





    }

    #endregion

    #region Internal Extension Method Providers

    internal static class InputFileExtensions
    {
        public static dynamic AsDynamic(this InputFile inputFile) {
            return inputFile;
        }
    }

    #endregion

}




