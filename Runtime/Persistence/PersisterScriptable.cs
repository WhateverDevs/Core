using UnityEngine;

namespace WhateverDevs.Core.Runtime.Persistence
{
    /// <summary>
    /// Scriptable version of persister for editor referencing.
    /// </summary>
    public abstract class PersisterScriptable : ScriptableObject
    {
        /// <summary>
        /// Actual persister.
        /// </summary>
        public abstract IPersister Persister { get; }
    }
}