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
        /// Recommended package version.
        /// </summary>
        private const string RecommendedVersion = "1.15.9";

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
        private static readonly ListRequest ListRequest;

        /// <summary>
        /// Check if the version control package is installed, the version and suggest to reboot.
        /// </summary>
        static VersionControlCheck()
        {
            CheckSettings();

            // ReSharper disable once PossibleNullReferenceException
            if (settings.CheckDismissed) return;
            
            ListRequest = Client.List();
            EditorApplication.update += OnVersionCheckFinished;
        }

        /// <summary>
        /// Called when the version checking is finished.
        /// </summary>
        private static void OnVersionCheckFinished()
        {
            if (!ListRequest.IsCompleted) return;

            EditorApplication.update -= OnVersionCheckFinished;

            if (ListRequest.Status == StatusCode.Failure)
            {
                Debug.LogError("Error listing packages.");
                return;
            }

            foreach (PackageInfo packageInfo in ListRequest.Result)
            {
                if (packageInfo.name != PackageName) continue;

                if (packageInfo.version.Equals(RecommendedVersion)) continue;

                if (EditorUtility.DisplayDialog("Whatever Dev's Core",
                                                "The core needs to update the Version Control package to version 1.15.9 compile. If you still get assembly errors after updating it, restart Unity.",
                                                "Update",
                                                "I'll do it myself"))
                    Client.Add(packageInfo.name + "@" + RecommendedVersion);
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