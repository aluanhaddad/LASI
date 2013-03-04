﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LASI.Algorithm
{
    /// <summary>
    /// Represents the present participle/gerund form of a verb. As such takes the behavior of both a verb and an entity.
    /// </summary>
    public class PresentParticipleGerund : Verb, IEntity
    {
        /// <summary>
        /// Initializes a new instance of the PresentPrtcplVerb class.
        /// </summary>
        /// <param name="text">The literal text content of the Verb.</param>
        public PresentParticipleGerund(string text)
            : base(text, VerbTense.PresentParticiple) {
            EntityKind = EntityKind.Activitiy;
        } 
        #region Methods

        /// <summary>
        /// Binds a Pronoun or PronounPhrase to refer to the gerund.
        /// </summary>
        /// <param name="pro">The Pronoun or PronounPhrase to bind to the gerund</param>
        public void BindPronoun(Pronoun pro) {
            pro.BoundEntity = this;
            _indirectReferences.Add(pro);
        }



        public void BindDescriber(IDescriber adj) {
            adj.Describes = this;
            _describedBy.Add(adj);
        }
        public void AddPossession(IEntity possession) {
            throw new NotImplementedException();
        }

        public bool Equals(IEntity other) {
            return this == other as PresentParticipleGerund;
        }
        #endregion

        #region Properties

        public IEnumerable<Pronoun> BoundPronouns {
            get {
                return _indirectReferences;
            }
        }
        /// <summary>
        /// The Verb construct which the Gerund is the subject of.
        /// </summary>
        public IAction SubjectOf {
            get;
            set;
        }
        /// <summary>
        /// The Verb construct which the gerund is the direct object of.
        /// </summary>
        public ITransitiveAction DirectObjectOf {
            get;
            set;
        }
        /// <summary>
        /// The Verb construct which the gerund is the indirect object of.
        /// </summary>
        public ITransitiveAction IndirectObjectOf {
            get;
            set;
        }
        public IEnumerable<IDescriber> DescribedBy {
            get {
                return _describedBy;
            }
        }

        public IEnumerable<IEntity> Possessed {
            get {
                return _possessed;
            }
        }
        public IEntity Possesser {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public EntityKind EntityKind {
            get;
            private set;
        }

        #endregion

        #region Fields

        private ICollection<IDescriber> _describedBy = new List<IDescriber>();
        private ICollection<IEntity> _possessed = new List<IEntity>();
        private ICollection<Pronoun> _indirectReferences = new List<Pronoun>();

        #endregion






        public EntityThemeMemberKind ThemeMemberKind {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
    }
}
