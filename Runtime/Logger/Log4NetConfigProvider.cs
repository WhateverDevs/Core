using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace WhateverDevs.Core.Runtime.Logger
{
    /// <summary>
    /// Class that is used to get the configuration for Log4Net.
    /// </summary>
    public static class Log4NetConfigProvider
    {
        /// <summary>
        /// Does the configuration exist?
        /// </summary>
        public static bool ConfigExists => Resources.Load<Log4NetConfiguration>("LoggerConfiguration") != null;

        /// <summary>
        /// Is the default configuration referenced?
        /// </summary>
        public static bool DefaultConfigSet =>
            Resources.Load<Log4NetConfiguration>("LoggerConfiguration").DefaultConfig != null;

        /// <summary>
        /// Does the configuration exist on the persistent data path?
        /// </summary>
        private static bool ConfigExistsInPersistent => File.Exists(Application.persistentDataPath + "/log4net.xml");

        /// <summary>
        /// Retrieves the config for the logging system.
        /// </summary>
        /// <param name="configOriginMessage">A string that says where the config came from.</param>
        /// <returns>The config file info object.</returns>
        public static FileInfo GetConfig(out string configOriginMessage)
        {
            Log4NetConfiguration config = Resources.Load<Log4NetConfiguration>("LoggerConfiguration");

            if (config.AlwaysOverride || !ConfigExistsInPersistent)
            {
                string configString = config.DefaultConfig.DefaultConfig;

                File.WriteAllText(Application.persistentDataPath + "/log4net.xml", configString);

                configOriginMessage = "Loaded default logger configuration from resources.";
            }
            else
                configOriginMessage = "Loaded logger configuration from persistent path.";

            FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/log4net.xml");

            return fileInfo;
        }

        #if UNITY_EDITOR

        /// <summary>
        /// Retrieves the default config from the package.
        /// </summary>
        /// <returns></returns>
        public static Log4NetDefaultConfig GetDefaultConfig() =>
            AssetDatabase
               .LoadAssetAtPath<Log4NetDefaultConfig
                >("Packages/whateverdevs.core/Runtime/Logger/Data/Log4NetDefaultConfig.asset");

        /// <summary>
        /// Sets the reference to the default logger configuration.
        /// </summary>
        public static void FixDefaultLoggerReference()
        {
            Log4NetConfiguration config = Resources.Load<Log4NetConfiguration>("LoggerConfiguration");
            config.DefaultConfig = GetDefaultConfig();
            EditorUtility.SetDirty(config);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// Create the logger configuration scriptable in the resources folder.
        /// </summary>
        public static void CreateLoggerConfigInResources()
        {
            if (!Directory.Exists("Assets/Resources")) Directory.CreateDirectory("Assets/Resources");

            Log4NetConfiguration config = ScriptableObject.CreateInstance<Log4NetConfiguration>();
            config.DefaultConfig = GetDefaultConfig();
            AssetDatabase.CreateAsset(config, "Assets/Resources/LoggerConfiguration.asset");
            AssetDatabase.SaveAssets();
        }
        #endif
    }
}