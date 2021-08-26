using System.Collections.Generic;
using UnityEngine;
using WhateverDevs.Core.Runtime.Common;

namespace WhateverDevs.Core.Runtime.DataStructures
{
    /// <summary>
    /// Scriptable object that can be used to create a simple library that stores a list of types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DataLibrary<T> : LoggableScriptableObject<DataLibrary<T>>
    {
        /// <summary>
        /// List of types in the library.
        ///
        /// We could add attributes on the child class but we would get a serialization error,
        /// so attributes are added here.
        /// 
        /// Attributes support:
        /// Alpha and hdr colors.
        /// </summary>
        [ColorUsage(true, true)]
        public List<T> Elements;

        /// <summary>
        /// Get a random element from the library.
        /// </summary>
        /// <returns></returns>
        public T GetRandom() => Elements[Random.Range(0, Elements.Count)];
    }
}