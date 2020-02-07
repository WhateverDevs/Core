using System.Collections.Generic;
using WhateverDevs.Core.Runtime.Serialization;

namespace WhateverDevs.Core.Runtime.Configuration
{
    /// <summary>
    /// Interface that configuration holders must implement.
    /// </summary>
    /// <typeparam name="TConfigurationData">Type of the configuration data to use.</typeparam>
    public interface IConfiguration<TConfigurationData> : IConfiguration where TConfigurationData : ConfigurationData
    {
        /// <summary>
        /// Serializers this configuration will use to persist.
        /// Priority on which serializers to use on load should be implemented by children.
        /// </summary>
        List<ISerializer> Serializers { get; set; }
        
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
        /// Save the data using the persistent serializers.
        /// </summary>
        /// <returns>True if it was successful.</returns>
        bool Save();

        /// <summary>
        /// Load the data using the persistent serializers.
        /// </summary>
        /// <returns>True if it was successful.</returns>
        bool Load();
    }
}