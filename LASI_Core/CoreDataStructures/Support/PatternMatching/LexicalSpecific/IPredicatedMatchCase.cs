﻿using System;
namespace LASI.Core.Patternization
{
    /// <summary>
    /// Specifies the required behavior for a pattern matching expression clause which immediately follows a When expression.
    /// </summary>
    /// <typeparam name="T">The Type of the value being matched over.</typeparam>
    /// <typeparam name="TResult">The Type of the result which the match expression may return.</typeparam>
    public interface IPredicatedMatchCase<T, TResult>
      where T : class, LASI.Core.ILexical
    {
        /// <summary>
        /// Appends a Then expression to the current pattern. Then expressions work exactly like Case expressions but are only matched if they immediately follow a When expression which evaluates to true.
        /// </summary>
        /// <typeparam name="TPattern">The Type to match with. 
        /// If the value being matched is of this type and the immediately preceding When expression evaluated to true, 
        /// this Then expression will be selected and the provided action invoked.</typeparam>
        /// <param name="resultValue">The result value to select if this Then expression is matched.</param>
        /// <returns>The IPatternMatching&lt;T, R&gt; describing the Match expression so far.</returns>
        IMatchCase<T, TResult> Then<TPattern>(TResult resultValue) where TPattern : class, T;
        /// <summary>
        /// Appends a Then expression to the current pattern. Then expressions work exactly like Case expressions but are only matched if they immediately follow a When expression which evaluates to true.
        /// </summary> 
        /// <param name="resultValue">The value to select if this Then expression is matched.</param>
        /// <returns>The IPatternMatching&lt;T, R&gt; describing the Match expression so far.</returns>
        IMatchCase<T, TResult> Then(TResult resultValue);
        /// <summary>
        /// Appends a Then expression to the current pattern. Then expressions work exactly like Case expressions but are only matched if they immediately follow a When expression which evaluates to true.
        /// </summary>
        /// <typeparam name="TPattern">The Type to match with. 
        /// If the value being matched is of this type and the immediately preceding When expression evaluated to true, 
        /// this Then expression will be selected and the provided action invoked.</typeparam>
        /// <param name="func">The function returning the value to select if this Then expression is matched.</param>
        /// <returns>The IPatternMatching&lt;T, R&gt; describing the Match expression so far.</returns>
        IMatchCase<T, TResult> Then<TPattern>(Func<TResult> func) where TPattern : class, T;
        /// <summary>
        /// Appends a Then expression to the current pattern. Then expressions work exactly like Case expressions but are only matched if they immediately follow a When expression which evaluates to true.
        /// </summary>
        /// <typeparam name="TPattern">The Type to match with. 
        /// If the value being matched is of this type and the immediately preceding When expression evaluated to true, 
        /// this Then expression will be selected and the provided action invoked.</typeparam>
        /// <param name="func">The function from TPattern -> R which will be invoked to generate the value if this Then expression is matched.</param>
        /// <returns>The IPatternMatching&lt;T, R&gt; describing the Match expression so far.</returns>
        IMatchCase<T, TResult> Then<TPattern>(Func<TPattern, TResult> func) where TPattern : class, T;
        /// <summary>
        /// Appends a Then expression to the current pattern. Then expressions work exactly like Case expressions but are only matched if they immediately follow a When expression which evaluates to true.
        /// </summary>
        /// <param name="func">The function from () -> R which will be invoked to generate the value if this Then expression is matched.</param>
        /// <returns>The IPatternMatching&lt;T, R&gt; describing the Match expression so far.</returns>
        IMatchCase<T, TResult> Then(Func<TResult> func);
        /// <summary>
        /// Appends a Then expression to the current pattern. Then expressions work exactly like Case expressions but are only matched if they immediately follow a When expression which evaluates to true.
        /// </summary>
        /// <param name="func">The function from T -> R which will be invoked to generate the value if this Then expression is matched.</param>
        /// <returns>The IPatternMatching&lt;T, R&gt; describing the Match expression so far.</returns>
        IMatchCase<T, TResult> Then(Func<T, TResult> func);


        /// <summary>
        /// Returns the result of the Pattern Matching expression. 
        /// The result will be one of 3 possibilities: 
        /// 1. The result specified for the first Match with Type expression which succeeded. 
        /// 2. If no matches succeeded, and a Default expression was provided, the result specified in the Default expression.
        /// 3. If no matches succeeded, and a Default expression was not provided, the default value for type the Result Type of the Match Expression.
        /// </summary>
        /// <returns>Returns the result of the Pattern Matching expression.</returns>
        TResult Result();
        /// <summary>
        /// Appends a Result Expression to the current pattern, thus specifying the default result to yield when no other patterns have been matched.
        /// </summary>
        /// <param name="defaultValue">The desired default value.</param>
        /// <returns>The MatchCase&lt;T, R&gt; describing the Match expression so far.</returns> 
        TResult Result(TResult defaultValue);
        /// <summary>
        /// Appends a Result Expression to the current pattern, thus specifying the default result to yield when no other patterns have been matched.
        /// </summary>
        /// <param name="func">The factory function returning a desired default value.</param>
        /// <returns>The MatchCase&lt;T, R&gt; describing the Match expression so far.</returns> 
        TResult Result(Func<TResult> func);
        /// <summary>
        /// Appends a Result Expression to the current pattern, thus specifying the default result to yield when no other patterns have been matched.
        /// </summary>
        /// <param name="func">The factory function returning a desired default value.</param>
        /// <returns>The MatchCase&lt;T, R&gt; describing the Match expression so far.</returns> 
        TResult Result(Func<T, TResult> func);

    }
    /// <summary>
    /// Specifies the required behavior for a pattern matching expression clause which immediately follows a When expression.
    /// </summary>
    /// <typeparam name="T">The Type of the value being matched over.</typeparam>
    public interface IPredicatedMatchCase<T>
    where T : class, LASI.Core.ILexical
    {
        /// <summary>
        /// Appends a Then expression to the current pattern. Then expressions work exactly like Case expressions but are only matched if they immediately follow a When expression which evaluates to true.
        /// </summary>
        /// <typeparam name="TPattern">The Type to match with. 
        /// If the value being matched is of this type and the immediately preceding When expression evaluated to true, 
        /// this Then expression will be selected and the provided action invoked.</typeparam>
        /// <param name="action">Action to invoke if this Then expression is matched.</param>
        /// <returns>The IPatternMatching&lt;T&gt; describing the Match expression so far.</returns>
        IMatchCase<T> Then<TPattern>(Action action) where TPattern : class, T;
        /// <summary>
        /// Appends a Then expression to the current pattern. Then expressions work exactly like Case expressions but are only matched if they immediately follow a When expression which evaluates to true.
        /// </summary>
        /// <typeparam name="TPattern">The Type to match with. 
        /// If the value being matched is of this type and the immediately preceding When expression evaluated to true, 
        /// this Then expression will be selected and the provided action invoked.</typeparam>
        /// <param name="action">Action taking to invoke on the matched value if this Then expression is matched.</param>
        /// <returns>The IPatternMatching&lt;T&gt; describing the Match expression so far.</returns>
        IMatchCase<T> Then<TPattern>(Action<TPattern> action) where TPattern : class, T;
        /// <summary>
        /// Appends a Then expression to the current pattern. Then expressions work exactly like Case expressions but are only matched if they immediately follow a When expression which evaluates to true.
        /// </summary> 
        /// <param name="action">The Action to invoke if this Then expression is matched.</param>
        /// <returns>The IPatternMatching&lt;T R&gt; describing the Match expression so far.</returns>
        IMatchCase<T> Then(Action action);
        /// <summary>
        /// Appends a Then expression to the current pattern. Then expressions work exactly like Case expressions but are only matched if they immediately follow a When expression which evaluates to true.
        /// </summary> 
        /// <param name="action">The Action&lt;T R&gt; to invoke if this Then expression is matched.</param>
        /// <returns>The IPatternMatching&lt;T R&gt; describing the Match expression so far.</returns>
        IMatchCase<T> Then(Action<T> action);

    }
}
