using System.IO;
using WhateverDevs.Core.Runtime.Persistence;

namespace WhateverDevs.Core.Runtime.Configuration
{
    /// <summary>
    /// Json file persister that adds a prefix for storing the configuration on the local folder.
    /// </summary>
    public class ConfigurationJsonFilePersisterOnLocalFolder : JsonFilePersister
    {
        /// <summary>
        /// Path to the configuration that is automatically added to the destination.
        /// </summary>
        protected virtual string ConfigurationPath => "Configuration/";

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
        public override bool Load<TOriginal>(out TOriginal data, string origin)
        {
            try
            {
                bool success = base.Load(out data, ConfigurationPath + origin);
                return success;
            }
            catch
            {
                data = default;
                return false;
            }
        }

        /// <summary>
        /// Checks if the configuration folder exists and creates it if it doesn't.
        /// </summary>
        private void CheckFolderAndCreate()
        {
            if (!Directory.Exists(ConfigurationPath)) Directory.CreateDirectory(ConfigurationPath);
        }
    }
}