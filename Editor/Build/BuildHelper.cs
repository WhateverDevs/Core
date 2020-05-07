using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;
using WhateverDevs.Core.Runtime.Common;
using Version = WhateverDevs.Core.Runtime.Build.Version;

namespace Varguiniano.Core.Editor.Build
{
    /// <summary>
    /// Helper class with functions related to builds, scene management, etc.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class BuildHelper : Loggable<BuildHelper>, IPreprocessBuildWithReport
    {
        /// <summary>
        /// Callback order for preprocessing the build.
        /// </summary>
        public int callbackOrder => 0;

        /// <summary>
        /// Path to the version asset.
        /// </summary>
        private const string VersionPath = "Assets/Data/Version.asset";

        /// <summary>
        /// Current date in string version format.
        /// </summary>
        private static string CurrentDate => DateTime.Now.ToString("yyyyMMddhhmmss");

        /// <summary>
        /// Operations to be performed before build.
        /// </summary>
        /// <param name="report">Report to add things to.</param>
        public void OnPreprocessBuild(BuildReport report)
        {
            bool buildSuccessful = GenerateVersion();

            if (buildSuccessful)
                GetLogger()
                   .Info("Successfully preprocessed build.");
            else
                GetLogger()
                   .Error("There was an error preprocessing the build, check the console.");
        }

        /// <summary>
        /// Generate a new version for the game.
        /// </summary>
        /// <returns></returns>
        private bool GenerateVersion()
        {
            Version version = AssetDatabase.LoadAssetAtPath<Version>(VersionPath);

            if (version == null)
            {
                if (EditorUtility.DisplayDialog("WhateverDev's Core",
                                                "No version asset found on the data folder. Do you want to automatically create one?",
                                                "Sure!",
                                                "I don't need version info (yeah, sure)"))
                {
                    if (!Directory.Exists("Assets/Data")) Directory.CreateDirectory("Assets/Data");

                    Version newVersionAsset = ScriptableObject.CreateInstance<Version>();
                    AssetDatabase.CreateAsset(newVersionAsset, VersionPath);
                    AssetDatabase.SaveAssets();

                    version = newVersionAsset;
                }
                else
                {
                    GetLogger()
                       .Error("No version found at "
                            + VersionPath
                            + ". Don't you want a version shipped with your game?");

                    return false;
                }
            }

            int newMinorVersion = version.MinorVersion + 1;
            string now = CurrentDate;

            string newVersion = new StringBuilder(version.GameVersion.ToString()).Append(".")
                                                                                 .Append(version.MayorVersion)
                                                                                 .Append(".")
                                                                                 .Append(newMinorVersion)
                                                                                 .Append(Version
                                                                                            .StabilityToString(version
                                                                                                                  .Stability))
                                                                                 .Append(".")
                                                                                 .Append(now)
                                                                                 .ToString();

            bool useNewVersion = EditorUtility.DisplayDialog("Game version",
                                                             "Shifting from "
                                                           + version.FullVersion
                                                           + " to "
                                                           + newVersion
                                                           + ". NOTE: To change mayor versions, do it manually on the version asset.",
                                                             "Shift version",
                                                             "Leave previous minor version");

            if (!useNewVersion) return true;
            version.MinorVersion = newMinorVersion;
            version.Date = now;
            EditorUtility.SetDirty(version);
            AssetDatabase.SaveAssets();

            GetLogger().Info("New version asset generated.");

            return true;
        }

        /// <summary>
        /// Operations to be performed after build.
        /// </summary>
        /// <param name="target">Build target built for.</param>
        /// <param name="pathToBuiltProject">Path to the executable/apk/etc.</param>
        [PostProcessBuild(0)] // First to be called after build.
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            bool buildSuccessful = true;
            string buildFolder = Directory.GetParent(pathToBuiltProject).FullName;

            if (target == BuildTarget.StandaloneWindows
             || target == BuildTarget.StandaloneWindows64
             || target == BuildTarget.StandaloneLinux64
             || target == BuildTarget.StandaloneOSX)
                try
                {
                    CopyConfigFolder(buildFolder);
                }
                catch (Exception e)
                {
                    buildSuccessful = false;
                    GetStaticLogger().Error("Exception!", e);
                }
            else
                GetStaticLogger().Info("Target is not standalone, skipping config copy.");

            if (buildSuccessful)
                GetStaticLogger().Info("Successfully postprocessed build in " + buildFolder + ".");
            else
                GetStaticLogger().Error("There was an error postprocessing the build, check the console.");
        }

        /// <summary>
        /// Copies the configuration folder to the build if it exists.
        /// </summary>
        /// <param name="buildPath">The path of the build.</param>
        private static void CopyConfigFolder(string buildPath)
        {
            if (Directory.Exists(buildPath + "/Configuration")) Utils.DeleteDirectory(buildPath + "/Configuration");

            if (Directory.Exists("Configuration"))
                GetStaticLogger().Info("Configuration folder found, copying it to build.");
            else
            {
                GetStaticLogger().Info("No configuration folder found, skipping config copy.");
                return;
            }

            Utils.CopyFilesRecursively(new DirectoryInfo("Configuration"),
                                       new DirectoryInfo(buildPath + "/Configuration"));
        }
    }
}