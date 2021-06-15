using UnityEngine;
using WhateverDevs.Core.Runtime.Persistence;

namespace WhateverDevs.Core.Runtime.Configuration
{
    /// <summary>
    /// Json file persister that adds a prefix for storing the configuration on the persistent data path.
    /// It uses the same logic as the one with the local folder, but a different path.
    /// </summary>
    [CreateAssetMenu(menuName = "WhateverDevs/Persistence/ConfigurationJsonFilePersistentDataPath",
                     fileName = "ConfigurationJsonFilePersisterOnPersistentDataPath")]
    public class
        ConfigurationJsonFilePersisterOnPersistentDataPathScriptable :
            ConfigurationJsonFilePersisterOnLocalFolderScriptable
    {
        /// <summary>
        /// Actual persister.
        /// </summary>
        public override IPersister Persister =>
            persister ??=
                new ConfigurationJsonFilePersisterOnPersistentDataPath {Serializer = JsonSerializer.Serializer};

        /// <summary>
        /// Backfield for persister.
        /// </summary>
        private ConfigurationJsonFilePersisterOnPersistentDataPath persister;
    }
}