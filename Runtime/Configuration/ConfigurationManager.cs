using System;
using System.Collections.Generic;
using UnityEngine;
using WhateverDevs.Core.Runtime.Common;
using Zenject;

namespace WhateverDevs.Core.Runtime.Configuration
{
    /// <summary>
    /// Basic implementation of a configuration manager.
    /// </summary>
    public class ConfigurationManager : Initializable<ConfigurationManager>, IConfigurationManager
    {
        /// <summary>
        /// Configurations this manager handles.
        /// </summary>
        public List<IConfiguration> Configurations { get; set; }

        /// <summary>
        /// Event called when a configuration is updated.
        /// </summary>
        public Action<ConfigurationData> ConfigurationUpdated { get; set; }

        /// <summary>
        /// Inject the references and initialize.
        /// </summary>
        /// <param name="configurations">Reference to the configuration files.</param>
        [Inject]
        public void Construct(List<IConfiguration> configurations)
        {
            Configurations = configurations;
            Initialize();
        }

        /// <summary>
        /// Constructor that loads the configurations.
        /// </summary>
        protected override bool OnInitialization()
        {
            List<Type> checkedTypes = new List<Type>();

            if (Configurations == null)
            {
                Debug.LogError("Configurations have not been injected!");
                return false;
            }
                
            for (int i = 0; i < Configurations.Count; ++i)
            {
                if (checkedTypes.Contains(Configurations[i].GetType()))
                {
                    Debug.LogError("You have two configuration files of the same type added to the configuration list. "
                                 + "Only one file of each type should be created. Manager won't initialize."); // TODO: change to custom log system.

                    return false;
                }

                checkedTypes.Add(Configurations[i].GetType());
                Configurations[i].Load();
            }

            return true;
        }

        /// <summary>
        /// Retrieves a certain configuration.
        /// </summary>
        /// <param name="configurationData">The configuration asked for.</param>
        /// <typeparam name="TConfigurationData">The type of configuration to retrieve.</typeparam>
        /// <returns>True if it was successful.</returns>
        public bool GetConfiguration<TConfigurationData>(out TConfigurationData configurationData)
            where TConfigurationData : ConfigurationData
        {
            configurationData = null;

            if (!Initialized)
            {
                Debug.LogError("The configuration manager has not been initialized!"); // TODO: change to custom log system.
                return false;
            }

            for (int i = 0; i < Configurations.Count; ++i)
                if (Configurations[i] is IConfiguration<TConfigurationData>)
                {
                    configurationData = ((IConfiguration<TConfigurationData>) Configurations[i]).ConfigurationData;
                    return true;
                }

            Debug.LogError("Config file not found for configuration type "
                         + typeof(TConfigurationData)
                         + "!"); // TODO: change to custom log system.

            return false;
        }

        /// <summary>
        /// Sets a new configuration of the given type.
        /// </summary>
        /// <param name="configurationData">The new configuration values.</param>
        /// <typeparam name="TConfigurationData">The type of configuration to set.</typeparam>
        /// <returns>True if it was successful.</returns>
        public bool SetConfiguration<TConfigurationData>(TConfigurationData configurationData)
            where TConfigurationData : ConfigurationData
        {
            if (!Initialized)
            {
                Debug.LogError("The configuration manager has not been initialized!"); // TODO: change to custom log system.
                return false;
            }

            for (int i = 0; i < Configurations.Count; ++i)
                if (Configurations[i] is IConfiguration<TConfigurationData>)
                {
                    IConfiguration<TConfigurationData> configuration =
                        (IConfiguration<TConfigurationData>) Configurations[i];

                    configuration.ConfigurationData = configurationData;
                    configuration.Save();

                    ConfigurationUpdated?.Invoke(configurationData);

                    return true;
                }

            Debug.LogError("Config file not found for configuration type "
                         + typeof(TConfigurationData)
                         + "!"); // TODO: change to custom log system.

            return false;
        }
    }
}