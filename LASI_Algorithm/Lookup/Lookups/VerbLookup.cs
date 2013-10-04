﻿using LASI.Algorithm.LexicalLookup.InterSetRelationshipManagement;
using LASI.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LASI.Algorithm.LexicalLookup
{
    using LASI.Algorithm.LexicalLookup.Morphemization;
    using SetReference = System.Collections.Generic.KeyValuePair<VerbSetRelationship, int>;

    internal sealed class VerbLookup : IWordNetLookup<Verb>
    {
        /// <summary>
        /// Initializes a new instance of the VerbThesaurus class. 
        /// </summary>
        /// <param name="path">The path of the WordNet database file containing the sysnonym data for verbals.</param>
        public VerbLookup(string path) {
            filePath = path;
        }
        /// <summary>
        /// Parses the contents of the underlying WordNet database file.
        /// </summary>
        public void Load() {
            using (var reader = new StreamReader(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None, 10024, FileOptions.SequentialScan))) {
                var fileLines = reader.ReadToEnd().Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Skip(HEADER_LENGTH);
                foreach (var line in fileLines) {
                    LinkSynset(CreateSet(line));
                }
            }
        }
        public async Task LoadAsync() {
            await System.Threading.Tasks.Task.Run(() => Load());
        }

        private VerbSynSet CreateSet(string fileLine) {
            var line = fileLine.Substring(0, fileLine.IndexOf('|'));

            var referencedSets = from Match M in Regex.Matches(line, POINTER_REGEX)
                                 let split = M.Value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                 where split.Count() > 1
                                 select new SetReference(RelationMap[split[0]], Int32.Parse(split[1]));

            var words = from Match ContainedWord in Regex.Matches(line.Substring(17), WORD_REGEX)
                        select ContainedWord.Value.Replace('_', '-').ToLower();
            var id = Int32.Parse(line.Substring(0, 8));
            var lexCategory = (VerbCategory)Int32.Parse(line.Substring(9, 2));
            return new VerbSynSet(id, words, referencedSets, lexCategory);
        }


        private void LinkSynset(VerbSynSet synset) {
            setsBySetID[synset.ID] = synset;
            foreach (var word in synset.Words) {
                if (verbData.ContainsKey(word)) {
                    var newSet = new VerbSynSet(
                        verbData[word].ID,
                        verbData[word].Words.Concat(synset.Words),
                        verbData[word].RelatedSetsByRelationKind
                            .Concat(synset.RelatedSetsByRelationKind)
                            .SelectMany(grouping => grouping.Select(pointer => new SetReference(grouping.Key, pointer))), verbData[word].LexName);
                    verbData[word] = newSet;
                } else {
                    verbData[word] = synset;
                }
            }
        }

        private ISet<string> SearchFor(string search) {
            try {
                var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var verbRoots = VerbMorpher.FindRoots(search);
                VerbSynSet? containingSet = null;
                foreach (var root in verbRoots) {
                    VerbSynSet tryGetVal;
                    if (verbData.TryGetValue(root, out tryGetVal)) { containingSet = tryGetVal; } else {

                        try {
                            containingSet = verbData.First(kv => kv.Value.Words.Contains(root)).Value;
                        }
                        catch (InvalidOperationException) { }
                    }
                    result.UnionWith(

                        containingSet != null ?
                        containingSet.Value.ReferencedIndexes
                             .SelectMany(id => { VerbSynSet temp; return setsBySetID.TryGetValue(id, out temp) ? temp.ReferencedIndexes : Enumerable.Empty<int>(); })
                             .Select(s => { VerbSynSet temp; return setsBySetID.TryGetValue(s, out temp) ? new Nullable<VerbSynSet>(temp) : new Nullable<VerbSynSet>(); })
                             .Where(s => s.HasValue)
                             .Select(s => s.Value)
                             .Where(s => s.LexName == containingSet.Value.LexName)
                             .SelectMany(s => s.Words.SelectMany(w => VerbMorpher.GetConjugations(w))).Append(root) : new[] { search });
                }
                return result;
            }
            catch (ArgumentOutOfRangeException) {
            }
            catch (IndexOutOfRangeException) {
            }
            catch (KeyNotFoundException) {
            }
            return new HashSet<string>(new[] { search }, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Retrives the synonyms of the given verb as an ISet of strings.
        /// </summary>
        /// <param name="search">The text of the verb to look for.</param>
        /// <returns>A collection of strings containing all of the synonyms of the given verb.</returns>
        public ISet<string> this[string search] {
            get {
                return SearchFor(search);
            }
        }

        /// <summary>
        /// Retrives the synonyms of the given Verb as an ISet of strings.
        /// </summary>
        /// <param name="search">An instance of Verb</param>
        /// <returns>A collection of strings containing all of the synonyms of the given Verb.</returns>
        public ISet<string> this[Verb search] {
            get {
                return this[search.Text];
            }
        }

        private ConcurrentDictionary<int, VerbSynSet> setsBySetID = new ConcurrentDictionary<int, VerbSynSet>();
        private ConcurrentDictionary<string, VerbSynSet> verbData = new ConcurrentDictionary<string, VerbSynSet>();
        private static VerbPointerSymbolMap RelationMap = new VerbPointerSymbolMap();
        private SortedSet<string> allVerbs;

        public SortedSet<string> AllVerbs { get { return allVerbs = allVerbs ?? new SortedSet<string>(verbData.SelectMany(set => set.Value.Words)); } }
        private const string WORD_REGEX = @"\b[A-Za-z-_]{2,}";
        private const string POINTER_REGEX = @"\D{1,2}\s*[\d]+[\d]+[\d]+[\d]+[\d]+[\d]+[\d]+[\d]+";
        private string filePath;
        private const int HEADER_LENGTH = 29;

    }



}