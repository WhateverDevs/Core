using UnityEngine;
using WhateverDevs.Core.Runtime.Serialization;
using Zenject;

namespace WhateverDevs.Core.Runtime.Persistence
{
    /// <summary>
    /// Extenject installer for the JsonPersister.
    /// </summary>
    [CreateAssetMenu(menuName = "WhateverDevs/DI/Persistence/JsonPersisterInstaller",
                     fileName = "JsonPersisterInstaller")]
    public class JsonPersisterInstaller : ScriptableObjectInstaller
    {
        /// <summary>
        /// Define injections.
        /// </summary>
        public override void InstallBindings() =>
            // Inject a lazy singleton Json serializer into the Json persister.
            Container.Bind<ISerializer<string>>()
                     .To<JsonSerializer>()
                     .AsSingle()
                     .WhenInjectedInto<JsonFilePersister>()
                     .Lazy();
    }
}