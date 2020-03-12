using System;
using System.Collections;

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
        public void RunRoutine(Func<IEnumerator> routine)
        {
            GetLogger().Info("Running coroutine " + routine + ".");
            StartCoroutine(routine.Invoke());
        }
    }
}