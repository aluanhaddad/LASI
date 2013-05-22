﻿using LASI.Algorithm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace LASI.FileSystem
{
    /// <summary>
    /// Parses pairs of text and tag tokens into their runtime type equivalents as specified by the Tagset
    /// </summary>
    public class WordMapper
    {
        /// <summary>
        /// Initializes an instance of the TaggedWordParser class using the default Tagset (SharpNLP).
        /// <see cref="SharpNLPWordTagsetMap"/>
        /// </summary>
        public WordMapper() {
            context = new SharpNLPWordTagsetMap();
        }
        /// <summary>
        /// Initialized an instance of the TaggedWordParser class using the Tagset provided defined by the TaggingContext argument.
        /// </summary>
        /// <param name="taggingContext">The tagset-to-runtime-type mapping which will define how new verb instances will be instantiated.</param>
        public WordMapper(WordTagsetMap taggingContext) {
            context = taggingContext;
        }

        /// <summary>
        /// Creates entity new Instance of the Word class which corresponds to the given text token and Part Of Speech tag.
        /// </summary>
        /// <param name="tag">entity verb or punctuation string and its associated  Part Of Speech tag.</param>
        /// <returns>entity new instance of the appropriate verb type corresponding with the tag and containing the given text.</returns>
        public Word CreateWord(TaggedWordObject taggedText) {
            if (string.IsNullOrWhiteSpace(taggedText.Text))
                return null;
            return LookupMapping(taggedText)(taggedText.Text);
        }
        public Func<Word> ReadWordExpression(TaggedWordObject taggedText) {
            if (string.IsNullOrWhiteSpace(taggedText.Text))
                return null;
            var Constructor = LookupMapping(taggedText);
            return () => Constructor(taggedText.Text);
        }

        private Func<string, Word> LookupMapping(TaggedWordObject taggedText) {
            var tag = taggedText.Tag.Trim();
            var text = taggedText.Text.Trim();
            if (tag.Length < 2)
                return
                    (text == "." || text == "!" || text == "?") ?
                    new Func<string, Word>((s) => new LASI.Algorithm.SentenceDelimiter(s.First(c => !Char.IsWhiteSpace(c)))) :
                    new Func<string, Word>((s) => new LASI.Algorithm.Punctuation(s.First(c => !Char.IsWhiteSpace(c))));
            try {

                var constructor = context[tag];
                return constructor;
            }
            catch (UnknownPOSException) {
                return (s) => new LASI.Algorithm.GenericSingularNoun(taggedText.Text);
                throw new UnknownPOSException(String.Format("Unable to parse unknown tag\nTag: {0}\nFor text: {1}\n", tag, taggedText.Text));

            }
        }

        private bool checkThesaurusForGeneric(string text) {
            return LASI.Algorithm.Thesauri.Thesaurus.NounProvider[text.ToLower()].Any();
        }
        private WordTagsetMap context;
    }
}
