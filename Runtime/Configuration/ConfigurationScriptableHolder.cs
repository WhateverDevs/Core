using System;
using System.Collections.Generic;
using log4net;
using WhateverDevs.Core.Runtime.Persistence;
using UnityEngine;
using Zenject;
#if ODIN_INSPECTOR_3
using Sirenix.OdinInspector;
#endif

namespace WhateverDevs.Core.Runtime.Configuration
{
    /// <summary>
    /// Base class for configuration holders that will be accessible in Unity as Scriptable Objects.
    /// </summary>
    /// <typeparam name="TConfigurationData">Type of configuration the holder will, well, hold.</typeparam>
    public abstract class
        ConfigurationScriptableHolder<TConfigurationData> : ConfigurationScriptableHolder,
                                                            IConfiguration<TConfigurationData>
        where TConfigurationData : ConfigurationData, new()
    {
        /// <summary>
        /// Name the configuration will have.
        /// </summary>
        public string ConfigurationName;

        /// <summary>
        /// List of persisters the holder will use.
        /// Injecting lists: https://github.com/svermeulen/Extenject#list-bindings
        /// </summary>
        [Inject]
        public List<IPersister> Persisters { get; set; }

        /// <summary>
        /// Data this configuration will hold.
        /// </summary>
        public TConfigurationData ConfigurationData
        {
            get => ConfigData ??= new TConfigurationData();
            set => ConfigData = value;
        }

        /// <summary>
        /// Backfield for ConfigurationData.
        /// Also editable through inspector.
        /// </summary>
        [SerializeField]
        protected TConfigurationData ConfigData;
        
        /// <summary>
        /// Default data that will be used when no configuration file is found.
        /// </summary>
        [SerializeField]
        private TConfigurationData DefaultConfigurationData;

        /// <summary>
        /// Retrieve the type of configuration used.
        /// </summary>
        /// <returns></returns>
        public Type GetConfigurationType() => typeof(TConfigurationData);

        /// <summary>
        /// Retrieve the configuration for this holder or null if it doesn't match.
        /// </summary>
        /// <typeparam name="T">The type of configuration to retrieve.</typeparam>
        /// <returns>Either the configuration or null if it doesn't match.</returns>
        public T UnsafeRetrieveConfiguration<T>() where T : ConfigurationData
        {
            if (ConfigurationData is T data) return data;

            return null;
        }

        /// <summary>
        /// Set the configuration for this holder or null if it doesn't match.
        /// To be overriden if necessary by configuration holder extenders.
        /// </summary>
        /// <typeparam name="T">The type of configuration to set.</typeparam>
        /// <returns>True if it could save it, false if it couldn't or it can't be cast.</returns>
        public virtual bool UnsafeSetConfiguration<T>(T newConfiguration) where T : ConfigurationData
        {
            if (typeof(T) != typeof(TConfigurationData)) return false;
            ConfigurationData = newConfiguration as TConfigurationData;
            return true;
        }

        /// <summary>
        /// Save the data using the persistent persisters.
        /// </summary>
        /// <returns>True if it was successful.</returns>
        public bool Save()
        {
            bool success = true;

            for (int i = 0; i < Persisters.Count; ++i)
                if (!Persisters[i].Save(ConfigurationData, ConfigurationName))
                    success = false;

            return success;
        }

        /// <summary>
        /// Save the data using the persistent persisters.
        /// </summary>
        /// <returns>True if it was successful.</returns>
        public bool SaveDefault()
        {
            ConfigurationData = DefaultConfigurationData.Clone<TConfigurationData>();

            return Save();
        }

        /// <summary>
        /// Load the data using the persisters.
        /// Priority between persisters to be implemented by children.
        /// </summary>
        /// <returns>True if it was successful.</returns>
        public abstract bool Load();

        #region OdinButtons

        /// <summary>
        /// Method to be able to save on editor using Odin.
        /// </summary>
        /// <param name="persisterScriptables">List of persisters to use.</param>
        #if ODIN_INSPECTOR_3
        [HideInPlayMode]
        [Button(ButtonStyle.FoldoutButton)]
        #endif
        private void Save(PersisterScriptable[] persisterScriptables)
        {
            if (persisterScriptables == null)
            {
                Logger.Error("Assign some persister scriptables first!");
                return;
            }

            AssignPersisters(persisterScriptables);
            Save();
            UnAssignPersisters();
        }

        /// <summary>
        /// Method to able to load on editor using Odin.
        /// </summary>
        /// <param name="persisterScriptables">List of persisters to use.</param>
        #if ODIN_INSPECTOR_3
        [HideInPlayMode]
        [Button(ButtonStyle.FoldoutButton)]
        #endif
        private void Load(PersisterScriptable[] persisterScriptables)
        {
            if (persisterScriptables == null)
            {
                Logger.Error("Assign some persister scriptables first!");
                return;
            }

            AssignPersisters(persisterScriptables);
            Load();
            UnAssignPersisters();
        }

        /// <summary>
        /// Method to able to save on runtime editor using Odin.
        /// </summary>
        #if ODIN_INSPECTOR_3
        [HideInEditorMode]
        [Button]
        #endif
        private void SaveWithInjectedPersisters() => Save();

        /// <summary>
        /// Method to able to load on runtime editor using Odin.
        /// </summary>
        #if ODIN_INSPECTOR_3
        [HideInEditorMode]
        [Button]
        #endif
        private void LoadWithInjectedPersisters() => Load();

        /// <summary>
        /// Assign the persisters to the object.
        /// </summary>
        private void AssignPersisters(IReadOnlyList<PersisterScriptable> persisterScriptables)
        {
            Persisters = new List<IPersister>();

            for (int i = 0; i < persisterScriptables.Count; ++i) Persisters.Add(persisterScriptables[i].Persister);
        }

        /// <summary>
        /// Un assign the persisters from the object.
        /// </summary>
        private void UnAssignPersisters() => Persisters = null;

        #endregion
    }

    /// <summary>
    /// Non generic class for editor referencing.
    /// </summary>
    public class ConfigurationScriptableHolder : ScriptableObject
    {
        /// <summary>
        /// Logger to send messages to console.
        /// </summary>
        protected static readonly ILog Logger =
            LogManager.GetLogger(typeof(ConfigurationScriptableHolder));
    }
}