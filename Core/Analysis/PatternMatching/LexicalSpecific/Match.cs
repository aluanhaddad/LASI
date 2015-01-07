﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LASI.Core.PatternMatching
{
    /// <summary>
    /// Provides for the representation and free form structuring of Type based Pattern Matching
    /// expressions which match with a value of Type T and does not yield a result.
    /// </summary>
    /// <typeparam name="T">
    /// The Type of the value which the the Pattern Matching expression will match with. 
    /// </typeparam>
    /// <remarks>
    /// <para>
    /// It is important to note that null values are coalesced by the Match process. That is to say
    /// that if the value being tested is null it will never match any of the Clauses. Because of
    /// this a null value will always yield the default result and never produce an error.
    /// </para>
    /// <para>
    /// The type based pattern matching functionality was introduced to solve the problem of
    /// performing subtype dependent operations which could not be described by traditional virtual
    /// method approaches in an expressive or practical manner. There are several reasons for this.
    /// First and most important, is that the virtual method approach is limited to single dispatch*
    /// . Secondly different binding operations must be chosen based on a variety of information
    /// gathered from contexts which are often external to the single instance of the type
    /// representing a lexical construct. Different numbers and types of arguments that depend on
    /// the surrounding context of the lexical instnace cannot be specified in a manner compatible
    /// with virtual method signatures. However, a linear storage and traversal mechanism for
    /// lexical elements of differing syntactic types, as well as the need to define types
    /// representing elements having overlapping syntactic roles, remains a necessity which prevents
    /// the complete stratification of class and interface hierarchies. There were a variety of
    /// solutions that were considered:
    /// </para>
    /// <para>
    /// 1. A visitor pattern could be implemented such that the an object traversing a sequence of
    ///    lexical elements would be passed in turn to each, and by carrying with it sufficient and
    ///    arbitrary context and by providing an overload for every syntactic type, could provide
    ///    functionality to implement operations between elements. There are several drawbacks
    ///    involved including increased state disperal, increased class coupling, the maintenance
    ///    cost of re factoring types, the need to define new classes, which carry arbitrary
    ///    algorithm state with them but must exist in their own hierarchy, and other factors
    ///    generally increasing complexity.
    /// </para>
    /// <para>
    /// 2. A more flexible, visitor-like pattern could be implemented by taking advantage of dynamic
    ///    dispatch and runtime overload resolution. This has the drawback of the code not clearly
    ///    expressing its semantics statically as it essentially relies on the runtime to describe
    ///    control flow, never stating it explicitly.
    /// </para>
    /// <para>
    /// 3. Describe traversal algorithms using complex, nested tiers of conditional blocks
    ///    determined by the results of type casts. This is error prone, unwieldy, ugly, and
    ///    obscures the logic with noise. Additionally this approach may be inconsistently applied
    ///    in different section of code, causing the implementations of algorithms so written to be
    ///    prone to subtle errors**.
    /// </para>
    /// <para>
    /// The pattern matching implemented here abstracts over the type checking logic, allowing it to
    /// be tested and optimized for the large variety of algorithms in need of such functionality,
    /// allows for subtype constraints to be specified, allows for algorithms to handle as many or
    /// as few types as needed, and emphasizes the intent of the code which leverage it. It also
    /// eliminates the difficulty of defining state-full visitors by defining a match with a
    /// particular subtype as a function on that type. Such a function is intuitively specified as
    /// an anonymous closure which implicitly captures any state it will use. This approach also
    /// encourages the localization of logic, which in the case of visitors would not possible as
    /// implementations of visitors will inherently get spread out in both the textual space of the
    /// source code and the conceptions of implementers. The syntax for pattern matching uses a
    /// fluent interface style.
    /// </para> <example> 
    /// <code>
    /// var weight = myLexical.Match()
    ///         .Case((IReferencer r) =&gt; Console.WriteLine(r.ReferredTo.Weight))
    ///     	.Case((IEntity e) =&gt; Console.WriteLine(e.Weight))
    ///     	.Case((IVerbal v) =&gt;  Console.WriteLine(v.HasSubject()? v.Subject.Weight : 0));
    /// </code> </example><example> 
    /// <code>
    /// var weight = myLexical.Match()
    /// 		.Case((Phrase p) =&gt; Console.WriteLine(p.Words.Average(w =&gt; w.Weight)))
    /// 		.Case((Word w) =&gt; Console.WriteLine(w.Weight))
    ///     .Default(()=&gt; Console.WriteLine("not a word or phrase"))
    /// </code> </example> 
    /// <para> Patterns may be nested arbitrarily as in the following example </para>
    /// <para>
    /// When a Yield clause is not applied, the Match Expression will not yield a value. Instead it
    /// will behave much as a Type driven switch statement. <example>
    /// <code>
    /// myLexical.Match()
    ///         .Case((Phrase p) =&gt; Console.WriteLine("Phrase: ", p.Text))
    /// 	    .Case((Word w) =&gt; Console.WriteLine("Word: ", w.Text))
    ///     .Default(() =&gt; Console.WriteLine("Not a Word or Phrase"));
    /// </code> </example> 
    /// </para>
    /// <para>
    /// * a: The visitor pattern provides statically type safe double dispatch, at the cost of
    ///   increased code complexity and increased class coupling.
    /// b: As LASI is implemented using C# 5.0, it has access to the language's built in support for
    ///    truly dynamic multi-methods with arbitrary numbers of arguments. However while
    /// experimenting with this approach, in a constrained scope and environment involving a fixed
    /// set of method overloads, this approach still had the drawbacks of reducing type safety,
    /// making extensions to type hierarchies potentially volatile, and drastically harming
    /// readability and maintainability.
    /// </para>
    /// <para>
    /// ** C# offers several methods of type checking, type casting, and type conversions, each with
    ///    distinct semantics and sometimes drastically different performance characteristics. This
    /// is justification enough to form a centralized API and design pattern within the context of
    /// the project.(for example: if one algorithm is implemented using as/is operator semantics,
    /// which do not consider user defined conversions, it will not naturally adjust if the such
    /// conversions are defined)
    /// </para>
    /// </remarks>
    /// <see cref="LASI.Core.MatchExtensions">
    /// Provides extension methods which allow for the creation of Match expressions. 
    /// </see>
    [DebuggerStepThrough]
    public class Match<T> : MatchBase<T> where T : class, ILexical
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Match&lt;T&gt; class which will match against the
        /// supplied value.
        /// </summary>
        /// <param name="value">
        /// The value to match against. 
        /// </param>
        [DebuggerStepThrough]
        internal Match(T value) : base(value) { }

        #endregion Constructors

        #region Expression Transformations

        /// <summary>
        /// Promotes the current non result returning expression of type Case&lt;T&gt; into a result
        /// returning expression of Case&lt;T, R&gt; Such that subsequent Case expressions appended
        /// are now to yield a result value of the supplied Type R.
        /// </summary>
        /// <typeparam name="TResult">
        /// The Type of the result which the match expression may now return. 
        /// </typeparam>
        /// <returns>
        /// A Match&lt;T, R&gt; representing the now result yielding Match expression. 
        /// </returns>
        public Match<T, TResult> Yield<TResult>() => new Match<T, TResult>(Value, Accepted);

        public Match<T, TResult> Case<TPattern, TResult>(Func<TPattern, TResult> f) where TPattern : class, ILexical => this.Yield<TResult>().Case(f);

        #endregion Expression Transformations

        #region When Expressions

        /// <summary>
        /// Appends a When expression to the current PatternMatching Expression. The When expression
        /// applies a predicate to the value being matched over. It must be followed by a Then
        /// expression which is only considered if the predicate applied here returns true.
        /// </summary>
        /// <param name="predicate">
        /// The predicate to test the value being matched over. 
        /// </param>
        /// <returns>
        /// The PredicatedMatch&lt;T&gt; describing the Match expression so far. This must be
        /// followed by a single Then expression.
        /// </returns>
        public PredicatedMatch<T> When(Func<T, bool> predicate) {
            return new PredicatedMatch<T>(predicate(Value), this);
        }

        /// <summary>
        /// Appends a When expression to the current PatternMatching Expression. The When expression
        /// applies a predicate to the value being matched over. It must be followed by a Then
        /// expression which is only considered if the predicate applied here returns true.
        /// </summary>
        /// <typeparam name="TPattern">
        /// The Type to match with. That the value being matched is of this type is also necessary
        /// for the following then expression to be selected.
        /// </typeparam>
        /// <param name="predicate">
        /// The predicate to test the value being matched over. 
        /// </param>
        /// <returns>
        /// The PredicatedMatch&lt;T&gt; describing the Match expression so far. This must be
        /// followed by a single Then expression.
        /// </returns>
        public PredicatedMatch<T> When<TPattern>(Func<TPattern, bool> predicate) where TPattern : class, ILexical {
            var typed = Value as TPattern;
            return new PredicatedMatch<T>(typed != null && predicate(typed), this);
        }

        /// <summary>
        /// Appends a When expression to the current pattern. This applies a predicate to the value
        /// being matched such that the subsequent Then expression will only be chosen if the
        /// predicate returns true.
        /// </summary>
        /// <param name="condition">
        /// The predicate to test the value being matched. 
        /// </param>
        /// <returns>
        /// The PredicatedMatch&lt;T&gt; describing the Match expression so far. This must be
        /// followed by a single Then expression
        /// </returns>
        public PredicatedMatch<T> When(bool condition) {
            return new PredicatedMatch<T>(condition, this);
        }

        #endregion When Expressions

        #region Case Expressions

        /// <summary>
        /// Appends a Match with Type expression to the current PatternMatching Expression. 
        /// </summary>
        /// <typeparam name="TPattern">
        /// The Type to match with. If the value being matched is of this type, this Case expression
        /// will be selected and the provided action invoked.
        /// </typeparam>
        /// <param name="action">
        /// The Action which, if this Case expression is Matched, will be invoked. 
        /// </param>
        /// <returns>
        /// The Match&lt;T&gt; describing the Match expression so far. 
        /// </returns>
        public Match<T> Case<TPattern>(Action action) where TPattern : class, ILexical {
            if (!Accepted && Value is TPattern) {
                Accepted = true;
                action();
            }
            return this.Case((TPattern t) => action());
        }

        /// <summary>
        /// Appends a Match with Type expression to the current PatternMatching Expression. 
        /// </summary>
        /// <typeparam name="TPattern">
        /// The Type to match with. If the value being matched is of this type, this Case expression
        /// will be selected and the provided action invoked.
        /// </typeparam>
        /// <param name="action">
        /// The Action&lt;TPattern&gt; which, if this Case expression is Matched, will be invoked on
        /// the value being matched over by the PatternMatching expression.
        /// </param>
        /// <returns>
        /// The Match&lt;T&gt; describing the Match expression so far. 
        /// </returns>
        public Match<T> Case<TPattern>(Action<TPattern> action) where TPattern : class, ILexical {
            if (Value != null) {
                if (!Accepted) {
                    var matched = Value as TPattern;
                    if (matched != null) {
                        Accepted = true;
                        action(matched);
                    }
                }
            }
            return this;
        }

        #endregion Case Expressions

        #region Default Expressions

        /// <summary>
        /// Appends the Default expression to the current Pattern Matching expression. 
        /// </summary>
        /// <param name="action">
        /// The function to invoke if no matches in the expression succeeded. 
        /// </param>
        public void Default(Action action) {
            if (!Accepted)
                action();
        }

        /// <summary>
        /// Appends the Default expression to the current Pattern Matching expression. 
        /// </summary>
        /// <param name="action">
        /// The function to invoke on the match Case value if no matches in the expression succeeded. 
        /// </param>
        public void Default(Action<T> action) {
            if (!Accepted && Value != null) {
                action(Value);
            }
        }

        #endregion Default Expressions

        #region Operators

        //public static implicit operator bool(Match<T> match) { return match.Accepted; }

        #endregion Operators
    }

    /// <summary>
    /// Provides for the representation and free form structuring of Type based Pattern Matching
    /// expressions from a match over a value of Type T to a result of Type TResult. If no Type is
    /// matched, the result will be the default value for the Type TResult.
    /// </summary>
    /// <typeparam name="T">
    /// The Type of the value which the the Pattern Matching expression will match with. 
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The Type of the result to be yielded by the Pattern Matching expression. 
    /// </typeparam>
    /// <remarks>
    /// <para>
    /// The type based pattern matching functionality was introduced to solve the problem of
    /// performing subtype dependent operations which could not be described by traditional virtual
    /// method approaches in an expressive or practical manner. There are several reasons for this.
    /// First and most important, is that the virtual method approach is limited to single dispatch*
    /// . Secondly different binding operations must be chosen based on a variety of information
    /// gathered from contexts which are often external to the single instance of the type
    /// representing a lexical construct. Different numbers and types of arguments that depend on
    /// the surrounding context of the lexical instnace cannot be specified in a manner compatible
    /// with virtual method signatures. However, a linear storage and traversal mechanism for
    /// lexical elements of differing syntactic types, as well as the need to define types
    /// representing elements having overlapping syntactic roles, remains a necessity which prevents
    /// the complete stratification of class and interface hierarchies. There were a variety of
    /// solutions that were considered:
    /// </para>
    /// <para>
    /// 1. A visitor pattern could be implemented such that the an object traversing a sequence of
    ///    lexical elements would be passed in turn to each, and by carrying with it sufficient and
    ///    arbitrary context and by providing an overload for every syntactic type, could provide
    ///    functionality to implement operations between elements. There are several drawbacks
    ///    involved including increased state disperal, increased class coupling, the maintenance
    ///    cost of re factoring types, the need to define new classes, which carry arbitrary
    ///    algorithm state with them but must exist in their own hierarchy, and other factors
    ///    generally increasing complexity.
    /// </para>
    /// <para>
    /// 2. A more flexible, visitor-like pattern could be implemented by taking advantage of dynamic
    ///    dispatch and runtime overload resolution. This has the drawback of the code not clearly
    ///    expressing its semantics statically as it essentially relies on the runtime to describe
    ///    control flow, never stating it explicitly.
    /// </para>
    /// <para>
    /// 3. Describe traversal algorithms using complex, nested tiers of conditional blocks
    ///    determined by the results of type casts. This is error prone, unwieldy, ugly, and
    ///    obscures the logic with noise. Additionally this approach may be inconsistently applied
    ///    in different section of code, causing the implementations of algorithms so written to be
    ///    prone to subtle errors**.
    /// </para>
    /// <para>
    /// The pattern matching implemented here abstracts over the type checking logic, allowing it to
    /// be tested and optimized for the large variety of algorithms in need of such functionality,
    /// allows for subtype constraints to be specified, allows for algorithms to handle as many or
    /// as few types as needed, and emphasizes the intent of the code which leverage it. It also
    /// eliminates the difficulty of defining state-full visitors by defining a match with a
    /// particular subtype as a function on that type. Such a function is intuitively specified as
    /// an anonymous closure which implicitly captures any state it will use. This approach also
    /// encourages the localization of logic, which in the case of visitors would not possible as
    /// implementations of visitors will inherently get spread out in both the textual space of the
    /// source code and the conceptions of implementers. The syntax for pattern matching uses a
    /// fluent interface style.
    /// </para> <example> 
    /// <code>
    /// var weight = myLexical.Match().Yield&lt;double&gt;()
    ///         .Case((IReferencer r) =&gt; r.ReferredTo.Weight)
    ///     	.Case((IEntity e) =&gt; e.Weight)
    ///     	.Case((IVerbal v) =&gt; v.HasSubject()? v.Subject.Weight : 0)
    /// 	.Result(1);
    /// </code> </example><example> 
    /// <code>
    /// var weight = myLexical.Match().Yield&lt;double&gt;()
    /// 		.Case((Phrase p) =&gt; p.Words.Average(w =&gt; w.Weight))
    /// 		.Case((Word w) =&gt; w.Weight)
    /// 	.Result();
    /// </code> </example> 
    /// <para> Patterns may be nested arbitrarily as in the following example </para> <example> 
    /// <code>
    /// var weight = myLexical.Match().Yield&lt;double&gt;()
    ///         .Case((IReferencer r) =&gt; r.ReferredTo
    ///             .Match().Yield&lt;double&gt;()
    ///                 .Case((Phrase p) =&gt; p.Words.OfNoun().Average(w =&gt; w.Weight))
    ///             .Result())
    ///         .Case((Noun n) =&gt; n.Weight)
    ///     .Result();
    /// </code> </example> 
    /// <para>
    /// When a Yield clause is not applied, the Match Expression will not yield a value. Instead it
    /// will behave much as a Type driven switch statement. <example>
    /// <code>
    /// myLexical.Match()
    ///         .Case((Phrase p) =&gt; Console.Write("Phrase: ", p.Text))
    /// 	    .Case((Word w) =&gt; Console.Write("Word: ", w.Text))
    ///     .Default(() =&gt; Console.Write("Not a Word or Phrase"));
    /// </code> </example> 
    /// </para>
    /// <para>
    /// * a: The visitor pattern provides statically type safe double dispatch, at the cost of
    ///   increased code complexity and increased class coupling.
    /// b: As LASI is implemented using C# 5.0, it has access to the language's built in support for
    ///    truly dynamic multi-methods with arbitrary numbers of arguments. However while
    /// experimenting with this approach, in a constrained scope and environment involving a fixed
    /// set of method overloads, this approach still had the drawbacks of reducing type safety,
    /// making extensions to type hierarchies potentially volatile, and drastically harming
    /// readability and maintainability.
    /// </para>
    /// <para>
    /// ** C# offers several methods of type checking, type casting, and type conversions, each with
    ///    distinct semantics and sometimes drastically different performance characteristics. This
    /// is justification enough to form a centralized API and design pattern within the context of
    /// the project.(for example: if one algorithm is implemented using as/is operator semantics,
    /// which do not consider user defined conversions, it will not naturally adjust if the such
    /// conversions are defined)
    /// </para>
    /// </remarks>
    [DebuggerStepThrough]
    public class Match<T, TResult> : MatchBase<T> where T : class, ILexical
    {
        #region Constructors

        /// <summary>
        /// Initailizes a new instance of the Case&lt;T,R&gt; which will allow for Pattern Matching
        /// with the provided value.
        /// </summary>
        /// <param name="value">
        /// <param name="accepted">Indicates if the match is to be initialized as already matched. </param>
        /// The value to match with. 
        /// </param>
        [DebuggerStepThrough]
        internal Match(T value, bool accepted) : base(value) {
            Accepted = accepted;
        }

        #endregion Constructors

        #region When Expressions

        /// <summary>
        /// Appends a When expression to the current pattern. This applies a predicate to the value
        /// being matched such that the subsequent Then expression will only be chosen if the
        /// predicate returns true.
        /// </summary>
        /// <param name="predicate">
        /// The predicate to test the value being matched. 
        /// </param>
        /// <returns>
        /// The PredicatedMatch&lt;T, R&gt; describing the Match expression so far. This must be
        /// followed by a single Then expression.
        /// </returns>
        public PredicatedMatch<T, TResult> When(Func<T, bool> predicate) {
            return new PredicatedMatch<T, TResult>(predicate(Value), this);
        }

        /// <summary>
        /// Appends a When expression to the current pattern. This applies a predicate to the value
        /// being matched such that the subsequent Then expression will only be chosen if the
        /// predicate returns true.
        /// </summary>
        /// <typeparam name="TPattern">
        /// The Type to match with. That the value being matched is of this type is also necessary
        /// for the following then expression to be selected.
        /// </typeparam>
        /// <param name="predicate">
        /// The predicate to test the value being matched. 
        /// </param>
        /// <returns>
        /// The PredicatedMatch&lt;T, R&gt; describing the Match expression so far. This must be
        /// followed by a single Then expression.
        /// </returns>
        public PredicatedMatch<T, TResult> When<TPattern>(Func<TPattern, bool> predicate) where TPattern : class, ILexical {
            var typed = Value as TPattern;
            return new PredicatedMatch<T, TResult>(typed != null && predicate(typed), this);
        }

        /// <summary>
        /// Appends a When expression to the current pattern. This applies a predicate to the value
        /// being matched such that the subsequent Then expression will only be chosen if the
        /// predicate returns true.
        /// </summary>
        /// <param name="condition">
        /// The predicate to test the value being matched. 
        /// </param>
        /// <returns>
        /// The PredicatedMatch&lt;T, R&gt; describing the Match expression so far. This must be
        /// followed by a single Then expression.
        /// </returns>
        public PredicatedMatch<T, TResult> When(bool condition) {
            return new PredicatedMatch<T, TResult>(condition, this);
        }

        #endregion When Expressions

        #region Case Expressions

        /// <summary>
        /// Appends a Match with Type expression to the current PatternMatching Expression. 
        /// </summary>
        /// <typeparam name="TPattern">
        /// The Type to match with. If the value being matched is of this type, this Case expression
        /// will be selected and executed.
        /// </typeparam>
        /// <param name="func">
        /// The function which, if this Case expression is Matched, will be invoked to produce the
        /// corresponding desired result for a Match Case TPattern.
        /// </param>
        /// <returns>
        /// The Match&lt;T, R&gt; describing the Match expression so far. 
        /// </returns>
        public Match<T, TResult> Case<TPattern>(Func<TResult> func) where TPattern : class, ILexical {
            if (!Accepted && Value is TPattern) {
                result = func();
                Accepted = true;
            }
            return this;
        }

        /// <summary>
        /// Appends a Match with Type expression to the current PatternMatching Expression. 
        /// </summary>
        /// <typeparam name="TPattern">
        /// The Type to match with. If the value being matched is of this type, this Case expression
        /// will be selected and executed.
        /// </typeparam>
        /// <param name="func">
        /// The function which, if this Case expression is Matched, will be invoked on the value
        /// being matched with to produce the desired result for a Match with TPattern.
        /// </param>
        /// <returns>
        /// The Match&lt;T, R&gt; describing the Match expression so far. 
        /// </returns>
        public Match<T, TResult> Case<TPattern>(Func<TPattern, TResult> func) where TPattern : class, ILexical {
            if (!Accepted) {
                var matched = Value as TPattern;
                if (matched != null) {
                    result = func(matched);
                    Accepted = true;
                }
            }
            return this;
        }

        /// <summary>
        /// Appends a Match with Type expression to the current PatternMatching Expression. 
        /// </summary>
        /// <typeparam name="TPattern">
        /// The Type to match with. If the value being matched is of this type, this Case expression
        /// will be selected and executed.
        /// </typeparam>
        /// <param name="result">
        /// The value which, if this Case expression is Matched, will be the result of the Pattern Match. 
        /// </param>
        /// <returns>
        /// The Match&lt;T, R&gt; describing the Match expression so far. 
        /// </returns>
        public Match<T, TResult> Case<TPattern>(TResult result) where TPattern : class, ILexical {
            if (!Accepted) {
                var matched = Value as TPattern;
                if (matched != null) {
                    this.result = result;
                    Accepted = true;
                }
            }
            return this;
        }

        public Match<T, TResult> Case(Func<T, TResult> func) { return this.Case<T>(func); }

        public Match<T, TResult> Case(Func<TResult> func) { return this.Case<T>(func); }

        public Match<T, TResult> Case(TResult result) { return this.Case<T>(result); }

        #endregion Case Expressions

        #region Result Expressions

        /// <summary>
        /// Returns the result of the Pattern Matching expression. This will be either the result
        /// specified for the first Match expression which succeeded or the default value the type TResult.
        /// </summary>
        /// <returns>
        /// The result of the Pattern Matching expression. 
        /// </returns>
        public TResult Result() { return result; }

        /// <summary>
        /// Appends a Result Expression to the current pattern, thus specifying the default result
        /// to yield when no other patterns have been matched.
        /// </summary>
        /// <param name="func">
        /// The factory function returning a desired default value. 
        /// </param>
        /// <returns>
        /// The result of the first successful match or the value given by invoking the supplied
        /// factory function.
        /// </returns>
        public TResult Result(Func<TResult> func) {
            if (!Accepted) {
                result = func();
                Accepted = true;
            }
            return result;
        }

        /// <summary>
        /// Appends a Result Expression to the current pattern, thus specifying the default result
        /// to yield when no other patterns have been matched.
        /// </summary>
        /// <param name="func">
        /// The factory function returning a desired default value. 
        /// </param>
        /// <returns>
        /// The result of the first successful match or the value given by invoking the supplied
        /// factory function.
        /// </returns>
        public TResult Result(Func<T, TResult> func) {
            if (Value != null && !Accepted) {
                result = func(Value);
                Accepted = true;
            }
            return result;
        }

        /// <summary>
        /// Appends a Result Expression to the current pattern, thus specifying the default result
        /// to yield when no other patterns have been matched.
        /// </summary>
        /// <param name="defaultValue">
        /// The desired default value. 
        /// </param>
        /// <returns>
        /// The result of the first successful match or supplied default value. 
        /// </returns>
        public TResult Result(TResult defaultValue) {
            if (!Accepted) {
                result = defaultValue;
                Accepted = true;
            }
            return result;
        }

        #endregion Result Expressions

        #region Fields

        private TResult result = default(TResult);

        #endregion Fields
    }
}