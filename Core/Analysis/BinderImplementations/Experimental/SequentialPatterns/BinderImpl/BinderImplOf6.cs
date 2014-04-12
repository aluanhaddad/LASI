﻿#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LASI.Core.Analysis.BinderImplementations.Experimental.SequentialPatterns.Test
{
    // Alias types to shorten name and thus file size.
    using E = IEntity;
    using V = IVerbal;
    using C = IConjunctive;
    using D = IDescriptor;
    using R = IReferencer;
    using A = IAdverbial;
    using P = IPrepositional;
    using S = SymbolPhrase;
    public partial class BinderComponent :
        IBinderComponent<E, V, C, D, R, A, P>,
        IBinderComponent<P, E, V, C, D, R, A>,
        IBinderComponent<A, P, E, V, C, D, R>,
        IBinderComponent<R, A, P, E, V, C, D>,
        IBinderComponent<D, R, A, P, E, V, C>,
        IBinderComponent<R, D, C, A, P, E, V>,
        IBinderComponent<V, C, D, R, A, P, E>,
        IBinderComponent<E, R, D, R, A, P, E>,
        IBinderComponent<E, C, R, E, A, V, E>,
        IBinderComponent<E, C, E, V, C, V, A>,
        IBinderComponent<E, E, C, E, C, E, V>,
        IBinderComponent<S, V, C, D, R, A, P>,
        IBinderComponent<P, S, V, C, D, R, A>,
        IBinderComponent<A, P, S, V, C, D, R>,
        IBinderComponent<R, A, P, S, V, C, D>,
        IBinderComponent<D, R, A, P, S, V, C>,
        IBinderComponent<R, D, C, A, S, E, V>,
        IBinderComponent<V, C, D, R, A, S, E>,
        IBinderComponent<E, R, D, R, A, P, S>,
        IBinderComponent<S, C, R, E, A, V, E>,
        IBinderComponent<E, S, E, V, C, V, A>,
        IBinderComponent<E, S, C, E, C, E, V>,
        System.Collections.IEnumerable
    {
        public BinderComponent Add(Func<A, Func<D, Func<A, Func<C, Func<A, Action<R>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<A, Func<D, Func<A, Func<C, Func<V, Action<R>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<A, Func<E, Func<A, Func<P, Func<A, Action<V>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<A, Func<E, Func<A, Func<P, Func<C, Action<D>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<A, Func<P, Func<E, Func<V, Func<C, Action<D>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<C, Func<D, Func<R, Func<A, Func<P, Action<E>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<C, Func<E, Func<C, Func<P, Func<A, Action<V>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<C, Func<E, Func<C, Func<P, Func<C, Action<V>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<C, Func<R, Func<C, Func<D, Func<C, Action<A>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<C, Func<R, Func<C, Func<D, Func<P, Action<E>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<D, Func<A, Func<D, Func<R, Func<D, Action<P>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<D, Func<A, Func<D, Func<R, Func<E, Action<V>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<D, Func<R, Func<A, Func<P, Func<E, Action<V>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<D, Func<V, Func<D, Func<E, Func<D, Action<C>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<D, Func<V, Func<D, Func<E, Func<P, Action<C>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<E, Func<A, Func<E, Func<R, Func<D, Action<P>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<E, Func<A, Func<E, Func<R, Func<E, Action<P>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<E, Func<C, Func<E, Func<V, Func<E, Action<D>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<E, Func<C, Func<E, Func<V, Func<R, Action<A>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<E, Func<V, Func<C, Func<D, Func<R, Action<A>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<P, Func<C, Func<P, Func<D, Func<P, Action<A>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<P, Func<C, Func<P, Func<D, Func<R, Action<A>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<P, Func<E, Func<V, Func<C, Func<D, Action<R>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<P, Func<R, Func<P, Func<D, Func<C, Action<A>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<P, Func<R, Func<P, Func<D, Func<P, Action<A>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<P, Func<V, Func<P, Func<E, Func<D, Action<R>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<P, Func<V, Func<P, Func<E, Func<P, Action<C>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<R, Func<A, Func<P, Func<E, Func<V, Action<C>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<R, Func<C, Func<R, Func<D, Func<P, Action<E>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<R, Func<C, Func<R, Func<D, Func<R, Action<A>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<R, Func<C, Func<R, Func<V, Func<E, Action<D>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<R, Func<C, Func<R, Func<V, Func<R, Action<D>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<R, Func<D, Func<C, Func<A, Func<P, Action<E>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<R, Func<P, Func<R, Func<A, Func<R, Action<E>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<R, Func<P, Func<R, Func<A, Func<V, Action<C>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<V, Func<C, Func<D, Func<R, Func<A, Action<P>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<V, Func<D, Func<V, Func<C, Func<A, Action<P>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<V, Func<D, Func<V, Func<C, Func<V, Action<R>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<V, Func<P, Func<V, Func<A, Func<R, Action<E>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<V, Func<P, Func<V, Func<A, Func<V, Action<E>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<E, Func<R, Func<D, Func<R, Func<A, Action<P>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<E, Func<D, Func<E, Func<R, Func<A, Action<P>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<E, Func<D, Func<E, Func<R, Func<E, Action<R>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<A, Func<D, Func<A, Func<R, Func<A, Action<R>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<A, Func<D, Func<A, Func<R, Func<E, Action<R>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<E, Func<C, Func<R, Func<E, Func<A, Action<V>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<E, Func<R, Func<E, Func<C, Func<A, Action<V>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<E, Func<R, Func<E, Func<C, Func<E, Action<E>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<A, Func<R, Func<A, Func<C, Func<A, Action<E>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<A, Func<R, Func<A, Func<C, Func<E, Action<E>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }

        public BinderComponent Add(Func<E, Func<D, Func<R, Action<P>>>> p) {
            throw new NotImplementedException();
        }

        public BinderComponent Add(Func<A, Func<S, Func<P, Action<D>>>> p) {
            throw new NotImplementedException();
        }

        public BinderComponent Add(Func<P, Func<R, Action<V>>> p) {
            throw new NotImplementedException();
        }

        public BinderComponent Add(Func<E, Func<R, Func<C, Action<V>>>> p) {
            throw new NotImplementedException();
        }

        public BinderComponent Add(Func<E, Func<C, Func<E, Func<V, Func<C, Action<V>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<E, Func<E, Func<E, Func<C, Func<C, Action<V>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<E, Func<E, Func<E, Func<C, Func<E, Action<V>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<C, Func<E, Func<C, Func<C, Func<C, Action<V>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<C, Func<E, Func<C, Func<C, Func<E, Action<V>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<E, Func<E, Func<C, Func<E, Func<C, Action<E>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<E, Func<C, Func<E, Func<E, Func<C, Action<E>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<E, Func<C, Func<E, Func<E, Func<E, Action<E>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<C, Func<C, Func<C, Func<E, Func<C, Action<E>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<C, Func<C, Func<C, Func<E, Func<E, Action<E>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<S, Func<V, Func<C, Func<D, Func<R, Action<A>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<S, Func<C, Func<S, Func<V, Func<R, Action<A>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<S, Func<C, Func<S, Func<V, Func<S, Action<D>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<R, Func<C, Func<R, Func<V, Func<S, Action<D>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<P, Func<S, Func<V, Func<C, Func<D, Action<R>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<P, Func<V, Func<P, Func<S, Func<D, Action<R>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<P, Func<V, Func<P, Func<S, Func<P, Action<C>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<D, Func<V, Func<D, Func<S, Func<D, Action<C>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<D, Func<V, Func<D, Func<S, Func<P, Action<C>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<A, Func<P, Func<S, Func<V, Func<C, Action<D>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<A, Func<S, Func<A, Func<P, Func<C, Action<D>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<A, Func<S, Func<A, Func<P, Func<A, Action<V>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<C, Func<S, Func<C, Func<P, Func<C, Action<V>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<C, Func<S, Func<C, Func<P, Func<A, Action<V>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<R, Func<A, Func<P, Func<S, Func<V, Action<C>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<R, Func<P, Func<R, Func<A, Func<R, Action<S>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<V, Func<P, Func<V, Func<A, Func<V, Action<S>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<V, Func<P, Func<V, Func<A, Func<R, Action<S>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<D, Func<R, Func<A, Func<P, Func<S, Action<V>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<D, Func<A, Func<D, Func<R, Func<S, Action<V>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<S, Func<A, Func<S, Func<R, Func<S, Action<P>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<S, Func<A, Func<S, Func<R, Func<D, Action<P>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<R, Func<D, Func<C, Func<A, Func<S, Action<E>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<R, Func<C, Func<R, Func<D, Func<S, Action<E>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<S, Func<C, Func<S, Func<D, Func<S, Action<A>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<S, Func<C, Func<S, Func<D, Func<R, Action<A>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<V, Func<C, Func<D, Func<R, Func<A, Action<S>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<V, Func<D, Func<V, Func<C, Func<A, Action<S>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<S, Func<C, Func<R, Func<E, Func<A, Action<V>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<S, Func<R, Func<S, Func<C, Func<A, Action<V>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<S, Func<R, Func<S, Func<C, Func<S, Action<E>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<A, Func<R, Func<A, Func<C, Func<S, Action<E>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<E, Func<S, Func<E, Func<V, Func<C, Action<V>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<E, Func<E, Func<E, Func<S, Func<C, Action<V>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<E, Func<E, Func<E, Func<S, Func<E, Action<V>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<C, Func<E, Func<C, Func<S, Func<C, Action<V>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<C, Func<E, Func<C, Func<S, Func<E, Action<V>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<E, Func<S, Func<C, Func<E, Func<C, Action<E>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<E, Func<C, Func<E, Func<S, Func<C, Action<E>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<E, Func<C, Func<E, Func<S, Func<E, Action<E>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<C, Func<C, Func<C, Func<S, Func<C, Action<E>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
        public BinderComponent Add(Func<C, Func<C, Func<C, Func<S, Func<E, Action<E>>>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(6)), 6); }
    }
}
#pragma warning restore 1591