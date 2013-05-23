﻿
namespace LASI.Algorithm.FundamentalSyntacticInterfaces
{
    /// <summary>
    /// Defines the role reqirements for Action Objects, generally the objects of Verbs or VerbPhrases.
    /// Along with the rhs interfaces in the Syntactic Interfaces Library, the IActionObject interface provides for generalization and abstraction over Word and Phrase types.
    /// </summary>
    public interface IVerbialObject
    {
        ITransitiveVerbal DirectObjectOf {
            get;
            set;
        }
        ITransitiveVerbal IndirectObjectOf {
            get;
            set;
        }
    }
}