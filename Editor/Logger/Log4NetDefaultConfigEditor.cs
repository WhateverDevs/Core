using System.IO;
using UnityEditor;
using UnityEngine;
using Varguiniano.ExtendedEditor.Editor;
using WhateverDevs.Core.Runtime.Logger;

namespace WhateverDevs.Core.Editor.Logger
{
    /// <summary>
    /// Custom editor for the Log4Net default config.
    /// </summary>
    [CustomEditor(typeof(Log4NetDefaultConfig))]
    public class Log4NetDefaultConfigEditor : ScriptableExtendedEditor<Log4NetDefaultConfig>
    {
        /// <summary>
        /// Paint the UI.
        /// </summary>
        protected override void PaintUi()
        {
            EditorGUILayout
               .HelpBox("This stores the contents from the log4net.xml file. To edit the configuration, change that file and the load it here.",
                        MessageType.Info);

            if (GUILayout.Button("Load new config"))
                TargetObject.DefaultConfig =
                    File.ReadAllText("./Packages/whateverdevs.core/Runtime/Logger/Data/log4net.xml");
            else
            {
                EditorGUI.BeginDisabledGroup(true);

                EditorGUILayout.TextArea(TargetObject.DefaultConfig);

                EditorGUI.EndDisabledGroup();
            }
        }
    }
}