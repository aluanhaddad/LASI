﻿using LASI.Algorithm.CoreDataStructures.LexicalStructures.MiscLanguageConstructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LASI.Algorithm
{
    /// <summary>
    /// Represents a single double quote within a passage of text.
    /// </summary>
    public class DoubleQuote : QuotationMark<DoubleQuote>
    {
        /// <summary>
        /// Initializes a new instance of the DoubleQuote class.
        /// </summary>
        public DoubleQuote() : base('"') { }
        /// <summary>
        /// Pairs one DoubleQuote with another DoubleQuote, establishing a reflexive link between the two.
        /// </summary>
        /// <param name="complement">A matching DoubleQuote with which to pair.</param>
        public override void PairWith(DoubleQuote complement)
        {
            PairedInstance = complement;
            complement.PairedInstance = this;
        }

    }
}
