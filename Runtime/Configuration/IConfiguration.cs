using System;
using System.Collections.Generic;
using WhateverDevs.Core.Runtime.Persistence;

namespace WhateverDevs.Core.Runtime.Configuration
{
    /// <summary>
    /// Interface that configuration holders must implement.
    /// </summary>
    /// <typeparam name="TConfigurationData">Type of the configuration data to use.</typeparam>
    public interface IConfiguration<TConfigurationData> : IConfiguration where TConfigurationData : ConfigurationData
    {
        /// <summary>
        /// Persisters this configuration will use to persist.
        /// Priority on which persisters to use on load should be implemented by children.
        /// </summary>
        List<IPersister> Persisters { get; set; }

        /// <summary>
        /// Data this configuration will hold.
        /// </summary>
        TConfigurationData ConfigurationData { get; set; }
    }

    /// <summary>
    /// Interface that configuration holders must implement.
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// Retrieve the type of configuration used.
        /// </summary>
        /// <returns></returns>
        Type GetConfigurationType();

        /// <summary>
        /// Retrieve the configuration for this holder or null if it doesn't match.
        /// </summary>
        /// <typeparam name="TConfigurationData">The type of configuration to retrieve.</typeparam>
        /// <returns>Either the configuration or null if it doesn't match.</returns>
        TConfigurationData UnsafeRetrieveConfiguration<TConfigurationData>()
            where TConfigurationData : ConfigurationData;

        /// <summary>
        /// Set the configuration for this holder or null if it doesn't match.
        /// </summary>
        /// <typeparam name="TConfigurationData">The type of configuration to set.</typeparam>
        /// <returns>True if it could save it, false if it couldn't or it can't be cast.</returns>
        bool UnsafeSetConfiguration<TConfigurationData>(TConfigurationData newConfiguration)
            where TConfigurationData : ConfigurationData;

        /// <summary>
        /// Save the data using the persistent persisters.
        /// </summary>
        /// <returns>True if it was successful.</returns>
        bool Save();

        /// <summary>
        /// Load the data using the persistent persisters.
        /// </summary>
        /// <returns>True if it was successful.</returns>
        bool Load();
    }
}