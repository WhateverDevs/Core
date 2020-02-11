using UnityEngine;
using WhateverDevs.Core.Runtime.Serialization;

namespace WhateverDevs.Core.Runtime.Persistence
{
    /// <summary>
    /// Scriptable version of the json file persister for editor referencing.
    /// </summary>
    [CreateAssetMenu(menuName = "WhateverDevs/Persistence/JsonFile", fileName = "JsonFilePersister")]
    public class JsonFilePersisterScriptable : PersisterScriptable
    {
        /// <summary>
        /// Reference to the json serializer.
        /// </summary>
        public JsonSerializerScriptable JsonSerializer;

        /// <summary>
        /// Actual persister.
        /// </summary>
        public override IPersister Persister =>
            persister ?? (persister = new JsonFilePersister {Serializer = JsonSerializer.Serializer});

        /// <summary>
        /// Backfield for persister.
        /// </summary>
        private JsonFilePersister persister;
    }
}