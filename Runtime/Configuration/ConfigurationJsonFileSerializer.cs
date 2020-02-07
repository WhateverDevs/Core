using System.IO;
using WhateverDevs.Core.Runtime.Serialization;

namespace WhateverDevs.Core.Runtime.Configuration
{
    /// <summary>
    /// Json file serializer that adds a prefix for storing the configuration.
    /// </summary>
    public class ConfigurationJsonFileSerializer : JsonFileSerializer
    {
        /// <summary>
        /// Path to the configuration that is automatically added to the destination.
        /// TODO: This will only work on Standalone. Change to persistent path?
        /// </summary>
        private const string ConfigurationPath = "Configuration/";

        /// <summary>
        /// Saves data to the given destination in a Json file.
        /// </summary>
        /// <param name="data">Data to save.</param>
        /// <param name="destination">File path for the json.</param>
        /// <typeparam name="TOriginal">Type of the original data.</typeparam>
        /// <returns>True if it was successful.</returns>
        public override bool Save<TOriginal>(TOriginal data, string destination)
        {
            CheckFolderAndCreate();
            return base.Save(data, ConfigurationPath + destination);
        }

        /// <summary>
        /// Loads the data from the given json file path.
        /// </summary>
        /// <param name="data">Object that will store the data.</param>
        /// <param name="origin">File path of the json file.</param>
        /// <typeparam name="TOriginal">Type of the original data.</typeparam>
        /// <returns>True if it was successful.</returns>
        public override bool Load<TOriginal>(out TOriginal data, string origin) => base.Load(out data, ConfigurationPath + origin);

        /// <summary>
        /// Checks if the configuration folder exists and creates it if it doesn't.
        /// </summary>
        private static void CheckFolderAndCreate()
        {
            if (!Directory.Exists(ConfigurationPath)) Directory.CreateDirectory(ConfigurationPath);
        }
    }
}