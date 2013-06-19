﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LASI.Algorithm.Thesuari;
using LASI.Utilities;

namespace LASI.Algorithm.Thesauri
{


    /// <summary>
    /// Provides an indexed lookup between the values of the Noun enum and their corresponding string representation in WordNet data files.
    /// </summary>
    internal class NounPointerSymbolMap
    {
        /// <summary>
        /// Gets the Noun value corresponding to the Key string. 
        /// The Value UNDEFINED is returned if no value is mapped to the key.
        /// </summary>
        /// <param name="key">The Key string for which to retrieve a Noun value.</param>
        /// <returns>The Noun value corresponding to the Key string.
        /// The Value UNDEFINED is returned if no value is mapped to the key.</returns>
        public NounPointerSymbol this[string key] {
            get {
                NounPointerSymbol result;
                data.TryGetValue(key, out result);
                return result;
            }
        }
        public override string ToString() {
            return data.Format(pair => string.Format("\"{0}\" -> {1}", pair.Key, pair.Value));
        }
        private static readonly IReadOnlyDictionary<string, NounPointerSymbol> data = new Dictionary<string, NounPointerSymbol>{ 
            { "!", NounPointerSymbol.Antonym },
            { "@", NounPointerSymbol.HypERnym },
            { "@i", NounPointerSymbol.InstanceHypERnym },
            { "~", NounPointerSymbol.HypOnym },
            { "~i", NounPointerSymbol.InstanceHypOnym },
            { "#m", NounPointerSymbol.MemberHolonym },
            { "#s", NounPointerSymbol.SubstanceHolonym },
            { "#p", NounPointerSymbol.PartHolonym },
            { "%m", NounPointerSymbol.MemberMeronym },
            { "%s", NounPointerSymbol.SubstanceMeronym },
            { "%p", NounPointerSymbol.PartMeronym },
            { "=", NounPointerSymbol.Attribute },
            { "+", NounPointerSymbol.DerivationallyRelatedForm },
            { ";c", NounPointerSymbol.DomainOfSynset_TOPIC },
            { "-c", NounPointerSymbol.MemberOfThisDomain_TOPIC },
            { ";r", NounPointerSymbol.DomainOfSynset_REGION },
            { "-r", NounPointerSymbol.MemberOfThisDomain_REGION },
            { ";u", NounPointerSymbol.DomainOfSynset_USAGE },
            { "-u", NounPointerSymbol.MemberOfThisDomain_USAGE }
        };
    }
    /// <summary>
    /// Provides an indexed lookup between the values of the VerbPointerSymbol enum and their corresponding string representation in WordNet data files.
    /// </summary>
    internal class VerbPointerSymbolMap
    {
        /// <summary>
        /// Gets the VerbPointerSymbol value corresponding to the Key string. 
        /// The Value UNDEFINED is returned if no value is mapped key.
        /// </summary>
        /// <param name="key">The Key string for which to retrieve a Noun value.</param>
        /// <returns>The VerbPointerSymbol value corresponding to the Key string.
        /// The Value UNDEFINED is returned if no value is mapped to the key.</returns>
        public VerbPointerSymbol this[string key] {
            get {
                VerbPointerSymbol result;
                data.TryGetValue(key, out result);
                return result;
            }
        }
        private static readonly IReadOnlyDictionary<string, VerbPointerSymbol> data = new Dictionary<string, VerbPointerSymbol> {
            { "!", VerbPointerSymbol. Antonym }, 
            { "@", VerbPointerSymbol.Hypernym },
            { "~", VerbPointerSymbol.Hyponym },
            { "*", VerbPointerSymbol.Entailment },
            { ">", VerbPointerSymbol.Cause },
            { "^", VerbPointerSymbol. Also_see },
            { "$", VerbPointerSymbol.Verb_Group },
            { "+", VerbPointerSymbol.DerivationallyRelatedForm },
            { ";c", VerbPointerSymbol.DomainOfSynset_TOPIC },
            { ";r", VerbPointerSymbol.DomainOfSynset_REGION },
            { ";u", VerbPointerSymbol.DomainOfSynset_USAGE}
        };
    }
    internal class NounSetIDSymbolMap : ILookup<NounPointerSymbol, int>
    {
        public NounSetIDSymbolMap(IEnumerable<KeyValuePair<NounPointerSymbol, int>> relationData) {
            data = (from pair in relationData
                    group pair.Value by pair.Key into g
                    select new KeyValuePair<NounPointerSymbol, HashSet<int>>(g.Key, new HashSet<int>(g))).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        private IDictionary<NounPointerSymbol, HashSet<int>> data;
        private IEnumerable<IGrouping<NounPointerSymbol, int>> groupData;



        public IEnumerable<int> this[NounPointerSymbol key] {
            get {
                return data.ContainsKey(key) ? data[key] : Enumerable.Empty<int>();
            }
        }

        public int Count {
            get {
                return data.Count;
            }
        }

        public bool Contains(NounPointerSymbol key) {
            return data.ContainsKey(key);
        }

        public IEnumerator<IGrouping<NounPointerSymbol, int>> GetEnumerator() {
            groupData = groupData ?? from pair in data
                                     from value in pair.Value
                                     group value by pair.Key;
            return groupData.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            throw new NotImplementedException();
        }
    }

    internal class VerbSetIDSymbolMap : ILookup<VerbPointerSymbol, int>
    {
        public VerbSetIDSymbolMap(IEnumerable<KeyValuePair<VerbPointerSymbol, int>> relationData) {
            data = (from pair in relationData
                    group pair.Value by pair.Key into g
                    select new KeyValuePair<VerbPointerSymbol, HashSet<int>>(g.Key, new HashSet<int>(g))).ToDictionary(pair => pair.Key, pair => pair.Value);
        }
        private IDictionary<VerbPointerSymbol, HashSet<int>> data;
        private IEnumerable<IGrouping<VerbPointerSymbol, int>> groupData;
        public IEnumerable<int> this[VerbPointerSymbol key] {
            get {
                return data.ContainsKey(key) ? data[key] : Enumerable.Empty<int>();
            }
        }

        public int Count {
            get {
                return data.Count;
            }
        }

        public bool Contains(VerbPointerSymbol key) {
            return data.ContainsKey(key);
        }

        public IEnumerator<IGrouping<VerbPointerSymbol, int>> GetEnumerator() {
            groupData = groupData ?? from pair in data
                                     from value in pair.Value
                                     group value by pair.Key;
            return groupData.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            throw new NotImplementedException();
        }
    }
    public class NounSynSet
    {

        public WordNetNounCategory LexName {
            get;
            private set;
        }
        public NounSynSet(int ID, IEnumerable<string> words, IEnumerable<int> pointers, WordNetNounCategory lexCategory) {
            this.ID = ID;
            Words = new HashSet<string>(words);
            ReferencedIndexes = new HashSet<int>(pointers);
            LexName = lexCategory;

        }
        public NounSynSet(int ID, IEnumerable<string> words, IEnumerable<KeyValuePair<NounPointerSymbol, int>> pointerRelations, WordNetNounCategory lexCategory) {
            this.ID = ID;
            Words = new HashSet<string>(words);
            relatedOnPointerSymbol = new NounSetIDSymbolMap(pointerRelations);
            ReferencedIndexes = new HashSet<int>(from pair in pointerRelations
                                                 select pair.Value);
            LexName = lexCategory;
        }
        /// <summary>
        /// Returns a single string representing the members of the VerbThesaurusSynSet.
        /// </summary>
        /// <returns>A single string representing the members of the VerbThesaurusSynSet.</returns>
        public override string ToString() {
            return "[" + ID + "] " + Words.Aggregate("", (str, code) => {
                return str + "  " + code;
            });
        }
        public override int GetHashCode() {
            return ID;
        }
        public override bool Equals(object obj) {
            return this == obj as NounSynSet;
        }
        public static bool operator ==(NounSynSet lhs, NounSynSet rhs) {
            if (ReferenceEquals(lhs, null))
                return ReferenceEquals(rhs, null);
            if (ReferenceEquals(rhs, null))
                return ReferenceEquals(lhs, null);
            return lhs.ID == rhs.ID;
        }
        public static bool operator !=(NounSynSet lhs, NounSynSet rhs) {
            return !(lhs == rhs);
        }

        public IEnumerable<int> this[NounPointerSymbol pointerSymbol] {
            get {
                return relatedOnPointerSymbol[pointerSymbol];
            }
        }

        private NounSetIDSymbolMap relatedOnPointerSymbol;


        public HashSet<string> Words {
            get;
            private set;

        }
        public HashSet<int> ReferencedIndexes {
            get;
            private set;
        }

        public int ID {
            get;
            private set;
        }

    }
    public class VerbSynSet
    {
        public WordNetVerbCategory LexName {
            get;
            private set;
        }

        public VerbSynSet(int ID, IEnumerable<string> words, IEnumerable<KeyValuePair<VerbPointerSymbol, int>> pointerRelations, WordNetVerbCategory lexCategory) {
            this.ID = ID;
            Words = new HashSet<string>(words);
            RelatedOnPointerSymbol = new VerbSetIDSymbolMap(pointerRelations);
            ReferencedIndexes = new HashSet<int>(from pair in pointerRelations
                                                 select pair.Value);
            LexName = lexCategory;
        }
        /// <summary>
        /// Returns a single string representing the members of the VerbThesaurusSynSet.
        /// </summary>
        /// <returns>A single string representing the members of the VerbThesaurusSynSet.</returns>
        public override string ToString() {
            return "[" + ID + "] " + Words.Aggregate("", (str, code) => {
                return str + "  " + code;
            });
        }
        public override int GetHashCode() {
            return ID;
        }
        public override bool Equals(object obj) {
            return this == obj as VerbSynSet;
        }
        public static bool operator ==(VerbSynSet lhs, VerbSynSet rhs) {
            if (ReferenceEquals(lhs, null))
                return ReferenceEquals(rhs, null);
            if (ReferenceEquals(rhs, null))
                return ReferenceEquals(lhs, null);
            return lhs.ID == rhs.ID;
        }
        public static bool operator !=(VerbSynSet lhs, VerbSynSet rhs) {
            return !(lhs == rhs);
        }
        public IEnumerable<int> this[VerbPointerSymbol pointerSymbol] {
            get {
                return RelatedOnPointerSymbol[pointerSymbol];
            }
        }

        internal VerbSetIDSymbolMap RelatedOnPointerSymbol {
            get;
            set;
        }


        public HashSet<string> Words {
            get;
            private set;

        }

        public HashSet<int> ReferencedIndexes {
            get;
            private set;
        }

        public int ID {
            get;
            private set;
        }

    }
}