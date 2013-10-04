﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LASI.Utilities;
using LASI.Algorithm.DocumentConstructs;


namespace LASI.Algorithm.Binding
{
    public class ObjectBinder
    {
        /// <summary>
        /// Initializes a new instances of ObjectBinder class.
        /// </summary>
        public ObjectBinder()
        {

            St0 = new State0(this);
            St1 = new State1(this);
            St2 = new State2(this);
            St3 = new State3(this);
            St4 = new State4(this);
            St5 = new State5(this);
            St6 = new State6(this);

        }


        public void Bind(Sentence sentence)
        {
            this.Bind(sentence.Phrases);
        }
        public void Bind(IEnumerable<Phrase> contiguousPhrases)
        {
            if (contiguousPhrases.GetVerbPhrases().Count() == 0)
                throw new VerblessPhrasalSequenceException();
            var phrases = contiguousPhrases.ToList();
            var verbPhraseIndex = phrases.FindIndex(r => r is VerbPhrase);
            bindingTarget = contiguousPhrases.ElementAt(verbPhraseIndex) as VerbPhrase;
            var remainingPhrases = phrases.Skip(verbPhraseIndex + 1).Reverse();
            if (remainingPhrases.Any()) {
                inputstream.PushAll(remainingPhrases);
                try {
                    St0.Transition(inputstream.PopDynamic());
                }
                catch (InvalidOperationException) {
                }
            }
        }
        private void AssociateDirect()
        {
            foreach (var e in entities) {
                bindingTarget.BindDirectObject(e);
            }
            entities.Clear();
            ConjunctNounPhrases.Clear();

        }

        private void AssociateIndirect()
        {
            foreach (var e in entities) {
                bindingTarget.BindIndirectObject(e);
            }
            entities.Clear();
            ConjunctNounPhrases.Clear();

        }
        private void BindBuiltupAdjectivePhrases(NounPhrase phrase)
        {
            foreach (var adjp in this.lastAdjectivals) {
                phrase.BindDescriber(adjp);
            }
            this.lastAdjectivals.Clear();
        }


        public Phrase LastPhrase
        {
            get;
            set;
        }
        #region Fields
        protected Stack<Phrase> inputstream = new Stack<Phrase>();
        protected VerbPhrase bindingTarget;
        protected IVerbalObject directObject;
        protected IVerbalObject indirectObject;
        protected bool directFound = false;

        protected List<AdjectivePhrase> lastAdjectivals = new List<AdjectivePhrase>();
        protected List<NounPhrase> ConjunctNounPhrases = new List<NounPhrase>();
        protected List<AdjectivePhrase> ConjunctAdjectivePhrases = new List<AdjectivePhrase>();
        protected Stack<NounPhrase> entities = new Stack<NounPhrase>();
        private State0 St0;
        private State1 St1;
        private State2 St2;
        private State3 St3;
        private State4 St4;
        private State5 St5;
        private State6 St6;
        private ConjunctionPhrase lastConjunctive;
        private PrepositionalPhrase lastPrepositional;
        public bool indirectFound;

        #endregion




        abstract class State
        {
            protected State(ObjectBinder machine)
            {
                _machine = machine;
                _stream = machine.inputstream;
            }

            public virtual void Transition(Phrase phrase)
            {
                throw new InvalidStateTransitionException(StateName, phrase);
            }

            protected void Universal(Phrase Phrase)
            {
                Machine.LastPhrase = Phrase;
            }

            protected string StateName
            {
                get
                {
                    return stateName;
                }
                set
                {
                    stateName = value;
                }
            }


            public ObjectBinder Machine
            {
                get
                {
                    return _machine;
                }
                set
                {
                    _machine = value;
                }
            }


            protected Stack<Phrase> Stream
            {
                get
                {
                    return _stream;
                }
                set
                {
                    _stream = value;
                }
            }



            private string stateName;
            private Stack<Phrase> _stream;
            private ObjectBinder _machine;


        }



        class State0 : State
        {
            public State0(ObjectBinder machine)
                : base(machine)
            {
                StateName = "s0";
            }
            public void Transition(PrepositionalPhrase phrase)
            {
                Machine.lastPrepositional = phrase;
                try {
                    if (Machine.inputstream.Count > 1)
                        Machine.St0.Transition(Stream.PopDynamic());
                }
                catch (InvalidOperationException) {
                    Machine.AssociateDirect();
                    Machine.AssociateIndirect();
                }
            }
            public void Transition(VerbPhrase phrase)
            {
                new ObjectBinder().Bind(phrase.AsEnumerable().Concat(Stream.ToList()));
            }
            public void Transition(SubordinateClauseBeginPhrase phrase)
            {
                var subordinateClauseConstituents = new List<Phrase> {
                    phrase};
                for (var r = Stream.Count > 0 ? Stream.Pop() : null; !(r.Words.First() is Punctuation) && Stream.Count > 0; r = Stream.Pop()) {
                    subordinateClauseConstituents.Add(r);
                }
                var subClause = new ClauseTypes.SubordinateClause(subordinateClauseConstituents);
                Machine.bindingTarget.ModifyWith(subClause);
                new ObjectBinder().Bind(subordinateClauseConstituents);
            }

            public void Transition(AdverbPhrase phrase)
            {
                Machine.bindingTarget.ModifyWith(phrase);
                Universal(phrase);
                if (Stream.Count < 1)
                    return;
                try {
                    Machine.St0.Transition(Stream.PopDynamic());
                }
                catch (InvalidOperationException) {
                    Machine.AssociateDirect();
                    Machine.AssociateIndirect();
                }

            }


            public void Transition(ConjunctionPhrase phrase)
            {
                if (Machine.lastPrepositional != null) {
                    phrase.PrepositionOnLeft = Machine.lastPrepositional;
                    Machine.lastPrepositional.OnRightSide = phrase;
                    Machine.bindingTarget.AttachObjectViaPreposition(phrase.PrepositionOnLeft);
                }
                //Machine(parent);
                if (Machine.inputstream.Count < 1) {
                    if (!Machine.directFound)
                        Machine.AssociateDirect();
                    else
                        Machine.AssociateIndirect();

                    return;
                }
                Universal(phrase);

                try {
                    Machine.St2.Transition(Stream.PopDynamic());
                }
                catch (InvalidOperationException) {
                    Machine.AssociateDirect();
                    Machine.AssociateIndirect();
                }

            }


            public void Transition(NounPhrase phrase)
            {
                if (Machine.lastPrepositional != null) {
                    phrase.PrepositionOnLeft = Machine.lastPrepositional;
                    Machine.lastPrepositional.OnRightSide = phrase;
                    Machine.bindingTarget.AttachObjectViaPreposition(phrase.PrepositionOnLeft);
                }
                Machine.entities.Push(phrase);
                if (Machine.inputstream.Count < 1) {
                    if (!Machine.directFound)
                        Machine.AssociateDirect();
                    else
                        Machine.AssociateIndirect();

                    return;
                }
                Universal(phrase);

                try {
                    Machine.St2.Transition(Stream.PopDynamic());
                }
                catch (InvalidOperationException) {
                    Machine.AssociateDirect();
                    Machine.AssociateIndirect();
                }
            }
            public void Transition(AdjectivePhrase phrase)
            {
                Machine.lastAdjectivals.Add(phrase);
                Universal(phrase);
                if (Machine.inputstream.Count > 0) {
                    try {
                        Machine.St1.Transition(Stream.PopDynamic());
                    }
                    catch (InvalidOperationException) {
                        Machine.AssociateDirect();
                        Machine.AssociateIndirect();
                    }
                }
            }
        }


        class State1 : State
        {
            public State1(ObjectBinder machine)
                : base(machine)
            {
                StateName = "s1";
            }

            public void Transition(VerbPhrase phrase)
            {
                phrase.AdjectivalModifier = Machine.lastAdjectivals.Last();
                Machine.lastAdjectivals.Clear();
                try {
                    Machine.St1.Transition(Stream.PopDynamic());
                }
                catch (InvalidOperationException) {
                    Machine.AssociateDirect();
                    Machine.AssociateIndirect();
                }
            }
            public void Transition(NounPhrase phrase)
            {
                Machine.entities.Push(phrase);
                Machine.BindBuiltupAdjectivePhrases(phrase);
                if (Machine.inputstream.Count < 1) {
                    if (!Machine.directFound)
                        Machine.AssociateDirect();
                    else
                        Machine.AssociateIndirect();

                    return;
                }
                Universal(phrase);

                try {
                    Machine.St2.Transition(Stream.PopDynamic());
                }
                catch (InvalidOperationException) {
                    Machine.AssociateDirect();
                    Machine.AssociateIndirect();
                }
            }
            public void Transition(ConjunctionPhrase phrase)
            {
                Machine.lastConjunctive = phrase;
                Universal(phrase);
            }
            public void Transition(PrepositionalPhrase phrase)
            {
                Machine.lastPrepositional = phrase;

                Universal(phrase);
                try {
                    Machine.St0.Transition(Stream.PopDynamic());
                }
                catch (InvalidOperationException) {
                    Machine.AssociateDirect();
                    Machine.AssociateIndirect();
                }
            }
        }
        class State3 : State
        {
            public State3(ObjectBinder machine)
                : base(machine)
            {
                StateName = "s3";
            }

        }
        class State2 : State
        {


            public State2(ObjectBinder machine)
                : base(machine)
            {
                StateName = "s2";

            }
            public void Transition(ConjunctionPhrase phrase)
            {
                phrase.OnLeft = Machine.entities.Peek();
                Machine.lastConjunctive = phrase;
                Machine.ConjunctNounPhrases.Add(Machine.entities.Peek());
                if (Machine.inputstream.Count < 1) {
                    if (!Machine.directFound)
                        Machine.AssociateDirect();
                    else
                        Machine.AssociateIndirect();

                    return;
                }
                Universal(phrase);


                try {
                    Machine.St4.Transition(Stream.PopDynamic());
                }
                catch (InvalidOperationException) {
                    Machine.AssociateDirect();
                    Machine.AssociateIndirect();
                }

            }
            public void Transition(AdjectivePhrase phrase)
            {
                Machine.AssociateIndirect();
                Universal(phrase);

                try {
                    Machine.St4.Transition(Stream.PopDynamic());
                }
                catch (InvalidOperationException) {
                    Machine.AssociateDirect();
                    Machine.AssociateIndirect();
                }

            }
            public void Transition(VerbPhrase phrase)
            {
                InfinitivePhrase infinitive = new InfinitivePhrase(
                    phrase.AsEnumerable().GetWords().Concat(
                   phrase.Sentence.GetPhrasesAfter(phrase)
                    .TakeWhile(w => !(w is IConjunctive || w is IPrepositional)).GetWords()));
                Machine.directObject = infinitive;

            }
            public void Transition(PrepositionalPhrase phrase)
            {
                foreach (var e in Machine.entities)
                    Machine.bindingTarget.BindDirectObject(e);
                try {
                    Machine.lastPrepositional = phrase;

                    Machine.entities.Last().PrepositionOnRight = Machine.lastPrepositional;
                }
                catch (InvalidOperationException) {
                }
                phrase.OnLeftSide = Machine.entities.Last();
                Machine.entities.Clear();
                Machine.directFound = true;
                Machine.ConjunctNounPhrases.Clear();
                Universal(phrase);

                try {
                    Machine.St0.Transition(Stream.PopDynamic());
                }
                catch (InvalidOperationException) {
                    Machine.AssociateDirect();
                    Machine.AssociateIndirect();
                }
            }
            public void Transition(NounPhrase phrase)
            {
                foreach (var e in Machine.entities)
                    Machine.bindingTarget.BindIndirectObject(e);
                Machine.entities.Clear();
                Machine.indirectFound = true;
                Machine.ConjunctNounPhrases.Clear();

                Machine.entities.Push(phrase);

                if (Stream.Count < 1) {

                    Machine.AssociateDirect();
                    return;
                }
                Universal(phrase);

                try {
                    Machine.St0.Transition(Stream.PopDynamic());
                }
                catch (InvalidOperationException) {
                    Machine.AssociateDirect();
                    Machine.AssociateIndirect();
                }
            }
            public void Transition(SubordinateClauseBeginPhrase phrase)
            {
                Machine.AssociateDirect();
                Machine.AssociateIndirect();
                Universal(phrase);

            }
            public virtual void Transition(AdverbPhrase phrase)
            {
                Machine.bindingTarget.ModifyWith(phrase);
                Universal(phrase);
                foreach (var e in Machine.entities) if (!Machine.directFound)
                        Machine.bindingTarget.BindDirectObject(e);
                    else
                        Machine.bindingTarget.BindIndirectObject(e);
                if (Stream.Count < 1)
                    return;
                try {
                    Machine.St0.Transition(Stream.PopDynamic());
                }
                catch (InvalidOperationException) {
                    Machine.AssociateDirect();
                    Machine.AssociateIndirect();
                }

            }

        }
        class State4 : State
        {


            public State4(ObjectBinder machine)
                : base(machine)
            {

                StateName = "s4";
            }
            public void Transition(NounPhrase phrase)
            {
                Machine.entities.Push(phrase);
                Machine.ConjunctNounPhrases.Add(phrase);
                if (Machine.lastConjunctive != null) {
                    Machine.lastConjunctive.OnRight = phrase;
                }
                if (Machine.inputstream.Count < 1) {
                    if (!Machine.directFound)
                        Machine.AssociateDirect();
                    else
                        Machine.AssociateIndirect();

                    return;
                }
                Universal(phrase);

                try {
                    Machine.St2.Transition(Stream.PopDynamic());
                }
                catch (InvalidOperationException) {
                    Machine.AssociateDirect();
                    Machine.AssociateIndirect();
                }
            }
            public void Transition(AdjectivePhrase phrase)
            {
                Machine.lastAdjectivals.Add(phrase);
                if (Machine.inputstream.Count < 1) {
                    if (!Machine.directFound)
                        Machine.AssociateDirect();
                    else
                        Machine.AssociateIndirect();

                    return;
                }
                Universal(phrase);

                try {
                    Machine.St5.Transition(Stream.PopDynamic());
                }
                catch (InvalidOperationException) {
                    Machine.AssociateDirect();
                    Machine.AssociateIndirect();
                }
            }
        }
        class State5 : State
        {
            public State5(ObjectBinder machine)
                : base(machine)
            {
                StateName = "s5";
            }

            public void Transition(NounPhrase phrase)
            {
                Machine.ConjunctNounPhrases.Add(phrase);
                Machine.entities.Push(phrase);
                Machine.BindBuiltupAdjectivePhrases(phrase);
                if (Machine.inputstream.Count < 1) {
                    if (!Machine.directFound)
                        Machine.AssociateDirect();
                    else
                        Machine.AssociateIndirect();

                    return;
                }
                Universal(phrase);

                try {
                    Machine.St2.Transition(Stream.PopDynamic());
                }
                catch (InvalidOperationException) {
                    Machine.AssociateDirect();
                    Machine.AssociateIndirect();
                }
            }
            public void Transition(ConjunctionPhrase phrase)
            {
                phrase.OnLeft = Machine.lastAdjectivals.Last();
                Machine.lastConjunctive = phrase;
                if (Machine.inputstream.Count < 1) {
                    if (!Machine.directFound)
                        Machine.AssociateDirect();
                    else
                        Machine.AssociateIndirect();

                    return;
                }
                Universal(phrase);

                try {
                    Machine.St6.Transition(Stream.PopDynamic());
                }
                catch (InvalidOperationException) {
                    Machine.AssociateDirect();
                    Machine.AssociateIndirect();
                }
            }

        }
        class State6 : State
        {


            public State6(ObjectBinder machine)
                : base(machine)
            {
                StateName = "s6";
            }
            public void Transition(AdjectivePhrase phrase)
            {
                Machine.lastAdjectivals.Add(phrase);
                Machine.lastConjunctive.OnRight = phrase;
                if (Machine.inputstream.Count < 1) {
                    if (!Machine.directFound)
                        Machine.AssociateDirect();
                    else
                        Machine.AssociateIndirect();

                    return;
                }
                Universal(phrase);

                try {
                    Machine.St5.Transition(Stream.PopDynamic());
                }
                catch (InvalidOperationException) {
                    Machine.AssociateDirect();
                    Machine.AssociateIndirect();
                }
            }
            public void Transition(NounPhrase phrase)
            {
                Machine.entities.Push(phrase);
                Machine.ConjunctNounPhrases.Add(phrase);
                Machine.lastConjunctive.OnRight = phrase;
                Machine.BindBuiltupAdjectivePhrases(phrase);
                if (Machine.inputstream.Count < 1) {
                    if (!Machine.directFound)
                        Machine.AssociateDirect();
                    else
                        Machine.AssociateIndirect();

                    return;
                }
                Universal(phrase);

                try {
                    Machine.St2.Transition(Stream.PopDynamic());
                }
                catch (InvalidOperationException) {
                    Machine.AssociateDirect();
                    Machine.AssociateIndirect();
                }
            }


        }


    }

    static class DynamicStackExtensions
    {
        /// <summary>
        /// An extension method which pops the next item from the stack, but returns it as a an object of Type dynamic.
        /// This allows the overloaded methods present in each state to be correctly selected based on the run time type of the parent.
        /// </summary>
        /// <param name="stack">The Stack instances from which to pop.</param>
        /// <returns>The Phrase at the top of the stack typed as dynamic.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown if the Stack is empty.</exception>
        internal static dynamic PopDynamic(this Stack<Phrase> stack)
        {
            return stack.Pop();
        }
        internal static void PushAll(this Stack<Phrase> stack, IEnumerable<Phrase> items)
        {
            foreach (var i in items)
                stack.Push(i);
        }
    }

}