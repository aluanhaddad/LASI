﻿using LASI.Core.Binding;
using LASI.Core.Binding.Experimental;
using LASI.Core.DocumentStructures;
using LASI.Core.Patternization;
using LASI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LASI.Core
{
    /// <summary>
    /// Provides static acess to a comprehensive set of binding operations which are applicable to a document.
    /// </summary>
    public static class Binder
    {
        /// <summary>
        /// Gets an ordered collection of ProcessingTask objects which correspond to the steps required to Bind the given document.
        /// Each ProcessingTask contains a Task property which, when awaited will perform a step of the Binding process.
        /// </summary>
        /// <param name="document">The document for which to get the ProcessingTasks for Binding.</param>
        /// <returns>An ordered collection of ProcessingTask objects which correspond to the steps required to Bind the given document.
        /// </returns>
        /// <remarks>
        /// ProcessingTasks returned by this method may be run in an arbitrary order.
        /// However, to ensure the consistency/determinism of the Binding process, it is recommended that they be executed (awaited) in the order
        /// in which they are hereby returned.
        /// </remarks>
        public static IEnumerable<ProcessingTask> GetBindingTasks(this Document document) {

            yield return new ProcessingTask(() => BindAttributives(document.Sentences),
                    string.Format("{0}: Binding Attributives", document.Name),
                    string.Format("{0}: Bound Attributives", document.Name), 5);
            yield return new ProcessingTask(() => BindIntraPhrase(document.Phrases),
                    string.Format("{0}: Decomposing Phrasals", document.Name),
                    string.Format("{0}: Decomposed Phrasals", document.Name), 5);
            yield return new ProcessingTask(() => BindSubjectsAndObjects(document.Sentences),
                    string.Format("{0}: Analyzing Verbal Relationships", document.Name),
                    string.Format("{0}: Analyzed Verbal Relationships", document.Name), 5);
            yield return new ProcessingTask(() => BindPronouns(document.Sentences),
                    string.Format("{0}: Abstracting References", document.Name),
                    string.Format("{0}: Abstracted References", document.Name), 5);
        }






        #region Private Static Methods


        private static void BindAdjectivePhrases(IEnumerable<Sentence> sentences) {
            sentences.AsParallel()
                .WithDegreeOfParallelism(Concurrency.Max)
                .ForAll(sentence => AdjectivePhraseBinder.Bind(sentence));
        }

        private static void BindAttributives(IEnumerable<Sentence> sentences) {
            sentences.AsParallel()
                .WithDegreeOfParallelism(Concurrency.Max)
                .ForAll(sentence => AttributivePhraseBinder.Bind(sentence));
        }

        private static void BindSubjectsAndObjects(IEnumerable<Sentence> sentences) {
            try {
                sentences.AsParallel()
                    .WithDegreeOfParallelism(Concurrency.Max)
                    .ForAll(sentence => {
                        try { new SubjectBinder().Bind(sentence); }
                        catch (Exception x) {
                            if (x is NullReferenceException ||
                                x is VerblessPhrasalSequenceException) { x.LogIfDebug(); } else { throw; }
                        }
                        try { new ObjectBinder().Bind(sentence); }
                        catch (Exception x) {
                            if (x is InvalidStateTransitionException ||
                                x is VerblessPhrasalSequenceException ||
                                x is InvalidOperationException) {
                                x.LogIfDebug();
                            } else { throw; }
                        }
                    });
            }
            catch (Exception x) { x.LogIfDebug(); }
        }



        private static void BindIntraPhrase(IEnumerable<Phrase> phrases) {
            phrases.AsParallel().WithDegreeOfParallelism(Concurrency.Max)
                .OfNounPhrase().ForAll(np => IntraPhraseWordBinder.Bind(np));
            phrases.AsParallel().WithDegreeOfParallelism(Concurrency.Max)
                .OfVerbPhrase().ForAll(vp => IntraPhraseWordBinder.Bind(vp));
        }

        private static void BindPronouns(IEnumerable<Sentence> sentences) {
            sentences.AsParallel().WithDegreeOfParallelism(Concurrency.Max)
                .ForAll(sentence => PronounBinder.Bind(sentence));
            sentences.AsParallel().WithDegreeOfParallelism(Concurrency.Max)
                .ForAll(sentence => ClauseSeperatingBranchingBinder.Bind(sentence.Words));

        }

        #endregion


    }

}