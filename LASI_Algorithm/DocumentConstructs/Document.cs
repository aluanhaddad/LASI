﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LASI.Algorithm
{
    /// <summary>
    /// a line structure containing all of he paragraph, sentence, entity, and w objects in a document.
    /// Provides overalapping direct and indirect access to all of its children, 
    /// e.g. such as myDoc.Paragraphs.Sentences.Phrases.Words will get all the words in the document in linear order
    /// comparatively: myDoc.Words; yields the same collection.
    /// </summary>
    public sealed class Document
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Document class.
        /// </summary>
        /// <param name="paragrpahs">The collection of paragraphs which contain all text in the document.</param>
        public Document(IEnumerable<Paragraph> paragrpahs) {
            _paragraphs = paragrpahs.ToList();
            AssignMembers(paragrpahs);
            foreach (var p in _paragraphs) {
                p.EstablishParent(this);
            }
            EstablishLexicalLinks();
        }

        private void AssignMembers(IEnumerable<Paragraph> paragrpahs) {

            _sentences = (from p in _paragraphs
                          from s in p.Sentences
                          select s).ToList();
            _phrases = (from s in _sentences
                        from r in s.Phrases
                        select r).ToList();
            _words = (from s in _sentences
                      from w in s.Words
                      select w).ToList();

        }

        #endregion

        #region Methods

        private void EstablishLexicalLinks() {
            if (_words.Count > 1) {
                for (int i = 1; i < _words.Count(); ++i) {
                    _words[i].PreviousWord = _words[i - 1];
                    _words[i - 1].NextWord = _words[i];
                }

                var lastWord = _words[_words.Count - 1];
                if (_words.IndexOf(lastWord) > 0)
                    lastWord.PreviousWord = _words[_words.Count - 1];
                else
                    lastWord.PreviousWord = null;
                lastWord.NextWord = null;
            }
            if (_phrases.Count() > 1) {

                for (var i = 1; i < _phrases.Count; ++i) {
                    _phrases[i].PreviousPhrase = _phrases[i - 1];
                    _phrases[i - 1].NextPhrase = _phrases[i];
                }
            }

        }

        /// Returns the w instance at x location in the document 
        public Word WordAt(int loc) {
            if (loc < this._words.Count)
                return this._words.ElementAt(loc);
            else
                throw new ArgumentOutOfRangeException("Document.WordAt");
        }

        /// Returns the text  of w instance at x location in the document
        public string WordTextAt(int loc) {
            if (loc < _words.Count)
                return _words.ElementAt(loc).Text;
            else
                throw new ArgumentOutOfRangeException("Document.WordTextAt");
        }

        /// Returns the sentence instance at x location 
        public Sentence SentenceAt(int loc) {

            if (loc < this.Sentences.Count())
                return this.Sentences.ElementAt(loc);
            else
                throw new ArgumentOutOfRangeException("Document.SentenceAt");
        }

        ///  Returns the sentence instance text at x location
        public string SentenceTextAt(int loc) {
            if (loc < this.Sentences.Count())
                return this.Sentences.ElementAt(loc).Text;
            else
                throw new ArgumentOutOfRangeException("Document.SentenceTextAt");
        }

        ///// <summary>
        ///// Prints out the entire contents of the document, from left to right, by using the using the lexical links of each of its words.
        ///// </summary>
        //public void PrintByWordLinkage() {
        //    for (var w = _words.First(); w != null; w = w.NextWord)
        //        Console.Write(w.Text + " ");

        //}
        ///// <summary>
        ///// Prints out the entire contents of the document, from left to right, by using the using the lexical links of each of its phrases.
        ///// </summary>
        //public void PrintByPhraseLinkage() {

        //    for (var r = _phrases.First(); r != null; r = r.NextPhrase)
        //        Console.Write(r.Text + " ");
        //}

        /// <summary>
        /// Returns all of the Action identified within the docimument.
        /// </summary>
        /// <returns>all of the Action identified within the docimument.</returns>
        public IEnumerable<ITransitiveVerbial> GetActions() {
            return from a in _words.GetVerbs().Concat<ITransitiveVerbial>(_phrases.GetVerbPhrases())
                   orderby a is Word ? (a as Word).ID : (a as Phrase).Words.Last().ID ascending
                   select a;
        }

        /// <summary>
        /// Returns all of the word and entity level entities identified in the document.
        /// </summary>
        /// <returns> All of the word and entity level entities identified in the document.</returns>
        public IEnumerable<IEntity> GetEntities() {
            return from e in _words.GetNouns().Concat<IEntity>(_words.GetPronouns()).Concat<IEntity>(_phrases.GetNounPhrases())
                   orderby e is Word ? (e as Word).ID : (e as Phrase).Words.Last().ID ascending
                   select e;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Sentences the document contains in linear, left to right order.
        /// </summary>
        public IEnumerable<Sentence> Sentences {
            get {
                return _sentences;
            }

        }

        /// <summary>
        /// Gets the Paragraphs the document contains in linear, left to right order.
        /// </summary>
        public IEnumerable<Paragraph> Paragraphs {
            get {
                return _paragraphs;
            }
        }

        /// <summary>
        /// Gets the Phrases the document contains in linear, left to right order.
        /// </summary>
        public IEnumerable<Phrase> Phrases {
            get {
                return _phrases;
            }
        }
        /// <summary>
        /// Gets the Words the document contains in linear, left to right order.
        /// </summary>
        public IEnumerable<Word> Words {
            get {
                return _words;
            }
        }

        #endregion

        #region Fields

        private IList<Word> _words;
        private IList<Phrase> _phrases;
        private IList<Sentence> _sentences;
        private IList<Paragraph> _paragraphs;

        #endregion

        //public static implicit operator List<ILexical>(Document d) {
        //    return (from e in d.Words.Concat<ILexical>(d.Phrases)
        //            orderby e is Word ? (e as Word).ID : (e as Phrase).Words.Last().ID ascending
        //            select e).ToList();
        //}
    }
}