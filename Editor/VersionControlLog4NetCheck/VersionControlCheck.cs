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
        /// Object to store listing packages request.
        /// </summary>
        private static readonly ListRequest listRequest;

        /// <summary>
        /// Check if the version control package is installed, the version and suggest to reboot.
        /// </summary>
        static VersionControlCheck()
        {
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

                if (packageInfo.version == packageInfo.versions.latestCompatible) continue;

                if (EditorUtility.DisplayDialog("Whatever Dev's Core",
                                                "The core needs to update the Version Control package to compile. If you still get assembly errors after updating it, restart Unity.",
                                                "Update it",
                                                "I'll do it myself"))
                    Client.Add(packageInfo.name + "@" + packageInfo.versions.latestCompatible);
            }
        }
    }
}