﻿using System.Collections.Generic;
using System.Linq;

namespace LASI.Core.Heuristics
{

    public static partial class Lookup
    {
        /// <summary> Determines if two IEntity instances are similar. </summary>
        /// <param name="first"> The first IEntity </param>
        /// <param name="second"> The second IEntity </param>
        /// <returns> <c>true</c> if the given IEntity instances are similar; otherwise, <c>false</c>. </returns>
        /// <remarks>
        /// There are two calling conventions for this method. See the following examples 
        /// <code>
        /// if (Lookup.IsSimilarTo(e1, e2)) { ... }
        /// </code>
        /// <code>
        /// if (e1.IsSimilarTo(e2)) { ... }
        /// </code> Please prefer the second convention. 
        /// </remarks>
        public static Similarity IsSimilarTo(this IEntity first, IEntity second) {
            return first.Match().Yield<Similarity>()
                    .When(first.Text.EqualsIgnoreCase(second.Text)).Then(Similarity.Similar)
                    .Case((IAggregateEntity ae1) => second.Match().Yield<Similarity>()
                            .Case((IAggregateEntity ae2) => ae1.IsSimilarTo(ae2))
                            .Case((IEntity e2) => new Similarity(ae1.Any(entity => entity.IsSimilarTo(e2))))
                        .Result())
                    .Case((Noun n1) => second.Match().Yield<Similarity>()
                            .Case((Noun n2) => n1.IsSimilarTo(n2))
                            .Case((NounPhrase np2) => n1.IsSimilarTo(np2))
                          .Result())
                    .Case((NounPhrase np1) => second.Match().Yield<Similarity>()
                          .Case((NounPhrase np2) => np1.IsSimilarTo(np2))
                          .Case((Noun n2) => np1.IsSimilarTo(n2))
                        .Result())
                    .Result();
        }

        /// <summary> Determines if two IAggregateEntity instances are similar. </summary>
        /// <param name="first"> The first IAggregateEntity </param>
        /// <param name="second"> The second IAggregateEntity </param>
        /// <returns> <c>true</c> if the given IAggregateEntity instances are similar; otherwise, <c>false</c>. </returns>
        /// <remarks>
        /// There are two calling conventions for this method. See the following examples 
        /// <code>
        /// if (Lookup.IsSimilarTo(e1, e2)) { ... }
        /// </code>
        /// <code>
        /// if (e1.IsSimilarTo(e2)) { ... }
        /// </code> Please prefer the second convention. 
        /// </remarks>
        public static Similarity IsSimilarTo(this IAggregateEntity first, IAggregateEntity second) {
            var simResults = from e1 in first
                             from e2 in second
                             let result = e1.IsSimilarTo(e2)
                             group result by (bool)result into byResult
                             let count = byResult.Count()
                             orderby count descending
                             select new { byResult.Key, Count = count };
            return new Similarity(
                simResults.FirstOrDefault()?.Key ?? false,
                simResults
                    .Select(result => result.Count)
                    .Aggregate(0d, (last, count) => last / count)
                );
        }

        /// <summary> Determines if the provided Noun is similar to the provided NounPhrase. </summary>
        /// <param name="first"> The Noun. </param>
        /// <param name="second"> The NounPhrase. </param>
        /// <returns> <c>true</c> if the provided Noun is similar to the provided NounPhrase; otherwise, <c>false</c>. </returns>
        /// <remarks>
        /// There are two calling conventions for this method. See the following examples: <example> 
        /// <code>
        /// if (Lookup.IsSimilarTo(n1, np2)) { ... }
        /// </code>
        /// <code>
        /// if (n1.IsSimilarTo(np2)) { ... }
        /// </code> </example> Please prefer the second convention. 
        /// </remarks>
        public static Similarity IsSimilarTo(this Noun first, NounPhrase second) {
            var phraseNouns = second.Words.OfNoun().ToList();
            return new Similarity(phraseNouns.Count == 1 && phraseNouns.First().IsSynonymFor(first));
        }

        /// <summary> Determines if the provided NounPhrase is similar to the provided Noun. </summary>
        /// <param name="first"> The NounPhrase. </param>
        /// <param name="second"> The Noun. </param>
        /// <returns> <c>true</c> if the provided NounPhrase is similar to the provided Noun; otherwise, <c>false</c>. </returns>
        /// <remarks>
        /// There are two calling conventions for this method. See the following examples: <example> 
        /// <code>
        /// if (Lookup.IsSimilarTo(np1, n2)) { ... }
        /// </code>
        /// <code>
        /// if (np1.IsSimilarTo(n2)) { ... }
        /// </code> </example> Please prefer the second convention. 
        /// </remarks>
        public static Similarity IsSimilarTo(this NounPhrase first, Noun second) {
            return second.IsSimilarTo(first);
        }

        /// <summary> Determines if two NounPhrases are similar. </summary>
        /// <param name="first"> The first NounPhrase </param>
        /// <param name="second"> The second NounPhrase </param>
        /// <returns> <c>true</c> if the given NounPhrases are similar; otherwise, <c>false</c>. </returns>
        /// <remarks>
        /// There are two calling conventions for this method. See the following examples: <example> 
        /// <code>
        /// if (Lookup.IsSimilarTo(np1, np2)) { ... }
        /// </code>
        /// <code>
        /// if (np1.IsSimilarTo(np2)) { ... }
        /// </code> Please prefer the second convention. </example> 
        /// </remarks>
        public static Similarity IsSimilarTo(this NounPhrase first, NounPhrase second) {
            var ratio = GetSimilarityRatio(first, second);
            return new Similarity(ratio > SIMILARITY_THRESHOLD, ratio);
        }

        /// <summary> Determines if the two provided Noun instances are similar. </summary>
        /// <param name="first"> The first Noun. </param>
        /// <param name="second"> The second Noun. </param>
        /// <returns> <c>true</c> if the first Noun is similar to the second; otherwise, <c>false</c>. </returns>
        /// <remarks>
        /// There are two calling conventions for this method. See the following examples: <example> 
        /// <code>
        /// if (Lookup.IsSimilarTo(n1, n2)) { ... }
        /// </code>
        /// <code>
        /// if (n1.IsSimilarTo(n2)) { ... }
        /// </code> </example> Please prefer the second convention. 
        /// </remarks>
        public static Similarity IsSimilarTo(this Noun first, Noun second) {
            return new Similarity(first.IsSynonymFor(second));
        }

        /// <summary> Returns a double value indicating the degree of similarity between two NounPhrases. </summary>
        /// <param name="first"> The first NounPhrase </param>
        /// <param name="second"> The second NounPhrase </param>
        /// <returns> A double value indicating the degree of similarity between two NounPhrases. </returns>
        private static double GetSimilarityRatio(NounPhrase first, NounPhrase second) {
            var left = first.Words.OfNoun().ToList();
            if (left.Count == 0) {
                return 0;
            }
            var right = second.Words.OfNoun().ToList();
            if (right.Count == 0) {
                return 0;
            }
            return (from outer in right.Count > left.Count ? left : right
                    from inner in right.Count > left.Count ? right : left
                    select outer.IsSynonymFor(inner) ? 0.7 : 0)
                    .Average();
        }
    }
}