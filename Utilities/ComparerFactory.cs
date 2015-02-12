﻿using System;
using System.Collections.Generic;
using System.Linq;
using LASI.Utilities.Validation;

namespace LASI.Utilities
{
    /// <summary>
    /// Provides static methods for the creation of CustomComparer&lt;T&gt; instances.
    /// </summary>
    public static class ComparerFactory
    {
        /// <summary>
        /// Creates a new instance of the CustomComparer class which will use the provided function
        /// to define element equality.
        /// </summary>
        /// <typeparam name="T">The type of the objects to compare.</typeparam>
        /// <param name="equals">A function which determines if two objects of type T are equal.</param>
        /// <exception cref="ArgumentNullException">
        /// The provided <paramref name="equals" /> function is null.
        /// </exception>
        /// <remarks>
        /// A custom hashing function is automatically provided, ensuring that equality comparisons
        /// take place except when reference is null. While this provides clean, customizable
        /// semantics for set operations, more expensive to use having a complexity of N^2
        /// </remarks>
        public static IEqualityComparer<T> CreateEquality<T>(Func<T, T, bool> equals)
        {
            return new CustomComparerImplementation<T>(equals);
        }

        /// <summary>
        /// Creates a new <see cref="IEqualityComparer{T}" /> which will use the provided equality
        /// and hashing functions.
        /// </summary>
        /// <param name="equals">A function which determines if two objects of type T are equal.</param>
        /// <param name="getHashCode">
        /// A function which generates a hash code from an element of type T.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The provided <paramref name="equals" /> or <paramref name="getHashCode" /> function is null.
        /// </exception>
        /// <remarks>
        /// Proper usage requires that elements which will compare equal under the specified equals
        /// function will also produce identical hashcodes. Elements may yield identical hash codes,
        /// without being considered equal.
        /// </remarks>
        /// <returns>
        /// A new <see cref="IEqualityComparer{T}" /> which will use the provided equality and
        /// hashing functions.
        /// </returns>
        public static IEqualityComparer<T> CreateEquality<T>(Func<T, T, bool> equals, Func<T, int> getHashCode)
        {
            return new CustomComparerImplementation<T>(equals, getHashCode);
        }

        /// <summary>
        /// Creates a new instance of the CustomComparer class which will use the provided equality
        /// to define element equality and use the provided functions to compute hashcodes.
        /// </summary>
        /// <typeparam name="T">The type of the objects to compare.</typeparam>
        /// <param name="equals">A function which determines if two objects of type T are equal.</param>
        /// <param name="hashValueSelectors">
        /// One or more functions which describe an ad hoc representation from which hashcodes will
        /// be produced.
        /// </param>
        /// A new
        /// <see cref="IEqualityComparer{T}" />
        /// which will define equality based on the provided equals function and define a hashcode
        /// based on the given hash value selector functions.
        public static IEqualityComparer<T> CreateEquality<T>(Func<T, T, bool> equals, params Func<T, object>[] hashValueSelectors)
        {
            Validate.NotNull(hashValueSelectors, nameof(hashValueSelectors));
            Validate.NotEmpty(hashValueSelectors, nameof(hashValueSelectors), "At least one mapping to a hashable field must be specified.");
            return new CustomComparerImplementation<T>(
               equals: equals,
               getHashCode: value => hashValueSelectors.Select(f => f(value).GetHashCode()).Aggregate((h, x) => h ^ x)
           );
        }

        /// <summary>
        /// An EqualityComparer&lt;T&gt; whose Equals and GetHashCode implementations are specified
        /// upon construction.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <remarks>
        /// <para>
        /// The primary purpose of this class is to allow for the ad hoc, inline creation of an
        /// IEqualityComparer&lt;T&gt; from arbitrary functions. This allows for the easy use of
        /// Query Operators taking an IEqualityComparer&lt;T&gt; as an argument.
        /// </para>
        /// <para>
        /// Proper usage requires that elements which will compare equal under the specified equals
        /// function will also produce identical hashcodes. Elements may yield identical hash codes,
        /// without being considered equal.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// An equality comparer which makes determinations based on the specified comparison function.
        /// var fuzzilyDistinctNps = nps.Distinct(new CustomComparer&lt;Phrase&gt;((x, y) =&gt; x.IsSimilarTo(y));
        /// </code>
        /// <code>
        /// // An equality comparer which makes determinations based on the specified comparison and hashing functions.
        /// var fuzzilyDistinctNps = nps.Distinct(new CustomComparer&lt;Phrase&gt;((x, y) =&gt; x.IsSimilarTo(y), x =&gt; x == null? 0 : 1);
        /// </code>
        /// </example>
        private class CustomComparerImplementation<T> : EqualityComparer<T>
        {
            #region Constructors

            /// <summary>
            /// Initializes a new instance of the CustomComparer class which will use the provided
            /// function to define element equality.
            /// </summary>
            /// <param name="equals">
            /// A function which determines if two objects of type T are equal.
            /// </param>
            /// <exception cref="ArgumentNullException">
            /// The provided <paramref name="equals" /> function is null.
            /// </exception>
            /// <remarks>
            /// A custom hashing function is automatically provided, ensuring that equality
            /// comparisons take place except when reference is null. While this provides clean,
            /// customizable semantics for set operations, more expensive to use having a complexity
            /// of N^2
            /// </remarks>
            public CustomComparerImplementation(Func<T, T, bool> equals) : this(equals, o => o == null ? 0 : 1) { }

            /// <summary>
            /// Initializes a new instance of the CustomComparer class which will use the provided
            /// equality and hashing functions.
            /// </summary>
            /// <param name="equals">
            /// A function which determines if two objects of type T are equal.
            /// </param>
            /// <param name="getHashCode">
            /// A function which generates a hash code from an element of type T.
            /// </param>
            /// <exception cref="ArgumentNullException">
            /// The provided <paramref name="equals" /> or <paramref name="getHashCode" /> function
            /// is null.
            /// </exception>
            /// <remarks>
            /// Proper usage requires that elements which will compare equal under the specified
            /// equals function will also produce identical hashcodes. Elements may yield identical
            /// hash codes, without being considered equal.
            /// </remarks>
            public CustomComparerImplementation(Func<T, T, bool> equals, Func<T, int> getHashCode)
            {
                Validate.NotNull(equals, "equals", "A null equals function was provided.");
                Validate.NotNull(getHashCode, "getHashCode", "A null getHashCode function was provided.");
                this.equals = equals;
                this.getHashCode = getHashCode;
            }

            #endregion Constructors

            #region Methods

            /// <summary>
            /// Determines whether two objects of type T are equal.
            /// </summary>
            /// <param name="x">The first object to compare.</param>
            /// <param name="y">The second object to compare.</param>
            /// <returns><c>true</c> if the specified objects are equal; otherwise, <c>false</c>.</returns>
            public override bool Equals(T x, T y)
            {
                if (ReferenceEquals(x, null))
                    return ReferenceEquals(y, null);
                else if (ReferenceEquals(y, null))
                    return ReferenceEquals(x, null);
                else
                    return equals(x, y);
            }

            /// <summary>
            /// Serves as a hash function for the specified object for hashing algorithms and data
            /// structures, such as a hash table.
            /// </summary>
            /// <param name="obj">The object for which to get a hash code.</param>
            /// <returns>A hash code for the specified object.</returns>
            public override int GetHashCode(T obj)
            {
                return getHashCode(obj);
            }

            #endregion Methods

            #region Fields

            private Func<T, T, bool> equals;
            private Func<T, int> getHashCode;

            #endregion Fields
        }
    }
}