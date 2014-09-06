﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LASI.Core.Heuristics.WordNet
{
    using SetReference = KeyValuePair<AdverbLink, int>;
    internal sealed class AdverbLookup : WordNetLookup<Adverb, AdverbLink>
    {
        /// <summary>
        /// Initializes a new instance of the AdjectiveThesaurus class.
        /// </summary>
        /// <param name="path">The path of the WordNet database file containing the synonym data for adverbs.</param>
        public AdverbLookup(string path) {
            filePath = path;
        }

        HashSet<AdverbSynSet> allSets = new HashSet<AdverbSynSet>();

        /// <summary>
        /// Parses the contents of the underlying WordNet database file.
        /// </summary>
        internal override void Load() {
            using (StreamReader reader = new StreamReader(filePath)) {

                foreach (var line in reader.ReadToEnd().SplitRemoveEmpty('\n').Skip(HEADER_LENGTH)) {
                    try { allSets.Add(CreateSet(line)); }
                    catch (KeyNotFoundException e) {
                        Output.WriteLine("An error occured when Loading the {0}: {1}\r\n{2}", GetType().Name, e.Message, e.StackTrace);
                    }
                }

            }
        }


        private AdverbSynSet CreateSet(string fileLine) {


            var line = fileLine.Substring(0, fileLine.IndexOf('|'));

            var referencedSets = ParseReferencedSets(line, segments => new SetReference(interSetMap[segments[0]], int.Parse(segments[1])));

            IEnumerable<string> words = from match in Regex.Matches(line, wordRegex).Cast<Match>()
                                        select match.Value.Replace('_', '-');

            int id = int.Parse(line.Substring(0, 8));
            return new AdverbSynSet(id, words, referencedSets, AdverbCategory.All);
        }
        private ISet<string> SearchFor(string search) {
            return (from containingSet in allSets
                    where containingSet.ContainsWord(search)
                    from linkedSet in allSets
                    where containingSet.DirectlyReferences(linkedSet)
                    from word in linkedSet.Words
                    select word).ToHashSet(StringComparer.OrdinalIgnoreCase);

        }

        internal override ISet<string> this[string search] {
            get {
                var lexicalForms = conjugator.GetLexicalForms(search);
                return lexicalForms.SelectMany(form => SearchFor(form)).ToHashSet(StringComparer.OrdinalIgnoreCase);
            }
        }


        internal override ISet<string> this[Adverb search] {
            get {
                return this[search.Text];
            }
        }

        AdverbMorpher conjugator = new AdverbMorpher();
        private string filePath;

        // Provides an indexed lookup between the values of the AdjectivePointerSymbol enum and their corresponding string representation in WordNet data.adv files.
        protected override IReadOnlyDictionary<string, AdverbLink> InterSetMap {
            get {
                return interSetMap;
            }

        }
        private IReadOnlyDictionary<string, AdverbLink> interSetMap = new Dictionary<string, AdverbLink> {
            {"!", AdverbLink.Antonym },
            {@"\", AdverbLink.DerivedFromAdjective },
            {";c", AdverbLink.DomainOfSynset_TOPIC },
            {";r", AdverbLink.DomainOfSynset_REGION },
            {";u", AdverbLink.DomainOfSynset_USAGE }
        };
    }
    /// <summary>
    /// Defines the broad lexical categories assigned to Adverbs in the WordNet system.
    /// </summary>
    public enum AdverbCategory : byte
    {
        /// <summary>
        /// All adverbs have the same category. This value is simply included for completeness.
        /// </summary>
        All = 2

    }
}

