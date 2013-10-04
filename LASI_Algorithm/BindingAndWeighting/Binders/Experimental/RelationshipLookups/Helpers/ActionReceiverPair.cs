﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LASI.Algorithm.RelationshipLookups
{
    /// <summary>
    /// Stores the relationship between a Verbal construct used transitively (having at least one direct or indirect object) 
    /// and an Entity construct which receives the effect of said Verbal construct (i.e. its object).
    /// </summary>
    /// <typeparam name="TVerbal">The Type of the Verbal construct in the relationship. The stated or inferred Type must implement the IVerbal interface.</typeparam>
    /// <typeparam name="TEntity">The Type of the Entity construct in the relationship. The stated or inferred Type must implement the IEntity interface.</typeparam>
    /// <remarks>Any instance of the ActionReceiverPair struct is immutable unless passed as a 'ref' or 'out' argument to a function.</remarks>
    public struct ActionReceiverPair<TVerbal, TEntity>
        where TVerbal : IVerbal
        where TEntity : IEntity
    {
        /// <summary>
        /// Initializes a new instance of the ActionReceiverPair structure based on the provided action and receiver.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="receiver">The receiver of the action.</param>
        public ActionReceiverPair(TVerbal action, TEntity receiver)
            : this() {
            Action = action;
            Receiver = receiver;
        }
        /// <summary>
        /// Gets the Action.
        /// </summary>
        public TVerbal Action {
            get;
            private set;
        }
        /// <summary>
        /// Gets the Receiver of the Action.
        /// </summary>
        public TEntity Receiver {
            get;
            private set;
        }
    }
}