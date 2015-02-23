﻿using LASI.Core;
using System.Linq;
using System.Windows.Media;

namespace LASI.App.Visualization
{
    class SyntacticColorMap : LASI.Interop.Visualization.IStyleProvider<ILexical, Brush>
    {
        /// <summary>
        /// Maps a Lexical element to a syntax highlighting color. The returned value is a System.Windows.Media.Brush enumeration member.
        /// </summary>
        /// <param name="element">The Lexical for which to get a color based on its syntactic role.</param>
        /// <returns>A System.Windows.Media.Brush enumeration value mapped to the syntactic role of the element.</returns>
        public Brush this[ILexical element] =>
            element.Match()
                .Case((Phrase p) => p.Match()
                    .Case((PronounPhrase e) => Brushes.HotPink)
                    .When((NounPhrase n) => n.Words.OfProperNoun().Any())
                    .Then(Brushes.DarkBlue)
                    .Case((NounPhrase e) => Brushes.MediumTurquoise)
                    .Case((InfinitivePhrase e) => Brushes.Teal)
                    .Case((IReferencer e) => Brushes.DarkCyan)
                    .Case((IEntity e) => Brushes.DeepSkyBlue)
                    .Case((IVerbal e) => Brushes.Green)
                    .Case((IPrepositional e) => Brushes.DarkOrange)
                    .Case((IDescriptor e) => Brushes.Indigo)
                    .Case((IAdverbial e) => Brushes.Orange)
                    .Result(Brushes.Black))
                .Case((Word w) => w.Match()
                    .Case((Adjective e) => Brushes.Indigo)
                    .Case((PresentParticiple e) => Brushes.DarkGreen)
                    .Case((Verb e) => Brushes.Green)
                    .Result(Brushes.Black))
                .Result();
    }
}