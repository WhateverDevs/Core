using UnityEngine;
using WhateverDevs.Core.Runtime.Persistence;
using WhateverDevs.Core.Runtime.Serialization;

namespace WhateverDevs.Core.Runtime.Configuration
{
    /// <summary>
    /// Scriptable version of the configuration json file persister for editor referencing.
    /// </summary>
    [CreateAssetMenu(menuName = "WhateverDevs/Persistence/ConfigurationJsonFileOnLocal",
                     fileName = "ConfigurationJsonFilePersisterOnLocalFolder")]
    public class ConfigurationJsonFilePersisterOnLocalFolderScriptable : PersisterScriptable
    {
        /// <summary>
        /// Reference to the json serializer.
        /// </summary>
        public JsonSerializerScriptable JsonSerializer;

        /// <summary>
        /// Actual persister.
        /// </summary>
        public override IPersister Persister =>
            persisterOnLocalFolder ??=
                new ConfigurationJsonFilePersisterOnLocalFolder {Serializer = JsonSerializer.Serializer};

        /// <summary>
        /// Backfield for persister.
        /// </summary>
        private ConfigurationJsonFilePersisterOnLocalFolder persisterOnLocalFolder;
    }
}