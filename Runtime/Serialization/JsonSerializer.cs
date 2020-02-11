using System.Runtime.Serialization;
using UnityEngine;

namespace WhateverDevs.Core.Runtime.Serialization
{
    /// <summary>
    /// Class that serializes and retrieves data to and from a Json string.
    /// </summary>
    public class JsonSerializer : ISerializer<string>
    {
        /// <summary>
        /// Transform the data to a Json string.
        /// </summary>
        /// <param name="original">Original data.</param>
        /// <typeparam name="TOriginal">Type of the original data.</typeparam>
        /// <returns>The data as a Json string.</returns>
        public string To<TOriginal>(TOriginal original)
        {
            if (typeof(TOriginal).IsSerializable) return JsonUtility.ToJson(original, true);

            Debug.LogError("Data is not serializable, will not serialize."); // TODO: Change to custom log system.
            return null;
        }

        /// <summary>
        /// Transforms data from a Json string to the given type.
        /// </summary>
        /// <param name="serialized">Data as a Json string.</param>
        /// <typeparam name="TOriginal">Type of the original data.</typeparam>
        /// <returns>The original data in the original type.</returns>
        public TOriginal From<TOriginal>(string serialized)
        {
            if (typeof(TOriginal).IsSerializable) return JsonUtility.FromJson<TOriginal>(serialized);
            
            Debug.LogError("Data type is not serializable, will not deserialize."); // TODO: Change to custom log system.
            throw new SerializationException(); // We can't just return null as TOriginal may not be nullable.
        }
    }
}