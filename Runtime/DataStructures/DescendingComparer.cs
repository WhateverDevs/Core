using System;
using System.Collections.Generic;

namespace WhateverDevs.Core.Runtime.DataStructures
{
    /// <summary>
    /// Descending comparer to be used in SortedDictionaries.
    /// See: https://stackoverflow.com/questions/931891/reverse-sorted-dictionary-in-net
    /// </summary>
    /// <typeparam name="T">Element to compare.</typeparam>
    public class DescendingComparer<T> : IComparer<T> where T : IComparable<T>
    {
        /// <summary>
        /// Compare the two elements.
        /// </summary>
        /// <param name="x">First element.</param>
        /// <param name="y">Second element.</param>
        /// <returns>The order.</returns>
        // ReSharper disable once PossibleNullReferenceException
        public int Compare(T x, T y) => y.CompareTo(x);
    }
}