using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Varguiniano.ExtendedEditor.Editor;
using WhateverDevs.Core.Runtime.Configuration;
using WhateverDevs.Core.Runtime.Persistence;

namespace WhateverDevs.Core.Editor.Configuration
{
    /// <summary>
    /// Base editor for classes that inherit from ConfigurationScriptableHolder.
    /// </summary>
    public abstract class
        ConfigurationScriptableHolderEditor<TConfigurationScriptableHolder, TConfigurationData> :
            ScriptableExtendedEditor<TConfigurationScriptableHolder>
        where TConfigurationScriptableHolder : ConfigurationScriptableHolder<TConfigurationData>
        where TConfigurationData : ConfigurationData, new()
    {
        /// <summary>
        /// Paint the UI.
        /// </summary>
        protected override void PaintUi()
        {
            PaintProperty("ConfigurationName");

            PaintProperty("ConfigData", true);

            if (Application.isPlaying)
            {
                if (GUILayout.Button("Save")) TargetObject.Save();
                if (GUILayout.Button("Load")) TargetObject.Load();
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                {
                    EditorGUILayout.BeginVertical();

                    {
                        PaintProperty("PersisterScriptables", true);
                    }

                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical();

                    {
                        if (GUILayout.Button("Save"))
                        {
                            AssignPersisters();
                            TargetObject.Save();
                            UnAssignPersisters();
                        }

                        if (GUILayout.Button("Load"))
                        {
                            AssignPersisters();
                            TargetObject.Load();
                            UnAssignPersisters();
                        }
                    }

                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        /// <summary>
        /// Assign the persisters to the object.
        /// </summary>
        private void AssignPersisters()
        {
            TargetObject.Persisters = new List<IPersister>();

            for (int i = 0; i < TargetObject.PersisterScriptables.Length; ++i)
                TargetObject.Persisters.Add(TargetObject.PersisterScriptables[i].Persister);
        }
        
        /// <summary>
        /// Un assign the persisters from the object.
        /// </summary>
        private void UnAssignPersisters() => TargetObject.Persisters = null;
    }
}