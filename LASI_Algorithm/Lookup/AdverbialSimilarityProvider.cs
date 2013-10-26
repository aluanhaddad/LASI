﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LASI.Core.Patternization;
using System.Threading.Tasks;

namespace LASI.Core.ComparativeHeuristics
{
    public static class AdverbialSimilarityProvider
    {
        /// <summary>
        /// Determines if two IAdverbial instances are similar.
        /// </summary>
        /// <param name="first">The first IAdverbial</param>
        /// <param name="second">The second IAdverbial</param>
        /// <returns>True if the given IAdverbial instances are similar, false otherwise.</returns>
        /// <remarks>There are two calling conventions for this method. See the following examples
        /// <code>if ( Lookup.IsSimilarTo(d1, d2) ) { ... }</code>
        /// <code>if ( a1.IsSimilarTo(a2) ) { ... }</code> 
        /// Please prefer the second convention.
        /// </remarks>
        public static SimilarityResult IsSimilarTo(this IAdverbial first, IAdverbial second) {
            return new SimilarityResult(
                first.Match().Yield<bool>()
                    .When(first.Text.ToUpper() == second.Text.ToUpper())
                        .Then(true)
                    .Case<Adverb>(a1 =>
                        second.Match().Yield<bool>()
                            .Case<Adverb>(a2 => a1.IsSynonymFor(a2))
                            .Case<AdverbPhrase>(ap2 => ap2.IsSimilarTo(a1))
                        .Result())
                    .Case<AdverbPhrase>(ap1 =>
                        second.Match().Yield<bool>()
                        .Case<AdverbPhrase>(ap2 => ap1.IsSimilarTo(ap2))
                        .Case<Adverb>(a2 => ap1.IsSimilarTo(a2))
                    .Result())
                .Result());
        }
        /// <summary>
        /// Determines if the two provided Adverb instances are similar.
        /// </summary>
        /// <param name="first">The first Adverb.</param>
        /// <param name="second">The second Adverb.</param>
        /// <returns>True if the first Adverb is similar to the second, false otherwise.</returns>
        /// <remarks>There are two calling conventions for this method. See the following examples:
        /// <code>if ( Lookup.IsSimilarTo(a1, a2) ) { ... }</code>
        /// <code>if ( a1.IsSimilarTo(a2) ) { ... }</code>
        /// Please prefer the second convention.
        /// </remarks>
        public static SimilarityResult IsSimilarTo(this Adverb first, Adverb second) {
            return new SimilarityResult(first.IsSynonymFor(second));
        }
        /// <summary>
        /// Determines if the provided AdverbPhrase is similar to the provided Adverb.
        /// </summary>
        /// <param name="first">The AdverbPhrase.</param>
        /// <param name="second">The Adjective.</param>
        /// <returns>True if the provided AdverbPhrase is similar to the provided Adverb, false otherwise.</returns>
        /// <remarks>There are two calling conventions for this method. See the following examples:
        /// <code>if ( Lookup.IsSimilarTo(ap1, a2) ) { ... }</code>
        /// <code>if ( ap1.IsSimilarTo(a2) ) { ... }</code>
        /// Please prefer the second convention.
        /// </remarks>
        public static SimilarityResult IsSimilarTo(this AdverbPhrase first, Adverb second) {
            return second.IsSimilarTo(first);
        }
        /// <summary>
        /// Determines if the provided Adverb is similar to the provided AdverbPhrase.
        /// </summary>
        /// <param name="first">The Adverb.</param>
        /// <param name="second">The AdverbPhrase.</param>
        /// <returns>True if the provided Adverb is similar to the provided AdverbPhrase, false otherwise.</returns>
        /// <remarks>There are two calling conventions for this method. See the following examples:
        /// <code>if ( Lookup.IsSimilarTo(a1, ap2) ) { ... }</code>
        /// <code>if ( a1.IsSimilarTo(ap2) ) { ... }</code>
        /// Please prefer the second convention.
        /// </remarks>
        public static SimilarityResult IsSimilarTo(this Adverb first, AdverbPhrase second) {
            return new SimilarityResult(second.Words.OfAdverb().Any(adj => adj.IsSynonymFor(first)));
            // Must refine this to check for negators and modals which will potentially invert the meaning.
        }
        /// <summary>
        /// Determines if two AdverbPhrases are similar.
        /// </summary>
        /// <param name="first">The first AdverbPhrase</param>
        /// <param name="second">The second AdverbPhrase</param>
        /// <returns>True if the given AdverbPhrases are similar, false otherwise.</returns>
        /// <remarks>There are two calling conventions for this method. See the following examples:
        /// <code>if ( Lookup.IsSimilarTo(ap1, ap2) ) { ... }</code>
        /// <code>if ( ap1.IsSimilarTo(ap2) ) { ... }</code>
        /// Please prefer the second convention.
        /// </remarks>
        public static SimilarityResult IsSimilarTo(this AdverbPhrase first, AdverbPhrase second) {
            var synResults =
                first.Words.OfAdverb()
                .Zip(
                second.Words.OfAdverb(),
                    (a, b) => a.IsSynonymFor(b)
                )
                .Aggregate(new { Trues = 0f, Falses = 0f },
                    (a, c) => new { Trues = a.Trues + (c ? 1 : 0), Falses = a.Falses + (c ? 0 : 1) }
                );
            return new SimilarityResult(first == second || synResults.Trues / (synResults.Falses + synResults.Trues) > Lookup.SIMILARITY_THRESHOLD);
        }
    }
}