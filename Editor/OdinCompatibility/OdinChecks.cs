using System.IO;
using UnityEditor;
using UnityEngine;
using WhateverDevs.Core.Editor.Utils;

#if ODIN_INSPECTOR_3
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.Modules;
using System.Linq;
#endif

namespace WhateverDevs.Core.Editor.OdinCompatibility
{
    /// <summary>
    /// Class to make sure odin is installed and working correctly.
    /// </summary>
    public class OdinChecks
    {
        /// <summary>
        /// Path to data.
        /// </summary>
        private const string DataFolder = "Assets/Data/";

        /// <summary>
        /// Path to odin data.
        /// </summary>
        private const string OdinFolder = DataFolder + "Odin/";

        /// <summary>
        /// Path to editor odin data.
        /// </summary>
        private const string OdinEditorFolder = OdinFolder + "Editor/";

        /// <summary>
        /// Path to 
        /// </summary>
        private const string OdinSettingsPath = OdinEditorFolder + "OdinSettings.asset";

        /// <summary>
        /// Title for all window popups.
        /// </summary>
        private const string WindowTitle = "Whatever Dev's Core";

        /// <summary>
        /// Odin settings reference.
        /// </summary>
        private static OdinSettings odinSettings;

        /// <summary>
        /// Flag to know if the project has been imported and initialized.
        /// </summary>
        private static bool initialized;

        /// <summary>
        /// Check all odin compatibility.
        /// </summary>
        [InitializeOnLoadMethod]
        private static void CheckOdin()
        {
            CheckSettings();

            if (!initialized) return;

            bool odinFound = false;

            #if ODIN_INSPECTOR_3
            // ReSharper disable PossibleNullReferenceException
            if (odinSettings.OdinModuleConfig == null)
            {
                EditorUtility.DisplayProgressBar(WindowTitle, "Searching for Odin...", .4f);

                odinSettings.OdinModuleConfig =
                    AssetManagementUtils.FindAssetsByType<OdinModuleConfig>().FirstOrDefault();
            }

            if (odinSettings.OdinModuleConfig != null) odinFound = true;

            if (odinFound)
                if (!EditorOnlyModeConfig.Instance.IsEditorOnlyModeEnabled())
                    if (EditorUtility.DisplayDialog(WindowTitle,
                                                    "Odin need to be set in editor only mode to be compatible with Extenject.",
                                                    "Fix",
                                                    "I'll do it myself"))
                        EditorOnlyModeConfig.Instance.EnableEditorOnlyMode(false);

            #endif

            if (odinFound) return;

            if (odinSettings.AcknowledgedOdinNeed) return;

            if (!EditorUtility.DisplayDialog(WindowTitle,
                                             "Odin inspector is not enabled. While the core compiles, it is NOT DESIGNED to work without Odin. Please consider buying it on the Asset Store.",
                                             "Acknowledge"))
                return;

            odinSettings.AcknowledgedOdinNeed = true;
            AssetDatabase.SaveAssets();

            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Check if the settings exist or create them.
        /// </summary>
        private static void CheckSettings()
        {
            AssetManagementUtils.CreateFolderIfDoesNotExists(DataFolder);
            AssetManagementUtils.CreateFolderIfDoesNotExists(OdinFolder);
            AssetManagementUtils.CreateFolderIfDoesNotExists(OdinEditorFolder);

            odinSettings = AssetDatabase.LoadAssetAtPath<OdinSettings>(OdinSettingsPath);

            if (odinSettings != null)
            {
                initialized = true;
                return;
            }

            if (File.Exists(OdinSettingsPath))
            {
                initialized = false;
                return;
            }

            initialized = true;

            Debug.Log("#WhateverDevs Core #Generating settings for Odin compatibility.");

            odinSettings = ScriptableObject.CreateInstance<OdinSettings>();
            AssetDatabase.CreateAsset(odinSettings, OdinSettingsPath);
            AssetDatabase.SaveAssets();
        }
    }
}