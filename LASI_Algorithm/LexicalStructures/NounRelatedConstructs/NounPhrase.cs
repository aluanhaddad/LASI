﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace LASI.Algorithm
{
    /// <summary>
    /// Represents a noun phrase such as "The Pinko-Commy Conspiracy".
    /// Note that noun componentPhrases are the constructs which wrap both nouns and pronouns at the phrase level.
    /// </summary>
    public class NounPhrase : Phrase, IEntity
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the NounPhrase class.
        /// </summary>
        /// <param name="composed">The words which compose to form the NounPhrase.</param>
        public NounPhrase(IEnumerable<Word> composed)
            : base(composed) {
            determineEntityType();
        }
        #endregion


        #region Methods

        /// <summary>
        /// Current,  somewhat sloppy determination of the Noun, person, place, thing etc, of nounphrase by 
        /// selecting the most common Noun between its nouns and from its bound pronouns 
        /// </summary>
        protected void determineEntityType() {

            var kindsOfNouns = from N in Words.OfType<IEntity>()
                               group N by N.EntityKind into KindGroup
                               orderby KindGroup.Count()
                               select KindGroup.Key;
            /*
             * I'm not sure why this is causing my program to crash.
             * But when I comment it out my program works.
             * - Scott
             */

            EntityKind = kindsOfNouns.FirstOrDefault();
        }


        /// <summary>
        /// Binds an IPronoun, generally a Pronoun or PronounPhrase, as a reference to the NounPhrase.
        /// </summary>
        /// <param name="pro">The referencer which refers to the NounPhrase Instance.</param>
        public virtual void BindPronoun(LASI.Algorithm.IPronoun pro) {
            boundPronouns.Add(pro);
            pro.BindAsReferringTo(this);
        }
        /// <summary>
        /// Binds an IDescriptor, generally an Adjective or AdjectivePhrase, as a descriptor of the NounPhrase.
        /// </summary>
        /// <param name="adjective">The IDescriptor instance which will be added to the NounPhrase' descriptors.</param>
        public void BindDescriptor(IDescriptor adjective) {
            describedBy.Add(adjective);
            adjective.Describes = this;
        }
        /// <summary>
        /// Adds an IPossessible construct, such as a person place or thing, to the collection of the NounPhrase "Owns",
        /// and sets its owner to be the NounPhrase.
        /// If the item is already possessed by the current instance, this method has no effect.
        /// </summary>
        /// <param name="possession">The possession to add.</param>
        public void AddPossession(IEntity possession) {
            possessed.Add(possession);
            possession.Possesser = this;
        }
        /// <summary>
        /// Returns a string representation of the NounPhrase.
        /// </summary>
        /// <returns>A string representation of the NounPhrase.</returns>
        public override string ToString() {
            var result = base.ToString();
            if (Phrase.VerboseOutput && Possessed.Any()) {
                result += "\n\tpossessions:\n";
                foreach (var s in Possessed) {
                    result += s + "\n";
                }
            }
            if (Phrase.VerboseOutput) {
                if (Possesser != null)
                    result += "\n\towned By:\n\t\t" + Possesser.ToString();

                if (InnerAttributive != null) {
                    result += "\n\tDefines:\n\t\t" + InnerAttributive;
                }
                if (OuterAttributive != null) {
                    result += "\n\tDefines:\n\t\t" + OuterAttributive;
                }
            }
            return result;

        }

        #endregion

        /// <summary>
        /// Gets or sets the IVerbal instance, generally a Verb or VerbPhrase, which the NounPhrase is the subject of.
        /// </summary>
        public virtual IVerbal SubjectOf {
            get {
                return subjectOf;
            }
            set {
                subjectOf = value;
                foreach (var N in Words.OfType<IEntity>())
                    N.SubjectOf = subjectOf;
            }
        }
        /// <summary>
        /// Gets the or sets IVerbal instance, generally a TransitiveVerb or TransitiveVerbPhrase, which the NounPhrase is the DIRECT object of.
        /// </summary>
        public virtual IVerbal DirectObjectOf {
            get {
                return direcObjectOf;
            }
            set {
                direcObjectOf = value;
                foreach (var N in Words.OfType<IEntity>())
                    N.DirectObjectOf = direcObjectOf;
            }
        }

        /// <summary>
        /// Gets or sets the IVerbal instance, generally a TransitiveVerb or TransitiveVerbPhrase, which the NounPhrase is the INDIRECT object of.
        /// </summary>
        public virtual IVerbal IndirectObjectOf {
            get {
                return indirecObjectOf;
            }
            set {
                indirecObjectOf = value;
                foreach (var N in Words.OfType<IEntity>())
                    N.IndirectObjectOf = IndirectObjectOf;
            }
        }



        /// <summary>
        /// Gets all of the IPronoun instances, generally Pronouns or PronounPhrases, which refer to the NounPhrase.
        /// </summary>
        public IEnumerable<IPronoun> BoundPronouns {
            get {
                return boundPronouns;
            }
            protected set {
                boundPronouns = new HashSet<IPronoun>(value);
            }
        }

        /// <summary>
        /// Gets all of the IDescriptor constructs,generally Adjectives or AdjectivePhrases, which describe the NounPhrase.
        /// </summary>
        public IEnumerable<IDescriptor> Descriptors {
            get {
                return describedBy;
            }
            protected set {
                describedBy = new HashSet<IDescriptor>(value);
            }
        }


        /// <summary>
        /// Gets all of the constructs which the NounPhrase "owns".
        /// </summary>
        public IEnumerable<IEntity> Possessed {
            get {
                return possessed;
            }
            protected set {
                possessed = new HashSet<IEntity>(value);
            }
        }
        /// <summary>
        /// Gets or sets the Entity which "owns" the NounPhrase.
        /// </summary>
        public IEntity Possesser {
            get {
                return possessor;
            }
            set {
                possessor = value;
                if (value != null) {
                    foreach (var item in this.Words.OfType<IEntity>()) {
                        value.AddPossession(item);
                    }
                }
            }
        }
        /// <summary>
        /// Gets or sets another NounPhrase, to the left of current instance, which is functions as an Attributor of current instance.
        /// </summary>
        public NounPhrase OuterAttributive { get; set; }
        /// <summary>
        /// Gets or sets another NounPhrase, to the right of current instance, which is functions as an Attributor of current instance.
        /// </summary>
        public NounPhrase InnerAttributive { get; set; }
        /// <summary>
        /// Gets or sets the Entity PronounKind; Person, Place, Thing, Organization, or Activity; of the NounPhrase.
        /// </summary>
        public EntityKind EntityKind { get; protected set; }



        private HashSet<IDescriptor> describedBy = new HashSet<IDescriptor>();
        private HashSet<IEntity> possessed = new HashSet<IEntity>();
        private HashSet<IPronoun> boundPronouns = new HashSet<IPronoun>();
        private IEntity possessor;
        private IVerbal direcObjectOf;
        private IVerbal indirecObjectOf;
        private IVerbal subjectOf;




    }
}


///// <summary>
///// Gets or sets Noun to Nounphrase
///// </summary>
//public Noun BoundNoun {
//    get;
//    set;
//}

///// <summary>
///// Gets or sets NounPhrase to NounPhrase
///// </summary>
//public NounPhrase BoundNounPhrase {
//    get;
//    set;
//}

//public bool WasBound {
//    get;
//    set;
//}