using UnityEngine;
using WhateverDevs.Core.Runtime.Configuration;
using WhateverDevs.Core.Test.Editor.Configuration;
using WhateverDevs.Core.Runtime.Persistence;
using WhateverDevs.Core.Runtime.Serialization;
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
        /// Configurations that need a persister injection.
        /// </summary>
        public ConfigurationScriptableHolder[] ConfigurationsToInstall;

        /// <summary>
        /// Define injections.
        /// </summary>
        public override void InstallBindings()
        {
            // Scriptable objects don't register for injection automatically, so we need to do it manually.
            for (int i = 0; i < ConfigurationsToInstall.Length; ++i)
            {
                Container.QueueForInject(ConfigurationsToInstall[i]);

                // Inject a lazy singleton of each configuration into any configuration manager.
                Container.Bind<IConfiguration>()
                         .FromInstance((IConfiguration) ConfigurationsToInstall[i])
                         .WhenInjectedInto<ConfigurationManager>();
            }

            // Inject a lazy singleton Json serializer into the Json persister.
            Container.Bind<ISerializer<string>>()
                     .To<JsonSerializer>()
                     .AsSingle()
                     .WhenInjectedInto<JsonFilePersister>()
                     .Lazy();

            // Inject a lazy singleton Json File persister into the test configuration.
            Container.Bind<IPersister>()
                     .To<ConfigurationJsonFilePersister>()
                     .AsSingle()
                     .WhenInjectedInto<TestConfiguration>()
                     .Lazy();

            // Inject the configuration manager to all classes that need that interface.
            Container.Bind<IConfigurationManager>().To<ConfigurationManager>().AsSingle().Lazy();
        }
    }
}