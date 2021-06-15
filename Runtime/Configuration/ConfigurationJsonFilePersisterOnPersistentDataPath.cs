using UnityEngine;

namespace WhateverDevs.Core.Runtime.Configuration
{
    /// <summary>
    /// Json file persister that adds a prefix for storing the configuration on the persistent data path.
    /// It uses the same logic as the one with the local folder, but a different path.
    /// </summary>
    public class ConfigurationJsonFilePersisterOnPersistentDataPath : ConfigurationJsonFilePersisterOnLocalFolder
    {
        /// <summary>
        /// Override the path to use the persistent data path.
        /// </summary>
        protected override string ConfigurationPath => Application.persistentDataPath + "/Configuration/";
    }
}