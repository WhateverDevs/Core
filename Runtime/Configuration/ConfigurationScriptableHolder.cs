using System.Collections.Generic;
using log4net;
using WhateverDevs.Core.Runtime.Persistence;
using UnityEngine;
using Zenject;
using Sirenix.OdinInspector;

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
        [HideInPlayMode]
        [Button(ButtonStyle.FoldoutButton)]
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
        [HideInPlayMode]
        [Button(ButtonStyle.FoldoutButton)]
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
        [HideInEditorMode]
        [Button]
        private void SaveWithInjectedPersisters() => Save();

        /// <summary>
        /// Method to able to load on runtime editor using Odin.
        /// </summary>
        [HideInEditorMode]
        [Button]
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