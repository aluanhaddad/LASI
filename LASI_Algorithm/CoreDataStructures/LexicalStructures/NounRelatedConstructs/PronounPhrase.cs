﻿using LASI.Core.ComparativeHeuristics;
using LASI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LASI.Core
{
    /// <summary>
    /// This class is Experimental! It represents a PronounPhrase. A PronounPhrase is a custom Phrase type introduced by LASI.
    /// It it corresponds to NounPhrases which either contain almost exclusively Pronoun Words or which can be contextually 
    /// determined to refer to some previously mentioned entity within a constraned scope.
    /// </summary>
    public class PronounPhrase : NounPhrase, IReferencer
    {
        /// <summary>
        /// Initializes a new instance of the PronounPhrase class.
        /// </summary>
        /// <param name="composedWords">The words which compose to form the PronounPhrase.</param>
        public PronounPhrase(IEnumerable<Word> composedWords)
            : base(composedWords) {
            if (composedWords.OfPronoun().Any(p => p.Referent != null)) {
                _refersTo = new AggregateEntity(composedWords.OfPronoun().Select(p => p.Referent));
            }
        }

        /// <summary>
        /// Returns a string representation of the PronounPhrase
        /// </summary>
        /// <returns>A string representation of the PronounPhrase</returns>
        public override string ToString() {
            var result = base.ToString() + (Referent != null && Referent.Any() ? " referring to -> " + Referent.Text : string.Empty);
            result += (AliasLookup.GetDefinedAliases(Referent ?? this as IEntity).Any() ? "\nClassified as: " + AliasLookup.GetDefinedAliases(Referent ?? this as IEntity).Format() : string.Empty);
            return result;
        }
        private IAggregateEntity _refersTo;
        /// <summary>
        /// Gets the Entity which the IPronoun references.
        /// </summary>
        public IAggregateEntity Referent {
            get {
                _refersTo = _refersTo ?? new AggregateEntity(Words.OfPronoun().Where(p => p.Referent != null).Select(p => p.Referent));
                return _refersTo;
            }

        }

        /// <summary>
        /// Binds the PronounPhrase to refer to the given Entity.
        /// </summary>
        /// <param name="target">The entity to which to bind.</param>
        public void BindAsReference(IEntity target) {
            if (_refersTo == null) {
                _refersTo = new AggregateEntity(new[] { target });
            } else {
                _refersTo = new AggregateEntity(_refersTo.Append(target));
            }
            EntityKind = _refersTo.EntityKind;
        }




    }
}
