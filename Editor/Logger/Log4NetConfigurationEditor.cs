using UnityEditor;
using UnityEngine;
using Varguiniano.ExtendedEditor.Editor;
using WhateverDevs.Core.Runtime.Logger;

namespace WhateverDevs.Core.Editor.Logger
{
    /// <summary>
    /// Custom editor for the log4net configuration.
    /// </summary>
    [CustomEditor(typeof(Log4NetConfiguration))]
    public class Log4NetConfigurationEditor : ScriptableExtendedEditor<Log4NetConfiguration>
    {
        /// <summary>
        /// Paint the UI.
        /// </summary>
        protected override void PaintUi()
        {
            if (TargetObject.DefaultConfig == null)
            {
                EditorGUILayout
                   .HelpBox("You need to reference the default config! You can use the button bellow to automatically find it.",
                            MessageType.Error);

                if (GUILayout.Button("Find default config"))
                    TargetObject.DefaultConfig = Log4NetConfigProvider.GetDefaultConfig();
            }
            else
                EditorGUILayout.HelpBox("Everything set up correctly, you can use the logger :)", MessageType.Info);

            PaintProperty("DefaultConfig");

            PaintProperty("AlwaysOverride");
        }
    }
}