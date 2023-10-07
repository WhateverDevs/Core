namespace WhateverDevs.Core.Runtime.Configuration
{
    /// <summary>
    /// Base class for configuration holders that will be accessible in Unity as Scriptable Objects.
    /// This class loads the configuration using the first persister that finds a valid config.
    /// </summary>
    /// <typeparam name="TConfigurationData">Type of configuration the holder will, well, hold.</typeparam>
    public abstract class
        ConfigurationScriptableHolderUsingFirstValidPersister<TConfigurationData> : ConfigurationScriptableHolder<
            TConfigurationData> where TConfigurationData : ConfigurationData, new()
    {
        /// <summary>
        /// Load the data using the persisters.
        /// </summary>
        /// <returns>True if it was successful.</returns>
        public override bool Load()
        {
            TConfigurationData defaultData = ConfigData;

            for (int i = 0; i < Persisters.Count; ++i)
                if (Persisters[i].Load(out ConfigData, ConfigurationName, true))
                    return true;

            // Use the default if all persisters fail.
            ConfigData = defaultData;

            return false;
        }
    }
}