using System.Collections.Generic;
using Packages.Core.Runtime.Serialization;
using UnityEngine;
using Zenject;

namespace WhateverDevs.Core.Runtime.Configuration
{
    /// <summary>
    /// Base class for configuration holders that will be accessible in Unity as Scriptable Objects.
    /// </summary>
    /// <typeparam name="TConfigurationData">Type of configuration the holder will, well, hold.</typeparam>
    public abstract class
        ConfigurationScriptableHolder<TConfigurationData> : ScriptableObject, IConfiguration<TConfigurationData>
        where TConfigurationData : ConfigurationData, new()
    {
        /// <summary>
        /// Name the configuration will have.
        /// </summary>
        public string ConfigurationName;

        /// <summary>
        /// List of serializers the holder will use.
        /// Injecting lists: https://github.com/svermeulen/Extenject#list-bindings
        /// </summary>
        [Inject]
        public List<ISerializer> Serializers { get; set; }

        /// <summary>
        /// Data this configuration will hold.
        /// </summary>
        public TConfigurationData ConfigurationData
        {
            get => ProtectedConfigurationData ?? (ProtectedConfigurationData = new TConfigurationData());
            set => ProtectedConfigurationData = value;
        }

        /// <summary>
        /// Backfield for ConfigurationData.
        /// </summary>
        protected TConfigurationData ProtectedConfigurationData;

        /// <summary>
        /// Save the data using the persistent serializers.
        /// </summary>
        /// <returns>True if it was successful.</returns>
        public bool Save()
        {
            bool success = true;

            for (int i = 0; i < Serializers.Count; ++i)
                if (!Serializers[i].Save(ConfigurationData, ConfigurationName))
                    success = false;

            return success;
        }

        /// <summary>
        /// Load the data using the persistent serializers.
        /// Priority between serializers to be implemented by children.
        /// </summary>
        /// <returns>True if it was successful.</returns>
        public abstract bool Load();
    }
}