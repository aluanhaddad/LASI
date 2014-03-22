﻿using LASI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LASI.Core.Heuristics
{
    abstract class SynSet<TSetRelationship> : IEquatable<SynSet<TSetRelationship>>
    {
        protected SynSet(int id, IEnumerable<string> words, IEnumerable<KeyValuePair<TSetRelationship, int>> pointerRelationships) {
            Id = id;
            Words = new HashSet<string>(words);
            relatedSetsByRelationKindSource = pointerRelationships;
            ReferencedIndeces = new HashSet<int>(pointerRelationships.Select(p => p.Value));
        }
        /// <summary>
        /// Gets the ID of the SynSet.
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// Gets all of the words belonging to the SynSet.
        /// </summary>
        public HashSet<string> Words { get; private set; }
        /// <summary>
        /// Gets the IDs of all sets referenced by the SynSet.
        /// </summary>
        public HashSet<int> ReferencedIndeces { get; private set; }
        /// <summary>
        /// Returns the IDs of all other SynSets which are referenced from the current SynSet in the indicated fashion. 
        /// </summary>
        /// <param name="relationships">The kinds of external set relationships to consider return.</param>
        /// <returns>The IDs of all other SynSets which are referenced from the current SynSet in the indicated fashion.</returns>
        public IEnumerable<int> this[params TSetRelationship[] relationships] {

            get {
                if (relatedSetsByRelationKind == null)
                    relatedSetsByRelationKind = relatedSetsByRelationKindSource.ToLookup(p => p.Key, p => p.Value);
                foreach (var r in relationships) {
                    foreach (var related in relatedSetsByRelationKind[r]) { yield return related; }
                }
            }
        }
        private ILookup<TSetRelationship, int> relatedSetsByRelationKind;
        private IEnumerable<KeyValuePair<TSetRelationship, int>> relatedSetsByRelationKindSource;
        /// <summary>
        /// Returns the IDs of all other SynSets which are referenced from the current SynSet in the indicated fashion. 
        /// </summary>
        /// <returns>The IDs of all other SynSets which are referenced from the current SynSet in the indicated fashion.</returns>
        public ILookup<TSetRelationship, int> RelatedSetsByRelationKind {
            get { if (relatedSetsByRelationKind == null)relatedSetsByRelationKind = relatedSetsByRelationKindSource.ToLookup(p => p.Key, p => p.Value); return relatedSetsByRelationKind; }
        }

        public override int GetHashCode() {
            return Id;
        }
        public bool Equals(SynSet<TSetRelationship> other) {
            return this == other;
        }

        public override bool Equals(object obj) {
            return obj as SynSet<TSetRelationship> == this;
        }
        /// <summary>
        /// Returns a single string representing the SynSet.
        /// </summary>
        /// <returns>A single string representing the SynSet.</returns>
        public override string ToString() {
            return "[" + Id + "] " + Words
                .Format(Tuple.Create(' ', ',', ' '));
        }
        public static bool operator ==(SynSet<TSetRelationship> lhs, SynSet<TSetRelationship> rhs) {
            if (ReferenceEquals(lhs, null))
                return ReferenceEquals(rhs, null);
            if (ReferenceEquals(rhs, null))
                return ReferenceEquals(lhs, null);
            return lhs.Id == rhs.Id;
        }
        public static bool operator !=(SynSet<TSetRelationship> lhs, SynSet<TSetRelationship> rhs) {
            return !(lhs == rhs);
        }
    }
    /// <summary>
    /// Represents a synset parsed from a line of the data.noun file of the WordNet package.
    /// </summary>
    internal sealed class NounSynSet : SynSet<NounSetLink>
    {
        public NounSynSet(int id, IEnumerable<string> words, IEnumerable<KeyValuePair<NounSetLink, int>> pointerRelationships, NounLookup.Category lexicalCategory)
            : base(id, words, pointerRelationships) {
            LexicalCategory = lexicalCategory;
        }

        public NounLookup.Category LexicalCategory { get; private set; }
    }
    /// <summary>
    /// Represents a synset parsed from a line of the data.verb file of the WordNet package.
    /// </summary>
    internal sealed class VerbSynSet : SynSet<VerbSetLink>
    {
        public VerbSynSet(int id, IEnumerable<string> words, IEnumerable<KeyValuePair<VerbSetLink, int>> pointerRelationships, VerbLookup.Category lexicalCategory)
            : base(id, words, pointerRelationships) {
            Category = lexicalCategory;
        }
        public VerbLookup.Category Category { get; private set; }
    }
    /// <summary>
    /// Represents a synset parsed from the data.adj file of the WordNet package.
    /// </summary>
    internal sealed class AdjectiveSynSet : SynSet<AdjectiveSetLink>
    {
        public AdjectiveSynSet(int id, IEnumerable<string> words, IEnumerable<KeyValuePair<AdjectiveSetLink, int>> pointerRelationships, AdjectiveLookup.Category lexicalCategory)
            : base(id, words, pointerRelationships) {
            LexicalCategory = lexicalCategory;
        }

        public AdjectiveLookup.Category LexicalCategory { get; private set; }
    }
    /// <summary>
    /// Represents a synset parsed from a line of the data.adv file of the WordNet package.
    /// </summary>
    internal sealed class AdverbSynSet : SynSet<AdverbSetLink>
    {
        public AdverbSynSet(int id, IEnumerable<string> words, IEnumerable<KeyValuePair<AdverbSetLink, int>> pointerRelationships, AdverbLookup.Category lexicalCategory)
            : base(id, words, pointerRelationships) {
            LexicalCategory = lexicalCategory;
        }
        public AdverbLookup.Category LexicalCategory { get; private set; }
    }

}