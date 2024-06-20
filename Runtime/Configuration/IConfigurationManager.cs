using System;
using System.Collections.Generic;

namespace WhateverDevs.Core.Runtime.Configuration
{
    /// <summary>
    /// Interface that defines how a configuration manager should work.
    /// </summary>
    public interface IConfigurationManager
    {
        /// <summary>
        /// List of all the configurations this manager handles.
        /// </summary>
        List<IConfiguration> Configurations { get; set; }

        /// <summary>
        /// Event raised when a configuration is updated through this manager.
        /// TODO: This forces listeners to cast the configuration to see which one was changed. Is there another way to handle this?
        /// </summary>
        Action<ConfigurationData> ConfigurationUpdated { get; set; }

        /// <summary>
        /// Retrieves a certain configuration.
        /// </summary>
        /// <param name="configurationData">The configuration asked for.</param>
        /// <typeparam name="TConfigurationData">The type of configuration to retrieve.</typeparam>
        /// <returns>True if it was successful.</returns>
        bool GetConfiguration<TConfigurationData>(out TConfigurationData configurationData)
            where TConfigurationData : ConfigurationData;
        
        /// <summary>
        /// Retrieves the default values of a certain configuration.
        /// </summary>
        /// <param name="configurationData">The configuration asked for.</param>
        /// <typeparam name="TConfigurationData">The type of configuration to retrieve.</typeparam>
        /// <returns>True if it was successful.</returns>
        bool GetDefaultConfiguration<TConfigurationData>(out TConfigurationData configurationData)
            where TConfigurationData : ConfigurationData;

        /// <summary>
        /// Sets a new configuration of the given type.
        /// </summary>
        /// <param name="configurationData">The new configuration values.</param>
        /// <typeparam name="TConfigurationData">The type of configuration to set.</typeparam>
        /// <returns>True if it was successful.</returns>
        bool SetConfiguration<TConfigurationData>(TConfigurationData configurationData)
            where TConfigurationData : ConfigurationData;
    }
}