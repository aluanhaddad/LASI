﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using LASI.Core;
using LASI.WebApp;
using Newtonsoft.Json.Linq;

namespace LASI.WebApp.ViewModels
{
    public class PhraseViewModel : LexicalElementViewModel
    {
        public PhraseViewModel(Phrase phrase) : base(phrase) {
            ContextMenuJson = phrase.GetJsonMenuData();
            DetailText = phrase.ToString().SplitRemoveEmpty('\n', '\r').Format(Tuple.Create(' ', ' ', ' '), s => s + "\n");
            WordViewModels = phrase.Words.Select(word => new WordViewModel(word));
        }
        public string ContextMenuJson { get; private set; }
        public string DetailText { get; private set; }
        public IEnumerable<WordViewModel> WordViewModels { get; private set; }
    }
}