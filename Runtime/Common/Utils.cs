using UnityEngine;

namespace WhateverDevs.Core.Runtime.Common
{
    /// <summary>
    /// Class with utility functions and extensions.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Checks if a game object has the DontDestroyOnLoadFlag.
        /// </summary>
        /// <param name="gameObject">The game object to check.</param>
        /// <returns>True if it won't be destroyed.</returns>
        public static bool IsDontDestroyOnLoad(this GameObject gameObject) => gameObject.scene.buildIndex == -1;
    }
}