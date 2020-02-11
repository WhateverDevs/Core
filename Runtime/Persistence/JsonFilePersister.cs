using System;
using System.IO;
using UnityEngine;
using WhateverDevs.Core.Runtime.Serialization;
using Zenject;

namespace WhateverDevs.Core.Runtime.Persistence
{
    /// <summary>
    /// Persister that stores data to file as Json strings.
    /// </summary>
    public class JsonFilePersister : IPersister
    {
        /// <summary>
        /// Serializer that this persister will use.
        /// This should be injected by ExtenJect on runtime and manually assigned on Editor.
        /// </summary>
        [Inject]
        public ISerializer<string> Serializer { get; set; }

        /// <summary>
        /// Saves data to the given destination in a Json file.
        /// </summary>
        /// <param name="data">Data to save.</param>
        /// <param name="destination">File path for the json.</param>
        /// <typeparam name="TOriginal">Type of the original data.</typeparam>
        /// <returns>True if it was successful.</returns>
        public virtual bool Save<TOriginal>(TOriginal data, string destination)
        {
            if (!Directory.GetParent(destination).Exists)
            {
                Debug.LogError("The given destination folder doesn't exist!"); // TODO: Change to custom log system.
                return false;
            }

            string jsonString = Serializer.To(data);

            if (string.IsNullOrEmpty(jsonString)) return false;

            try
            {
                File.WriteAllText(destination, jsonString);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception); // TODO: Change to custom log system.
                return false;
            }

            return true;
        }

        /// <summary>
        /// Loads the data from the given json file path.
        /// </summary>
        /// <param name="data">Object that will store the data.</param>
        /// <param name="origin">File path of the json file.</param>
        /// <typeparam name="TOriginal">Type of the original data.</typeparam>
        /// <returns>True if it was successful.</returns>
        public virtual bool Load<TOriginal>(out TOriginal data, string origin)
        {
            if (!File.Exists(origin))
            {
                Debug.LogError("The given file does not exist: " + origin); // TODO: Change to custom log system.
                throw new ArgumentException(); // We can't just return null as TOriginal may not be nullable.
            }

            string jsonString = File.ReadAllText(origin);

            data = Serializer.From<TOriginal>(jsonString);
            return true;
        }
    }
}