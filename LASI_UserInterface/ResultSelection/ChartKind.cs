﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LASI.UserInterface
{
    /// <summary>
    /// Represents the various kinds of datasets that a Chart can display.
    /// </summary>
    public enum ChartKind
    {
        /// <summary>
        /// A chart of a data set's Subject Verb relationships.
        /// </summary>
        SubjectVerb,
        /// <summary>
        /// A chart of a data set's Subject Verb Object relationships.
        /// </summary>
        SubjectVerbObject,
        /// <summary>
        /// A chart of a data set's Significant NounPhrases.
        /// </summary>
        NounPhrasesOnly,
    }
}
