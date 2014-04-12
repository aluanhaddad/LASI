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
        public BinderComponent Add(Func<A, Func<A, Func<C, Action<A>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<C, Func<V, Action<A>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<A, Func<A, Func<C, Action<P>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<A, Func<A, Func<C, Action<R>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<A, Func<A, Func<P, Action<A>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<A, Func<A, Func<P, Action<R>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<A, Func<A, Func<R, Action<A>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<A, Func<A, Func<V, Action<A>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<A, Func<E, Func<V, Action<A>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<A, Func<P, Func<A, Action<A>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<A, Func<P, Func<A, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<A, Func<P, Func<A, Action<V>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<A, Func<P, Func<C, Action<P>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<A, Func<P, Func<E, Action<A>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<A, Func<P, Func<E, Action<V>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<A, Func<V, Func<C, Action<P>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<C, Func<A, Func<P, Action<D>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<C, Func<C, Func<A, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<C, Func<C, Func<D, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<C, Func<C, Func<D, Action<V>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<C, Func<C, Func<P, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<C, Func<C, Func<P, Action<D>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<C, Func<C, Func<P, Action<V>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<C, Func<C, Func<V, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<C, Func<D, Func<C, Action<A>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<C, Func<D, Func<C, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<C, Func<D, Func<C, Action<R>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<C, Func<D, Func<P, Action<D>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<C, Func<D, Func<R, Action<A>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<C, Func<D, Func<R, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<C, Func<R, Func<A, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<D, Func<A, Func<P, Action<D>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<D, Func<D, Func<C, Action<D>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<D, Func<D, Func<C, Action<R>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<D, Func<D, Func<E, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<D, Func<D, Func<E, Action<D>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<D, Func<D, Func<E, Action<R>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<D, Func<D, Func<P, Action<D>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<D, Func<D, Func<R, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<D, Func<D, Func<R, Action<D>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<D, Func<P, Func<E, Action<R>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<D, Func<R, Func<A, Action<D>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<D, Func<R, Func<A, Action<P>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<D, Func<R, Func<D, Action<A>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<D, Func<R, Func<D, Action<D>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<D, Func<R, Func<D, Action<P>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<D, Func<R, Func<E, Action<R>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<C, Func<D, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<D, Func<R, Action<V>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<E, Func<D, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<E, Func<P, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<E, Func<R, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<E, Func<R, Action<P>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<E, Func<R, Action<V>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<E, Func<V, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<E, Func<V, Action<P>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<V, Func<C, Action<D>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<V, Func<C, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<V, Func<E, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<V, Func<E, Action<D>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<V, Func<E, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<V, Func<R, Action<V>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<P, Func<C, Func<D, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<P, Func<E, Func<D, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<P, Func<E, Func<P, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<P, Func<E, Func<P, Action<P>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<P, Func<E, Func<P, Action<V>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<P, Func<E, Func<V, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<P, Func<E, Func<V, Action<P>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<P, Func<P, Func<A, Action<P>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<P, Func<P, Func<C, Action<P>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<P, Func<P, Func<D, Action<A>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<P, Func<P, Func<D, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<P, Func<P, Func<D, Action<P>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<P, Func<P, Func<E, Action<A>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<P, Func<P, Func<E, Action<P>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<P, Func<V, Func<C, Action<P>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<A, Func<P, Action<D>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<A, Func<P, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<A, Func<P, Action<R>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<A, Func<R, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<A, Func<R, Action<P>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<A, Func<R, Action<R>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<A, Func<V, Action<A>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<D, Func<C, Action<A>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<D, Func<C, Action<R>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<D, Func<P, Action<D>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<D, Func<R, Action<A>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<D, Func<R, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<D, Func<R, Action<R>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<E, Func<V, Action<A>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<P, Func<E, Action<R>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<R, Func<A, Action<D>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<R, Func<A, Action<R>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<R, Func<D, Action<R>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<R, Func<E, Action<R>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<R, Func<P, Action<D>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<R, Func<P, Action<R>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<R, Func<P, Action<V>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<R, Func<V, Action<A>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<R, Func<V, Action<D>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<R, Func<V, Action<R>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<V, Func<C, Func<A, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<V, Func<C, Func<D, Action<R>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<V, Func<C, Func<D, Action<V>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<V, Func<C, Func<V, Action<D>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<V, Func<C, Func<V, Action<R>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<V, Func<C, Func<V, Action<V>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<V, Func<D, Func<R, Action<V>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }

        public BinderComponent Add(Func<V, Func<V, Action<E>>> p) {
            throw new NotImplementedException();
        }

        public BinderComponent Add(Func<E, Func<E, Func<S, Action<V>>>> p) {
            throw new NotImplementedException();
        }

        public BinderComponent Add(Func<P, Func<V, Func<E, Action<R>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }

        public BinderComponent Add(Func<V, Func<R, Func<A, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<V, Func<V, Func<A, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<V, Func<V, Func<A, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<V, Func<V, Func<A, Action<V>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<V, Func<V, Func<C, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<V, Func<V, Func<C, Action<V>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<V, Func<V, Func<E, Action<V>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<V, Func<V, Func<R, Action<V>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<R, Func<D, Action<R>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<E, Func<A, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<R, Func<E, Action<D>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<R, Func<D, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<R, Func<E, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<R, Func<E, Action<R>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<R, Func<D, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<R, Func<A, Action<R>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<E, Func<A, Action<R>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<C, Func<R, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<C, Func<E, Action<R>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<C, Func<E, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<E, Func<C, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<C, Func<C, Func<R, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<C, Func<A, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<E, Func<A, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<E, Func<E, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<C, Func<E, Func<A, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<A, Func<A, Func<E, Action<A>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<C, Func<E, Action<V>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<C, Func<C, Func<E, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<C, Func<C, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<E, Func<C, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<E, Func<C, Action<A>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<C, Func<C, Func<C, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<V, Func<C, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<C, Func<V, Func<C, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<E, Func<E, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<E, Func<C, Action<V>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<C, Func<C, Func<E, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<S, Func<V, Func<C, Action<D>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<S, Func<S, Func<R, Action<S>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<S, Func<V, Func<S, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<S, Func<V, Func<C, Action<S>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<S, Func<V, Func<S, Action<S>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<S, Func<S, Func<V, Action<S>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<S, Func<V, Func<S, Action<D>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<V, Func<V, Func<C, Action<S>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<S, Func<V, Func<R, Action<V>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<S, Func<S, Func<R, Action<V>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<S, Func<S, Func<R, Action<P>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<S, Func<D, Func<R, Action<V>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<S, Func<S, Func<D, Action<S>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<P, Func<S, Func<V, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<P, Func<S, Func<P, Action<V>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<P, Func<S, Func<V, Action<P>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<P, Func<S, Func<P, Action<P>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<P, Func<P, Func<S, Action<P>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<P, Func<S, Func<P, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<S, Func<S, Func<V, Action<P>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<P, Func<S, Func<D, Action<S>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<P, Func<P, Func<D, Action<S>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<P, Func<C, Func<D, Action<S>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<S, Func<C, Func<D, Action<S>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<A, Func<P, Func<S, Action<V>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<A, Func<P, Func<A, Action<S>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<A, Func<P, Func<S, Action<A>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<P, Func<P, Func<S, Action<A>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<A, Func<P, Action<S>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<A, Func<R, Action<S>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<S, Func<V, Action<A>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<R, Func<S, Action<R>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<A, Func<S, Func<V, Action<A>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<V, Func<V, Func<S, Action<V>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<D, Func<D, Func<S, Action<D>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<D, Func<R, Func<S, Action<R>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<D, Func<D, Func<S, Action<R>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<D, Func<D, Func<S, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<D, Func<P, Func<S, Action<R>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<P, Func<S, Action<R>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<S, Func<S, Func<P, Action<S>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<D, Func<S, Action<D>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<R, Func<S, Action<D>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<R, Func<S, Action<V>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<R, Func<A, Func<S, Action<D>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<D, Func<A, Func<S, Action<D>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<S, Func<S, Func<A, Action<S>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<E, Func<A, Action<S>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<S, Func<C, Func<R, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<S, Func<C, Func<S, Action<R>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<S, Func<C, Func<R, Action<S>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<S, Func<C, Func<S, Action<S>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<S, Func<S, Func<C, Action<S>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<S, Func<C, Func<S, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<C, Func<C, Func<R, Action<S>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<S, Func<C, Func<A, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<S, Func<S, Func<A, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<S, Func<S, Func<A, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<S, Func<E, Func<A, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<S, Func<S, Func<E, Action<S>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<S, Func<E, Action<V>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<S, Func<E, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<E, Func<S, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<S, Func<S, Func<E, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<S, Func<C, Action<S>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<E, Func<C, Action<S>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<V, Func<C, Action<S>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<S, Func<C, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<E, Func<S, Func<E, Action<C>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<S, Func<S, Func<C, Action<E>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
        public BinderComponent Add(Func<S, Func<E, Func<C, Action<S>>>> p) { return Update(p.TryApply(Vals.FindAll(CheckRules).Take(4)), 4); }
    }
}
#pragma warning restore 1591