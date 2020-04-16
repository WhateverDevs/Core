using System;

namespace WhateverDevs.Core.Runtime.DataStructures
{
    /// <summary>
    /// Serializable class for reference pair to ease the creation of serializable dictionaries.
    /// </summary>
    [Serializable]
    public class ObjectPair<T1, T2>
    {
        /// <summary>
        /// First value.
        /// </summary>
        public T1 Key;

        /// <summary>
        /// Second value;
        /// </summary>
        public T2 Value;
    }
}