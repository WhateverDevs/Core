using System.Collections;
using UnityEngine;

namespace WhateverDevs.Core.Runtime.Common
{
    /// <summary>
    /// Singleton that can be used to run coroutines for classes that are not monobehaviours.
    /// </summary>
    public class CoroutineRunner : Singleton<CoroutineRunner>
    {
        /// <summary>
        /// Runs the coroutine given.
        /// </summary>
        /// <param name="routine">The coroutine to run.</param>
        public Coroutine RunRoutine(IEnumerator routine)
        {
            GetLogger().Info("Running coroutine " + routine + ".");
            return StartCoroutine(routine);
        }
    }
}