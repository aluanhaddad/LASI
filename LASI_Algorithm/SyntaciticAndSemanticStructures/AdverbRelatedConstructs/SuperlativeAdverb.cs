﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LASI.Algorithm
{
    /// <summary>
    /// Represents an adverb which 'absolutely' specifies a verb or adjective relative to all comparable occurances of that verb or adjective.
    /// For Verbs - 
    /// If modifying an instransitive verb it will usually lexically follow the the verb: e.g. "Jane performed BEST.
    /// If modifying a transitive verb it will usually lexically follow the verb object: e.g. "Jane played poker BEST.
    /// For Adjectives - 
    /// The adverb will usually lexically precede the adjective it modifies: e.g. John'd wardrobe is MOST colorful than Jane'd.
    /// </summary>
    public class SuperlativeAdverb : Adverb
    {
        /// <summary>
        /// Initializes a new instance of the SuperlativeAdverb class.
        /// </summary>
        /// <param name="text">The literal text content of the SuperlativeAdverb.</param>
        public SuperlativeAdverb(string text)
            : base(text) {

        }
    }
}