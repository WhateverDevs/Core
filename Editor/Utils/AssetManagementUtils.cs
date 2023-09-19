using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.SceneManagement;
using UnityEngine;
using WhateverDevs.Core.Runtime.Common;
using Object = UnityEngine.Object;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace WhateverDevs.Core.Editor.Utils
{
    /// <summary>
    /// Class with utils for asset management.
    /// </summary>
    public class AssetManagementUtils : Loggable<AssetManagementUtils>
    {
        /// <summary>
        /// Clean all the editor caches and restart the editor.
        /// </summary>
        [MenuItem("WhateverDevs/CleanCachesAndRestart", priority = 0)]
        public static void CleanAndRestart()
        {
            UpdatePowershellScripts();

            if (!EditorUtility.DisplayDialog("Clean caches and restart",
                                             "This will force reimporting the project, make sure you have saved everything first.",
                                             "Continue",
                                             "Cancel"))
                return;

            string arguments = "-ExecutionPolicy Bypass -File WhateverDevsScripts/CleanUpAndRestart.ps1 \""
                             + EditorApplication.applicationPath
                             + "\"";

            StaticLogger.Info(arguments);

            ProcessStartInfo startInfo =
                new ProcessStartInfo("powershell.exe", arguments)
                {
                    WorkingDirectory = Environment.CurrentDirectory,
                    UseShellExecute = true
                };

            Process process = new Process { StartInfo = startInfo };
            process.Start();
        }

        /// <summary>
        /// Find all components that reference a missing script in the project.
        /// </summary>
        [MenuItem("WhateverDevs/Asset Management/Find all missing script components")]
        // ReSharper disable once FunctionComplexityOverflow
        private static void FindAllMissingScripts()
        {
            EditorSceneManager.SaveOpenScenes();

            // ReSharper disable once AccessToStaticMemberViaDerivedType
            string previousScene = EditorSceneManager.GetActiveScene().path;

            try
            {
                bool foundMissingInPrefabs = false;
                EditorUtility.DisplayProgressBar("Looking for missing scripts", "Checking prefabs", .333f);

                foreach (GameObject prefab in GetAllPrefabs().Select(AssetDatabase.LoadAssetAtPath<GameObject>))
                {
                    if (prefab == null) return;
                    List<Component> componentsToCheck = prefab.GetComponents<Component>().ToList();
                    componentsToCheck.AddRange(prefab.GetComponentsInChildren<Component>());

                    foreach (Component _ in componentsToCheck.Where(component => component == null))
                    {
                        foundMissingInPrefabs = true;
                        StaticLogger.Warn(prefab.name + " or one of its children has a missing script!");
                    }
                }

                if (!foundMissingInPrefabs) StaticLogger.Info("There are no missing scripts in the prefabs.");

                EditorUtility.DisplayProgressBar("Looking for missing scripts", "Checking scene objects", .666f);

                bool sceneLoaded = false;
                EditorSceneManager.sceneOpened += (sceneOpened, mode) => sceneLoaded = true;

                bool foundMissingInScene = false;

                foreach (string scenePath in GetAllScenes())
                {
                    SceneAsset scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
                    if (scene == null) continue;

                    // Check if the scene is in a readonly package and ignore it if it is.
                    PackageInfo packageInfo = PackageInfo.FindForAssetPath(scenePath);

                    if (packageInfo != null)
                        if (packageInfo.source != PackageSource.Embedded && packageInfo.source != PackageSource.Local)
                            continue;

                    // ReSharper disable once AccessToStaticMemberViaDerivedType
                    EditorSceneManager.OpenScene(scenePath);

                    while (!sceneLoaded)
                    {
                    }

                    sceneLoaded = false;

                    foreach (GameObject gameObject in Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None))
                    {
                        // Reflection probes are giving false positives for some reason.
                        if (gameObject.GetComponent<ReflectionProbe>() != null) continue;

                        foreach (Component component in gameObject.GetComponents<Component>())
                        {
                            if (component != null) continue;
                            foundMissingInScene = true;
                            StaticLogger.Warn(gameObject.name + " in scene " + scene.name + " has a missing script!");
                        }
                    }
                }

                if (!foundMissingInScene) StaticLogger.Info("There are no missing scripts in the scenes.");
            }
            finally
            {
                EditorUtility.DisplayProgressBar("Looking for missing scripts",
                                                 "Loading previously opened scene",
                                                 .999f);

                if (!previousScene.IsNullEmptyOrWhiteSpace()) EditorSceneManager.OpenScene(previousScene);

                EditorUtility.ClearProgressBar();
            }
        }

        /// <summary>
        /// Creates a folder if does not exist.
        /// </summary>
        /// <param name="path">Path to that folder.</param>
        public static void CreateFolderIfDoesNotExists(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Finds all assets of a certain type.
        /// </summary>
        /// <typeparam name="T">Type to search.</typeparam>
        /// <returns>A list of all assets with that type.</returns>
        public static List<T> FindAssetsByType<T>() where T : Object
        {
            List<T> assets = new();
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T)}");

            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

                if (asset != null) assets.Add(asset);
            }

            return assets;
        }

        /// <summary>
        /// Updates the powershell scripts folder.
        /// </summary>
        private static void UpdatePowershellScripts()
        {
            if (Directory.Exists("WhateverDevsScripts"))
                WhateverDevs.Core.Runtime.Common.Utils.DeleteDirectory("WhateverDevsScripts");

            Directory.CreateDirectory("WhateverDevsScripts");

            File.Copy("Packages/whateverdevs.core/PowershellScripts/CleanUpAndRestart.ps1",
                      "WhateverDevsScripts/CleanUpAndRestart.ps1");
        }

        /// <summary>
        /// Gets all the prefab paths in the project.
        /// </summary>
        /// <returns>A string array.</returns>
        private static IEnumerable<string> GetAllPrefabs() => GetAllAssetsWithExtension("prefab");

        /// <summary>
        /// Gets all the scene paths in the project.
        /// </summary>
        /// <returns>A string array.</returns>
        private static IEnumerable<string> GetAllScenes() => GetAllAssetsWithExtension("unity");

        /// <summary>
        /// Gets all the asset paths with the given extension.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns>A string array.</returns>
        private static IEnumerable<string> GetAllAssetsWithExtension(string extension) =>
            AssetDatabase.GetAllAssetPaths().Where(s => s.EndsWith("." + extension)).ToArray();
    }
}