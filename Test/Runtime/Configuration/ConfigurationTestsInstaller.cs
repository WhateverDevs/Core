using Packages.Core.Runtime.Serialization;
using UnityEngine;
using WhateverDevs.Core.Runtime.Configuration;
using WhateverDevs.Core.Runtime.Formatting;
using WhateverDevs.Core.Runtime.Serialization;
using WhateverDevs.Core.Test.Editor.Configuration;
using Zenject;

namespace WhateverDevs.Core.Test.Runtime.Configuration
{
    /// <summary>
    /// Extenject installer for the runtime configuration tests.
    /// </summary>
    [CreateAssetMenu(menuName = "WhateverDevs/Test/ConfigurationTestsInstaller",
                     fileName = "ConfigurationTestsInstaller")]
    public class ConfigurationTestsInstaller : ScriptableObjectInstaller
    {
        /// <summary>
        /// Configurations that need a serializer injection.
        /// </summary>
        public ConfigurationScriptableHolder[] ConfigurationsToInstall;

        /// <summary>
        /// Define injections.
        /// </summary>
        public override void InstallBindings()
        {
            // Scriptable objects don't register for injection automatically, so we need to do it manually.
            for (int i = 0; i < ConfigurationsToInstall.Length; ++i)
                Container.QueueForInject(ConfigurationsToInstall[i]);

            // Inject a lazy singleton Json formatter into the Json Serializer.
            Container.Bind<IFormatter<string>>()
                     .To<JsonFormatter>()
                     .AsSingle()
                     .WhenInjectedInto<JsonFileSerializer>()
                     .Lazy();

            // Inject a lazy singleton Json File serializer into the test configuration.
            Container.Bind<ISerializer>()
                     .To<ConfigurationJsonFileSerializer>()
                     .AsSingle()
                     .WhenInjectedInto<TestConfiguration>()
                     .Lazy();
        }
    }
}