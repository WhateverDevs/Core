using UnityEngine;
using Zenject;

namespace WhateverDevs.Core.Runtime.Configuration
{
    /// <summary>
    /// Extenject installer for the Configuration Manager.
    /// Note that this installer does not install persisters on each of the configurations.
    /// </summary>
    [CreateAssetMenu(menuName = "WhateverDevs/DI/Configuration/ConfigurationManagerInstaller",
                     fileName = "ConfigurationManagerInstaller")]
    public class ConfigurationManagerInstaller : ScriptableObjectInstaller
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

            // Inject the configuration manager to all classes that need that interface.
            Container.Bind<IConfigurationManager>().To<ConfigurationManager>().AsSingle().Lazy();
        }
    }
}