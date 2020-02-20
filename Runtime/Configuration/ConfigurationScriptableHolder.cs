using System.Collections.Generic;
using log4net;
using WhateverDevs.Core.Runtime.Persistence;
using UnityEngine;
using Zenject;
#if ODIN_INSPECTOR
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
            get => ConfigData ?? (ConfigData = new TConfigurationData());
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

        #if ODIN_INSPECTOR

        #region OdinButtons

        /// <summary>
        /// Check if the application is playing.
        /// </summary>
        private static bool AppPlaying => Application.isPlaying;

        /// <summary>
        /// Check if the application is not playing.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private bool AppNotPlaying => !AppPlaying;

        /// <summary>
        /// Check if the persisters have been injected.
        /// </summary>
        private bool PersistersNotInjected => Persisters == null;

        /// <summary>
        /// Check if the persisters have been injected.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private bool PersistersInjected => !PersistersNotInjected;

        /// <summary>
        /// Method to be able to save on editor using Odin.
        /// </summary>
        /// <param name="persisterScriptables">List of persisters to use.</param>
        [HideIf("AppPlaying")]
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
        [HideIf("AppPlaying")]
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
        [HideIf("AppNotPlaying")]
        [HideIf("PersistersNotInjected")]
        [Button]
        private void SaveWithInjectedPersisters() => Save();

        /// <summary>
        /// Method to able to load on runtime editor using Odin.
        /// </summary>
        [HideIf("AppNotPlaying")]
        [HideIf("PersistersNotInjected")]
        [Button]
        private void LoadWithInjectedPersisters() => Load();

        /// <summary>
        /// This one is just to show the infobox.
        /// </summary>
        [SerializeField]
        [ReadOnly]
        [HideIf("AppNotPlaying")]
        [HideIf("PersistersInjected")]
        [InfoBox("Persisters are not injected!")]
        // ReSharper disable once NotAccessedField.Local
        #pragma warning disable 0414
        private bool SavingAndLoadingDisabled = true;
        #pragma warning restore 0414

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

        #endif
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