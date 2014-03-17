﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LASI.Utilities
{
    /// <summary>
    /// An EqualityComparer&lt;T&gt; whose Equals and GetHashCode implementations are specified on upon construction.
    /// </summary>
    /// <typeparam name="T">The type of objects to compare.</typeparam>
    /// <remarks>
    /// <para>
    /// The primary purpose of this class is to allow for the ad hoc, inline creation of an IEqualityComparer&lt;T&gt; from arbitrary functions.
    /// This allows for the easy use of Query Operators taking an IEqualityComparer&lt;T&gt; as an argument.
    /// </para>
    /// <para>
    /// Proper usage requires that elements which will compare equal under the specified equals function will also produce identical hashcodes.
    /// Elements may yield identical hash codes, without being considered equal.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Equality Comparer which defines equality in terms of the sepcified function.
    /// var fuzzilyDistinctNps = nps.Distinct(new CustomComparer&lt;Phrase&gt;((x, y) => x.IsSimilarTo(y));
    /// </code>
    /// <code>
    /// // Equality Comparer which determines equality in based on the specified comparison and hashing functions.
    /// var fuzzilyDistinctNps = nps.Distinct(new CustomComparer&lt;Phrase&gt;((x, y) => x.IsSimilarTo(y), x => x == null? 0: 1);
    /// </code>
    /// </example>
    public class CustomComparer<T> : EqualityComparer<T>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the CustomComparer class which will use the provided equals function. to define element equality.
        /// </summary>
        /// <param name="equals">A function which determines if two objects of type T are equal.</param>
        /// <exception cref="System.ArgumentNullException">The provided <paramref name="equals"/> function is null.</exception>
        /// <remarks>
        /// A custom hashing function is automatically provided, ensuring that equality comparisons take place except when reference is null.
        /// While this provides clean, customizable semantics for set operations, more expensive to use having a complexity of N^2
        /// </remarks>
        public CustomComparer(Func<T, T, bool> equals) {
            if (equals == null)
                throw new ArgumentNullException("equals", "A null equals function was provided.");
            this.equals = equals;
            getHashCode = o => o == null ? 0 : 1;
        }
        /// <summary>
        /// Initializes a new instance of the CustomComparer class which will use the provided equals and get hashcode functions.
        /// </summary>
        /// <param name="equals">A function which determines if two objects of type T are equal.</param>
        /// <param name="getHashCode">A function which generates a hash code from an element of type T.</param>
        /// <exception cref="System.ArgumentNullException">The provided <paramref name="equals"/> or <paramref name="getHashCode"/> function is null.</exception>
        /// <remarks>Proper usage requires that elements which will compare equal under the specified equals function will also produce identical hashcodes.
        /// Elements may yield identical hash codes, without being considered equal.
        /// </remarks>

        public CustomComparer(Func<T, T, bool> equals, Func<T, int> getHashCode) {
            if (equals == null)
                throw new ArgumentNullException("equals", "A null equals function was provided.");
            if (getHashCode == null)
                throw new ArgumentNullException("getHashCode", "A null getHashCode function was provided.");
            this.equals = equals;
            this.getHashCode = getHashCode;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Determines whether two objects of type T are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>true if the specified objects are equal; otherwise, false.</returns>
        public override bool Equals(T x, T y) {
            if (ReferenceEquals(x, null))
                return ReferenceEquals(y, null);
            else if (ReferenceEquals(y, null))
                return ReferenceEquals(x, null);
            else
                return equals(x, y);
        }
        /// <summary>
        /// Serves as a hash function for the specified object for hashing algorithms and data structures, such as a hash table.
        /// </summary>
        /// <param name="obj">The object for which to get a hash code.</param>
        /// <returns>A hash code for the specified object.</returns>
        public override int GetHashCode(T obj) {
            return getHashCode(obj);
        }
        #endregion

        #region Fields
        private Func<T, T, bool> equals;
        private Func<T, int> getHashCode;
        #endregion
    }
}