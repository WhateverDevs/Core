namespace WhateverDevs.Core.Runtime.Configuration
{
    /// <summary>
    /// Base class for configuration holders that will be accessible in Unity as Scriptable Objects.
    /// This class loads the configuration using the first serializer that finds a valid config.
    /// </summary>
    /// <typeparam name="TConfigurationData">Type of configuration the holder will, well, hold.</typeparam>
    public abstract class
        ConfigurationScriptableHolderUsingFirstValidSerializer<TConfigurationData> : ConfigurationScriptableHolder<
            TConfigurationData> where TConfigurationData : ConfigurationData, new()
    {
        /// <summary>
        /// Load the data using the persistent serializers.
        /// Priority between serializers to be implemented by children.
        /// </summary>
        /// <returns>True if it was successful.</returns>
        public override bool Load()
        {
            for (int i = 0; i < Serializers.Count; ++i)
                if (Serializers[i].Load(out ConfigData, ConfigurationName))
                    return true;

            return false;
        }
    }
}