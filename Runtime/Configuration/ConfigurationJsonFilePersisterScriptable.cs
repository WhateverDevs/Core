using UnityEngine;
using WhateverDevs.Core.Runtime.Persistence;
using WhateverDevs.Core.Runtime.Serialization;

namespace WhateverDevs.Core.Runtime.Configuration
{
    /// <summary>
    /// Scriptable version of the configuration json file persister for editor referencing.
    /// </summary>
    [CreateAssetMenu(menuName = "WhateverDevs/Persistence/ConfigurationJsonFile", fileName = "ConfigurationJsonFilePersister")]
    public class ConfigurationJsonFilePersisterScriptable : PersisterScriptable
    {
        /// <summary>
        /// Reference to the json serializer.
        /// </summary>
        public JsonSerializerScriptable JsonSerializer;

        /// <summary>
        /// Actual persister.
        /// </summary>
        public override IPersister Persister =>
            persister ?? (persister = new ConfigurationJsonFilePersister {Serializer = JsonSerializer.Serializer});

        /// <summary>
        /// Backfield for persister.
        /// </summary>
        private ConfigurationJsonFilePersister persister;
    }
}