using UnityEditor;
using UnityEngine;
using Varguiniano.ExtendedEditor.Editor;
using WhateverDevs.Core.Runtime.Configuration;

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

            // TODO: This won't work on editor time because the references to the persisters are not injected.
            EditorGUI.BeginDisabledGroup(!Application.isPlaying);
            if (GUILayout.Button("Save")) TargetObject.Save();
            if (GUILayout.Button("Load")) TargetObject.Load();
            EditorGUI.EndDisabledGroup();
        }
    }
}