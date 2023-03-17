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
        public static Coroutine RunRoutine(IEnumerator routine) => Instance.RunRoutineInternal(routine);

        /// <summary>
        /// Runs the coroutine given.
        /// </summary>
        /// <param name="routine">The coroutine to run.</param>
        private Coroutine RunRoutineInternal(IEnumerator routine)
        {
            Logger.Info("Running coroutine " + routine + ".");
            return StartCoroutine(routine);
        }
    }
}