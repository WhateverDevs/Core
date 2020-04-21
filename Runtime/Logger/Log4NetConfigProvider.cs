using System.IO;
using UnityEngine;

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
    }
}