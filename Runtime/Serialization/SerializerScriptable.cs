using UnityEngine;

namespace WhateverDevs.Core.Runtime.Serialization
{
    /// <summary>
    /// Scriptable to get a serializer for editor referencing.
    /// </summary>
    public abstract class SerializerScriptable<TSerializer, TSerialized> : ScriptableObject
        where TSerializer : ISerializer<TSerialized>, new()
    {
        /// <summary>
        /// Get a serializer.
        /// </summary>
        public TSerializer Serializer = new TSerializer();
    }
}