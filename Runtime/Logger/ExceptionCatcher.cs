using UnityEngine;
using WhateverDevs.Core.Runtime.Common;

namespace WhateverDevs.Core.Runtime.Logger
{
    /// <summary>
    /// Class that catches all exceptions the application throws and sends them to the custom logger system.
    /// </summary>
    public class ExceptionCatcher : Singleton<ExceptionCatcher>
    {
        /// <summary>
        /// Subscribe to the log callback.
        /// </summary>
        private void OnEnable()
        {
            Application.logMessageReceived += ExceptionCallBack;
            Logger.Debug("Global exception catching ready.");
        }

        /// <summary>
        /// Called whenever the unity system logs a message, but we will only use it for exceptions.
        /// </summary>
        /// <param name="condition">Message.</param>
        /// <param name="stacktrace">Stacktrace.</param>
        /// <param name="logType">Log type.</param>
        private static void ExceptionCallBack(string condition, string stacktrace, LogType logType)
        {
            if (logType == LogType.Exception || logType == LogType.Assert)
                StaticLogger.Fatal(condition + "\n" + stacktrace);
        }

        /// <summary>
        /// Unsubscribe from the log callback.
        /// </summary>
        private void OnDisable()
        {
            Application.logMessageReceived -= ExceptionCallBack;
            Logger.Debug("Global exception catching off.");
        }
    }
}