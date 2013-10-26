﻿using LASI.Core.DocumentStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LASI.Core.Binding
{
    /// <summary>
    /// A Binder which binds the AdjectivePhrases within a sentence to Applicable NounPhrases.
    /// </summary>
    public static class AdjectivePhraseBinder
    {
        /// <summary>
        /// Binds the AdjectivePhrases within a sentence to Applicable NounPhrases.
        /// </summary>
        /// <param name="sentence">The Sentence to bind within.</param>
        public static void Bind(Sentence sentence) {
            foreach (var bindingAction in GetPossibilities(sentence)) {
                bindingAction();
            }
        }

        private static IEnumerable<Action> GetPossibilities(Sentence sentence) {
            return
                from bindingPair in
                    (
                        from ADJP in sentence.Phrases.OfAdjectivePhrase()
                        let NP = ADJP.NextPhrase as NounPhrase
                        select new { ADJP, NP })
                        .Concat(
                        from clause in sentence.Clauses
                        from ADJP in clause.Phrases.Reverse().Take(1).OfAdjectivePhrase()
                        let NP = ADJP.PreviousPhrase as NounPhrase
                        select new { ADJP, NP })
                where bindingPair.NP != null && bindingPair.NP.Clause == bindingPair.ADJP.Clause
                select new Action(() => bindingPair.NP.BindDescriptor(bindingPair.ADJP));
        }
    }
}
