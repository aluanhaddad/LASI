﻿using LASI.Algorithm;
using LASI.Algorithm.Binding;
using LASI.Algorithm.Thesauri;
using LASI.Algorithm.Weighting;
using LASI.FileSystem;
using LASI.FileSystem.FileTypes;
using LASI.Utilities;
using SharpNLPTaggingModule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
namespace Aluan_Experimentation
{
    public class Program
    {

        static void Main(string[] args) {

            var docString = TaggerUtil.TagString(@"The students are happy about their good grades.");
            print(docString);
            var doc = TaggerUtil.TaggedToDoc(docString);
            SubjectBinder subjectBinder = new LASI.Algorithm.SubjectBinder();
            ObjectBinder objectBinder = new ObjectBinder();

            foreach (var s in doc.Sentences) {
                subjectBinder.Bind(s);
                objectBinder.Bind(s);
            }


            foreach (var r in doc.Phrases)
                print(r);
            StdIO.WaitForKey();
        }

        private static Document LoadDoc() {

            var doc = new TaggedFileParser(new TaggedFile(@"C:\Users\Aluan\Desktop\sec2-2.tagged")).LoadDocument();
            return doc;
        }
        private static void TagDoc() {
            var docxConverter = new DocxToTextConverter(new DocXFile(@"C:\Users\Aluan\Desktop\sec2-2.docx"));
            docxConverter.ConvertFile();
            var tagger = new SharpNLPTagger(TaggingOption.TagAndAggregate, @"C:\Users\Aluan\Desktop\sec2-2.txt");
            tagger.ProcessFile();
        }

        private static void TestSubjectObject() {
            var vT = LASI.Algorithm.Thesauri.Thesauri.VerbThesaurus;
            var nT = LASI.Algorithm.Thesauri.Thesauri.NounThesaurus;
            LASI.Utilities.TaggerUtil.TaggerOption = TaggingOption.TagAndAggregate;
            var str = TaggerUtil.TagString("He will have a blue ball, a red ball, a green ball, and a black ball.");
            Console.WriteLine(str);
            var doc = TaggerUtil.TaggedToDoc(str);
            SubjectBinder subjectBinder = new LASI.Algorithm.SubjectBinder();
            subjectBinder.Bind(doc.Sentences.First());
            ObjectBinder objectBinder = new ObjectBinder();
            objectBinder.Bind(doc.Sentences.First());
            foreach (var phrase in doc.Phrases)
                print(phrase);
        }

        private static void ParseAndCreate() {

            TaggerUtil.TaggerOption = TaggingOption.NameFind;
            var str = TaggerUtil.TagString(new[]{
                @"Add one plus one."
            });

            print(str);
            TaggerUtil.TaggerOption = TaggingOption.TagAndAggregate;
            str = TaggerUtil.TagString(str);

            print(str);

            var document = TaggerUtil.TaggedToDoc(str);
            foreach (var S in CountByTypeAndText(document).Result) {
                print(S);
            }

            var actions = document.GetActions();
            actions.WithSubject(e => e.Text == null);
            foreach (var A in actions) {
                print(A);
            }
            StdIO.WaitForKey(ConsoleKey.Escape);
        }



        private static async Task<Document> MakeDocumentFromTaggedFile(string filePath) {

            return await Task.Run(async () => await new TaggedFileParser(new TaggedFile(filePath)).LoadDocumentAsync());


        }

        private static async Task<IEnumerable<string>> CountByTypeAndText(Document document) {
            return await Task.Run(() => {
                var phrasePOSCounts = from R in document.Phrases
                                      group R by new {
                                          Type = R.GetType(),
                                          R.Text
                                      } into G
                                      orderby G.Count()
                                      select G;
                return from g in phrasePOSCounts
                       select String.Format("{0} : \"{1}\"; with count: {2}:", g.Key.Type.Name, g.Key.Text, g.Count());
            });
        }

        static void ThesaurusCMDLineTest() {
            var verbLookUp = new LASI.Algorithm.Thesauri.VerbThesaurus(@"..\..\..\..\WordNetThesaurusData\data.verb");
            verbLookUp.Load();

            Console.Write("Enter a Verb:    ");
            var input = Console.ReadLine();
            while (input != "~") {

                try {
                    foreach (var v in verbLookUp[input]) {
                        Console.Write(v + ", ");
                    }
                } catch (KeyNotFoundException) {
                    Console.WriteLine(String.Format("No synonyms recognized for \"{0}\" : as verb", input));
                }
                Console.WriteLine();
                Console.Write("Enter a Verb:    ");
                input = Console.ReadLine();
            }
        }


        static Action<object> print = (o) => Console.WriteLine(o);
        static Action<object> printFile = (o) => {
            using (var writer = new StreamWriter(@"..\..\..\..\Desktop\taggingModeTesting.txt", true)) {
                writer.WriteLine(o);
            }
        };
    }
}