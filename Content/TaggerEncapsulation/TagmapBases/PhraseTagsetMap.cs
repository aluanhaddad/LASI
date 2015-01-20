﻿using LASI;
using LASI.Core;
using System.Collections.Generic;
using System.Linq;

namespace LASI.Content.TaggerEncapsulation
{
    using PhraseCreator = System.Func<IEnumerable<LASI.Core.Word>, LASI.Core.Phrase>;

    /// <summary>
    /// Represents a tagset-to-runtime-type-mapping context for Phrase constructs which translates between a Part Of Speech
    /// Tagger's tagset and the classes whose instances provide the runtime representations of the Phrase tag.
    /// This class represents the tagset => runtime-type mapping for word occurances.
    /// </summary>
    /// <example>
    ///<code>
    /// var phraseMap = new PhraseTagSetMap();
    /// var createPhrase = phraseMap["TAG"];
    /// var phrase = createPhrase(phraseWords);
    /// </code>
    /// </example>
    /// <seealso cref="WordTagsetMap"/>
    public abstract class PhraseTagsetMap
    {
        /// <summary>
        /// When overriden in a derrived class, Provides POS-Tag indexed access to a constructor function which can be invoked to create an
        /// instance of the Phrase class which provides its run-time representation.
        /// </summary>
        /// <param name="tag">The textual representation of a Phrase Part Of Speech tag.</param>
        /// <returns>A function which creates an instance of the run-time Phrase type associated with the textual tag.</returns>
        /// <exception cref="UnknownPhraseTagException">Thrown when the indexing tag string is not defined by the tagset.</exception>
        public abstract PhraseCreator this[string tag] { get; }

        /// <summary>
        /// When overriden in a derrived class, Gets the PosTag string corresponding to the System.Type of the given LASI.Algorithm.Phrase.
        /// </summary>
        /// <param name="phrase">The LASI.Algorithm.Phrase for which to get the corresponding tag.</param>
        /// <returns>The PosTag string corresponding to the System.Type of the given <see cref="LASI.Core.Phrase"/>.</returns>
        public abstract string this[Phrase phrase] { get; }
    }
}