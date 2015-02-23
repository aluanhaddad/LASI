﻿using LASI.Core;

namespace AspSixApp.Models.Lexical
{
    class WordModel : LexicalModel<Word>
    {
        public WordModel(Word word) : base(word) { }

        public PhraseModel PhraseModel { get; set; }
        public override string ContextMenuJson { get; }
    }
}