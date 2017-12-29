﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LASI.Utilities.Validation;

namespace LASI.Utilities.Specialized.Enhanced.IList.Linq
{
    /// <summary>
    /// Provides a set of methods for querying collections of type <see cref="IList{T}"/> allowing
    /// queries over lists to transparently return <see cref="IList{T}"/> instead of <see cref="IEnumerable{T}"/>
    /// </summary>
    /// <seealso cref="Enumerable"/>
    /// <seealso cref="IList{T}"/>
    /// <seealso cref="List{T}"/>
    public static class IListExtensions
    {
        #region Select

        /// <summary>Projects each element of a list into a new form.</summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <typeparam name="R">The type of the value returned by selector.</typeparam>
        /// <param name="list">A list of values to invoke a transform function on.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>
        /// A <see cref="IList{R}"/> whose elements are the result of invoking the transform
        /// function on each element of source.
        /// </returns>
        [DebuggerHidden]
        public static IReadOnlyList<R> Select<T, R>(this List<T> list, Func<T, R> selector)
        {
            var ls = list as IReadOnlyList<T> ?? list.ToList();
            return ls.Select((e, i) => selector(e));
        }

        [DebuggerHidden]
        public static IReadOnlyList<R> Select<T, R>(this IReadOnlyList<T> list, Func<T, R> selector)
        {
            return Select(list, selector1);

            R selector1(T e, int i) => selector(e);
        }


        [DebuggerHidden]
        public static IReadOnlyList<R> Select<T, R>(this IReadOnlyList<T> list, Func<T, int, R> selector)
        {
            var results = new List<R>(list.Count);
            list.ForEach(action);

            return results;

            void action(T e, int i) => results.Add(selector(e, i));
        }


        #endregion Select

        #region Where

        /// <summary>Filters a list of values based on a predicate.</summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="list">A <see cref="IList{T}"/> to filter.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        /// A <see cref="IList{T}"/> that contains elements from the input list that satisfy the condition.
        /// </returns>
        [DebuggerHidden]
        public static IList<T> Where<T>(this IList<T> list, Func<T, bool> predicate) => list.AsEnumerable().Where(predicate).ToList();

        [DebuggerHidden]
        public static IReadOnlyList<T> Where<T>(this List<T> list, Func<T, bool> predicate) => list.AsEnumerable().Where(predicate).ToList();
        /// <summary>Filters a list of values based on a predicate.</summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="list">A <see cref="IList{T}"/> to filter.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        /// A <see cref="IList{T}"/> that contains elements from the input list that satisfy the condition.
        /// </returns>
        [DebuggerHidden]
        public static IList<T> Where<T>(this IList<T> list, Func<T, int, bool> predicate) => list.AsEnumerable().Where(predicate).ToList();

        [DebuggerHidden]
        public static IReadOnlyList<T> Where<T>(this List<T> list, Func<T, int, bool> predicate) => list.AsEnumerable().Where(predicate).ToList();

        #endregion Where

        #region SelectMany

        /// <summary>
        /// Projects each element of a list to an <see cref="IEnumerable{T}"/> and flattens the
        /// resulting sequences into one list.
        /// </summary>
        /// <typeparam name="T">The type of the elements of list.</typeparam>
        /// <typeparam name="R">The type of the elements of the sequence returned by selector.</typeparam>
        /// <param name="list">A list of values to project.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>
        /// A <see cref="IList{R}"/> whose elements are the result of invoking the one-to-many
        /// transform function on each element of the input list.
        /// </returns>
        [DebuggerHidden]
        public static IReadOnlyList<R> SelectMany<T, R>(this IReadOnlyList<T> list, Func<T, IEnumerable<R>> selector)
        {
            return from item in list
                   let results = selector(item).ToList()
                   from result in results
                   select result;
        }

        /// <summary>
        /// Projects each element of a list to an <see cref="IEnumerable{T}"/>, flattens the
        /// resulting sequences into one list, and invokes a result selector function on each
        /// element therein.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <typeparam name="TCollection">
        /// The type of the intermediate elements collected by collectionSelector.
        /// </typeparam>
        /// <typeparam name="R">The type of the value returned by selector.</typeparam>
        /// <param name="list">A <see cref="List{T}"/> to project.</param>
        /// <param name="collectionSelector">
        /// A transform function to apply to each element of the input list.
        /// </param>
        /// <param name="resultSelector">
        /// A transform function to apply to each element of the intermediate sequence.
        /// </param>
        /// <returns>
        /// A <see cref="IList{R}"/> whose elements are the result of invoking the one-to-many
        /// transform function collectionSelector on each element of source and then mapping each of
        /// those sequence elements and their corresponding source element to a result element.
        /// </returns>
        [DebuggerHidden]
        public static IReadOnlyList<R> SelectMany<T, TCollection, R>(this IReadOnlyList<T> list, Func<T, IReadOnlyList<TCollection>> collectionSelector, Func<T, TCollection, R> resultSelector)
        {
            var results = new List<R>();
            for (var i = 0; i < list.Count; ++i)
            {
                var element = list[i];
                var selectResult = resultSelector.Apply(element);
                results.AddRange(collectionSelector(element).Select(selectResult));

                //Func<TCollection, R> selector(T e) => selectResult(e);
            }
            return results;

        }
        [DebuggerHidden]
        public static IReadOnlyList<R> SelectMany<T, TCollection, R>(this IReadOnlyList<T> list, Func<T, IEnumerable<TCollection>> collectionSelector, Func<T, TCollection, R> resultSelector) => SelectMany(list, e => collectionSelector(e).ToList(), resultSelector);
        #endregion SelectMany

        #region Takes

        /// <summary>Returns a specified number of contiguous elements from the start of a list.</summary>
        /// <typeparam name="T">The type of the elements of the list</typeparam>
        /// <param name="list">The <see cref="List{T}"/> to return elements from.</param>
        /// <param name="count">The number of elements to return.</param>
        /// <returns>
        /// A <see cref="List{T}"/> that contains the specified number of elements from the start of
        /// the input sequence.
        /// </returns>

        [DebuggerHidden]
        public static IReadOnlyList<T> Take<T>(this List<T> list, int count)
        {
            Validate.NotNull(list, nameof(list));
            if (count < 0)
            { return new List<T>(); }
            if (count > list.Count)
            { return list.GetRange(0, list.Count); }
            return list.GetRange(0, count);
        }
        [DebuggerStepThrough]
        public static IReadOnlyList<T> TakeWhile<T>(this List<T> list, Func<T, bool> predicate)
        {
            Validate.NotNull(list, nameof(list), predicate, nameof(predicate));
            var i = 0;
            while (i < list.Count && predicate(list[i]))
            {
                ++i;
            }
            return new List<T>(list.Take(i));
        }
        #endregion Takes

        #region Skips

        /// <summary>
        /// Bypasses a specified number of elements in a list and then returns the remaining elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the list</typeparam>
        /// <param name="list">The <see cref="List{T}"/> to return elements from.</param>
        /// <param name="count">
        /// The number of elements to skip before returning the remaining elements.
        /// </param>
        /// <returns>
        /// A <see cref="List{T}"/> that contains the elements that occur after the specified index
        /// in the input list.
        /// </returns>
        [DebuggerHidden]
        public static IReadOnlyList<T> Skip<T>(this IReadOnlyList<T> list, int count)
        {
            if (count > list.Count)
            {
                return Array.Empty<T>();
            }
            else if (count < 1 && list is List<T> l)
            {
                return l.GetRange(0, list.Count);
            }

            return GetRange(count < 0 ? 0 : count, list.Count - count);

            ArraySegment<T> GetRange(int offset, int c)
            {
                return new ArraySegment<T>(list.ToArray(), offset, c);
            }
        }



        /// <summary>
        /// Bypasses the elements of the specified list while they satisfy the specified predicate.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the list.</typeparam>
        /// <param name="list">The <see cref="List{T}"/> to return elements from.</param>
        /// <param name="predicate">The predicate to test elements.</param>
        /// <returns>All elements in the list including and following the first that does not satisfy the predicate.</returns>
        [DebuggerHidden]
        public static IReadOnlyList<T> SkipWhile<T>(this IReadOnlyList<T> list, Func<T, bool> predicate)
        {
            Validate.NotNull(list, nameof(list), predicate, nameof(predicate));
            var i = 0;
            while (i < list.Count && predicate(list[i]))
            {
                ++i;
            }
            return new List<T>(list.Skip(i));
        }

        #endregion Skips

        #region Concat

        [DebuggerHidden]
        public static IReadOnlyList<T> Concat<T>(this IList<T> first, IList<T> second)
        {
            var result = new T[first.Count + second.Count];
            first.CopyTo(result, 0);
            second.CopyTo(result, first.Count);
            return result;
        }
        #endregion

        #region ForEach

        /// <summary>Performs the specified action on each element of the <see cref="System.Collections.Generic.IList{T}"/>.</summary>
        /// <typeparam name="T">The type of the elements of the list</typeparam>
        /// <param name="list">The list over which to execute.</param>
        /// <param name="action">
        /// The <see cref="System.Action{T}"/> delegate to perform on each element of the <see cref="System.Collections.Generic.IList{T}"/>.
        /// </param>
        [DebuggerHidden]
        public static void ForEach<T>(this IReadOnlyList<T> list, Action<T> action) => ForEach(list, (e, i) => action(e));
        [DebuggerHidden]
        public static void ForEach<T>(this IReadOnlyList<T> list, Action<T, int> action)
        {
            for (var i = 0; i < list.Count; ++i)
            {
                action(list[i], i);
            }
        }

        #endregion ForEach

        [DebuggerHidden]
        public static IReadOnlyList<(T element, int index)> WithIndices<T>(this IList<T> list)
        {
            var results = new List<(T, int)>(list.Count);
            for (var i = 0; i < list.Count; ++i)
            {
                results.Add((list[i], i));
            }
            return results;
        }
    }
}
