using System;

namespace WhateverDevs.Core.Runtime.Configuration
{
    /// <summary>
    /// Base class for configuration data.
    /// </summary>
    [Serializable]
    public abstract class ConfigurationData
    {
        /// <summary>
        /// Clone this data into a new instance of the same type.
        /// </summary>
        /// <typeparam name="TConfigurationData">Type of the cloned configuration.</typeparam>
        /// <returns>The cloned object.</returns>
        protected internal abstract TConfigurationData Clone<TConfigurationData>()
            where TConfigurationData : ConfigurationData, new();
    }
}