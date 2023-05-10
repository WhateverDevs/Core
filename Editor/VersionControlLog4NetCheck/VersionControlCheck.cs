using System.IO;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace WhateverDevs.Core.Editor.VersionControlLog4NetCheck
{
    /// <summary>
    /// Checks if the correct version of the version control plugin is installed.
    /// </summary>
    [InitializeOnLoad]
    public class VersionControlCheck
    {
        /// <summary>
        /// Package name.
        /// </summary>
        private const string PackageName = "com.unity.collab-proxy";

        /// <summary>
        /// Path to data.
        /// </summary>
        private const string DataFolder = "Assets/Data/";

        /// <summary>
        /// Path to VersionControl data.
        /// </summary>
        private const string VersionControlFolder = DataFolder + "VersionControl/";

        /// <summary>
        /// Path to editor VersionControl data.
        /// </summary>
        private const string VersionControlEditorFolder = VersionControlFolder + "Editor/";

        /// <summary>
        /// Path to settings file.
        /// </summary>
        private const string SettingsPath = VersionControlEditorFolder + "VersionControlSettings.asset";

        /// <summary>
        /// Reference to the settings.
        /// </summary>
        private static VersionControlPackageCheckConfig settings;

        /// <summary>
        /// Object to store listing packages request.
        /// </summary>
        private static ListRequest listRequest;

        /// <summary>
        /// Check if the version control package is installed, the version and suggest to reboot.
        /// </summary>
        static VersionControlCheck() => EditorApplication.delayCall += Initialize;

        /// <summary>
        /// Initialize after the editor has been loaded.
        /// </summary>
        private static void Initialize()
        {
            CheckSettings();

            // ReSharper disable once PossibleNullReferenceException
            if (settings.CheckDismissed) return;

            listRequest = Client.List();
            EditorApplication.update += OnVersionCheckFinished;
        }

        /// <summary>
        /// Called when the version checking is finished.
        /// </summary>
        private static void OnVersionCheckFinished()
        {
            if (!listRequest.IsCompleted) return;

            EditorApplication.update -= OnVersionCheckFinished;

            if (listRequest.Status == StatusCode.Failure)
            {
                Debug.LogError("Error listing packages.");
                return;
            }

            foreach (PackageInfo packageInfo in listRequest.Result)
            {
                if (packageInfo.name != PackageName) continue;

                if (EditorUtility.DisplayDialog("Whatever Dev's Core",
                                                "The core needs to remove the Version Control package in other to work correctly. If you still get assembly errors after updating it, restart Unity.",
                                                "Fix",
                                                "I'll do it myself"))
                    Client.Remove(packageInfo.name);
                else
                {
                    settings.CheckDismissed = true;
                    AssetDatabase.SaveAssets();
                }
            }
        }

        /// <summary>
        /// Check if the settings exist or create them.
        /// </summary>
        private static void CheckSettings()
        {
            CreateFolderIfDoesNotExists(DataFolder);
            CreateFolderIfDoesNotExists(VersionControlFolder);
            CreateFolderIfDoesNotExists(VersionControlEditorFolder);

            settings = AssetDatabase.LoadAssetAtPath<VersionControlPackageCheckConfig>(SettingsPath);

            if (settings != null) return;
            settings = ScriptableObject.CreateInstance<VersionControlPackageCheckConfig>();
            AssetDatabase.CreateAsset(settings, SettingsPath);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// Creates a folder if does not exist.
        /// </summary>
        /// <param name="path">Path to that folder.</param>
        private static void CreateFolderIfDoesNotExists(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }
    }
}