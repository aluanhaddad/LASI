﻿using System;
namespace LASI.Algorithm.Patternization
{
    /// <summary>
    /// Specifies the required behavior for Pattern Matching expression component that does not yield a value.
    /// </summary>
    /// <typeparam name="T">The Type of the value being matched over.</typeparam>
    public interface IPatternMatching<T>
       where T : class,ILexical
    {
        /// <summary>
        /// Appends a Match with Type expression to the current PatternMatching Expression.
        /// </summary>
        /// <typeparam name="TCase">The Type to match with. If the value being matched is of this type, this Case expression will be selected and the provided action invoked.</typeparam>
        /// <param name="action">The Action which, if this Case expression is Matched, will be invoked.</param>
        /// <returns>The IPatternMatching&lt;T, R&gt; describing the Match expression so far.</returns>
        IPatternMatching<T> Case<TCase>(Action action) where TCase : class, T;
        /// <summary>
        /// Appends a Match with Type expression to the current PatternMatching Expression.
        /// </summary>
        /// <typeparam name="TCase">The Type to match with. If the value being matched is of this type, this Case expression will be selected and the provided action invoked.</typeparam>
        /// <param name="action">The Action&lt;TCase&gt; which, if this Case expression is Matched, will be invoked on the value being matched over by the PatternMatching expression.</param>
        /// <returns>The IPatternMatching&lt;T, R&gt; describing the Match expression so far.</returns>
        IPatternMatching<T> Case<TCase>(Action<TCase> action) where TCase : class, T;
        /// <summary>
        /// Appends a When expression to the current PatternMatching Expression. The When expression applies a predicate to the value being matched over. 
        /// It must be followed by a Then expression which is only considered if the predicate applied here returns true.
        /// </summary>
        /// <param name="predicate">The predicate to test the value being matched over.</param>
        /// <returns>The IPredicatedPatternMatching&lt;T&gt; describing the Match expression so far. This must be followed by a single Then expression.</returns>
        IPredicatedPatternMatching<T> When(Func<T, bool> predicate);
        /// <summary>
        /// Appends a When expression to the current PatternMatching Expression. The When expression applies a predicate to the value being matched over. 
        /// It must be followed by a Then expression which is only considered if the predicate applied here returns true.
        /// </summary>
        /// <typeparam name="TCase">The Type to match with. That the value being matched is of this type is also necessary for the following then expression to be selected.</typeparam>
        /// <param name="predicate">The predicate to test the value being matched over.</param>
        /// <returns>The IPredicatedPatternMatching&lt;T&gt; describing the Match expression so far. This must be followed by a single Then expression.</returns>
        IPredicatedPatternMatching<T> When<TCase>(Func<TCase, bool> predicate) where TCase : class,ILexical;
        IPredicatedPatternMatching<T> When(bool condition);
        /// <summary>
        /// Promotes the current non result returning expression of type Case&lt;T&gt; into a result returning expression of Case&lt;T, R&gt;
        /// Such that subsequent Case expressions appended are now to yield a result value of the supplied Type R.
        /// </summary>
        /// <typeparam name="R">The Type of the result which the match expression may now return.</typeparam>
        /// <returns>A IPatternMatching&lt;T, R&gt; describing the Match expression so far.</returns> 
        IPatternMatching<T, R> Yield<R>();

        /// <summary>
        /// Appends the Default expression to the current Pattern Matching expression.
        /// </summary>
        /// <param name="action">The function to invoke if no matches in the expression succeeded.</param>
        void Default(Action action);
        /// <summary>
        /// Appends the Default expression to the current Pattern Matching expression.
        /// </summary>
        /// <param name="action">The function to invoke on the match with value if no matches in the expression succeeded.</param>
        void Default(Action<T> action);
    }
    /// <summary>
    /// Specifies the required behavior for Pattern Matching expression component that does not yield a value.
    /// </summary>
    /// <typeparam name="T">The Type of the value being matched over.</typeparam>
    /// <typeparam name="R">The Type of Results the matching expression will yield.</typeparam>
    public interface IPatternMatching<T, R>
    where T : class, LASI.Algorithm.ILexical
    {
        /// <summary>
        /// Returns the result of the Pattern Matching expression. 
        /// The result will be one of 3 possibilities: 
        /// 1. The result specified for the first Match with Type expression which succeeded. 
        /// 2. If no matches succeeded, and a Default expression was provided, the result specified in the Default expression.
        /// 3. If no matches succeeded, and a Default expression was not provided, the default value for type the Result Type of the Match Expression.
        /// </summary>
        /// <returns>Returns the result of the Pattern Matching expression.</returns>
        R Result();
        /// <summary>
        /// Appends a Result Expression to the current pattern, thus specifying the default result to yield when no other patterns have been matched.
        /// Although not enforced by the compiler, Result should only be used as the last clause in the match expression, never in between Case clauses.
        /// </summary>
        /// <param name="defaultValue">The desired default value.</param>
        /// <returns>The result corresponding to the first matched Case expression or the supplied default value if no Cases were matched.</returns> 
        R Result(R defaultValue);
        /// <summary>
        /// Appends a Result Expression to the current pattern, thus specifying the default result to yield when no other patterns have been matched.
        /// Although not enforced by the compiler, Result should only be used as the last clause in the match expression, never in between Case clauses.
        /// </summary>
        /// <param name="defaultValueFactory">The factory function returning a desired default value.</param>
        /// <returns>The result corresponding to the first matched Case expression or the result of invoking the supplied factory function if no Cases were matched.</returns> 
        R Result(Func<R> defaultValueFactory);
        /// <summary>
        /// Appends a Result Expression to the current pattern, thus specifying the default result to yield when no other patterns have been matched.
        /// Although not enforced by the compiler, Result should only be used as the last clause in the match expression, never in between Case clauses.
        /// </summary>
        /// <param name="defaultProjectionFunction">The factory function returning a desired default value.</param>
        /// <returns>The result corresponding to the first matched Case expression or the result of invoking the supplied function on the value being matched if no Cases were matched.</returns> 
        R Result(Func<T, R> defaultProjectionFunction);
        /// <summary>
        /// Appends a When expression to the current pattern. 
        /// This applies a predicate to the value being matched suched that the subsequent Then expression will only be chosen if the predicate returns true.
        /// </summary>
        /// <param name="predicate">The predicate to test the value being matched.</param>
        /// <returns>The IPredicatedPatternMatching&lt;T, R&gt; describing the Match expression so far. This must be followed by a single Then expression.</returns>
        IPredicatedPatternMatching<T, R> When(Func<T, bool> predicate);
        IPredicatedPatternMatching<T, R> When(bool condition);
        /// <summary>
        /// Appends a When expression to the current pattern. 
        /// This applies a predicate to the value being matched suched that the subsequent Then expression will only be chosen if the predicate returns true.
        /// </summary>
        /// <typeparam name="TCase">The Type to match with. That the value being matched is of this type is also necessary for the following then expression to be selected.</typeparam>
        /// <param name="predicate">The predicate to test the value being matched.</param>
        /// <returns>The IPredicatedPatternMatching&lt;T, R&gt; describing the Match expression so far. This must be followed by a single Then expression.</returns>
        IPredicatedPatternMatching<T, R> When<TCase>(Func<TCase, bool> predicate) where TCase : class, T;
        /// <summary>
        /// Appends a Match with Type expression to the current PatternMatching Expression.
        /// </summary>
        /// <typeparam name="TCase">The Type to match with. If the value being matched is of this type, this Case expression will be selected and executed.</typeparam>
        /// <param name="result">The value which, if this Case expression is Matched, will be the result of the Pattern Match.</param>
        /// <returns>The IPatternMatching&lt;T, R&gt; describing the Match expression so far.</returns>
        IPatternMatching<T, R> Case<TCase>(R result) where TCase : class, T;
        /// <summary>
        /// Appends a Match with Type expression to the current PatternMatching Expression.
        /// </summary>
        /// <typeparam name="TCase">The Type to match with. If the value being matched is of this type, this Case expression will be selected and executed.</typeparam>
        /// <param name="func">The function which, if this Case expression is Matched, will be invoked to produce the corresponding desired result for a Match with TCase.</param>
        /// <returns>The IPatternMatching&lt;T, R&gt; describing the Match expression so far.</returns>
        IPatternMatching<T, R> Case<TCase>(Func<R> func) where TCase : class, T;
        /// <summary>
        /// Appends a Match with Type expression to the current PatternMatching Expression.
        /// </summary>
        /// <typeparam name="TCase">The Type to match with. If the value being matched is of this type, this Case expression will be selected and executed.</typeparam>
        /// <param name="func">The function which, if this Case expression is Matched, will be invoked on the value being matched with to produce the desired result for a Match with TCase.</param>
        /// <returns>The IPatternMatching&lt;T, R&gt; describing the Match expression so far.</returns>
        IPatternMatching<T, R> Case<TCase>(Func<TCase, R> func) where TCase : class, T;
    }
}