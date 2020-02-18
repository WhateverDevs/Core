using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace WhateverDevs.Core.Runtime.Logger.Editor
{
    public class LoggerWindow : EditorWindow
    {
        [MenuItem("Window/UIElements/LoggerWindow")]
        public static void ShowExample()
        {
            LoggerWindow wnd = GetWindow<LoggerWindow>();
            wnd.titleContent = new GUIContent("LoggerWindow");
        }

        public void OnEnable()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/whateverdevs.core/Runtime/Logger/Editor/LoggerWindow.uxml");
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Packages/whateverdevs.core/Runtime/Logger/Editor/LoggerWindow.uss");
            root.styleSheets.Add(styleSheet);
            VisualElement xmlTree = visualTree.CloneTree();
            root.Add(xmlTree);
        
            xmlTree.Q<Button>(name = "OpenURLButton").clickable.clicked += URLButton;

        }

        private void URLButton()
        {
        
        }
    }
}