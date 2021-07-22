using log4net;
using UnityEditor;
using WhateverDevs.Core.Runtime.Common;
using WhateverDevs.Core.Runtime.Logger;

namespace WhateverDevs.Core.Editor.Common
{
    /// <summary>
    /// Base loggable class for editor windows.
    /// </summary>
    public abstract class LoggableEditorWindow<TLoggable> : EditorWindow, ILoggable
        where TLoggable : LoggableEditorWindow<TLoggable>
    {
        /// <summary>
        /// Get the logger for this class.
        /// </summary>
        public ILog Logger => GetLogger();

        /// <summary>
        /// Backfield for GetLogger.
        /// </summary>
        private ILog logger;

        /// <summary>
        /// Get the static logger for this class.
        /// </summary>
        public static ILog StaticLogger => GetStaticLogger();

        /// <summary>
        /// Backfield for GetLogger.
        /// </summary>
        // ReSharper disable once StaticMemberInGenericType
        private static ILog staticLogger;

        /// <summary>
        /// Get the logger for this class.
        /// </summary>
        /// <returns></returns>
        public ILog GetLogger()
        {
            #if UNITY_EDITOR
            LogHandler.Initialize();
            #endif
            return logger ??= LogManager.GetLogger(typeof(TLoggable));
        }

        /// <summary>
        /// Get the logger for this class.
        /// </summary>
        /// <returns></returns>
        public static ILog GetStaticLogger()
        {
            #if UNITY_EDITOR
            LogHandler.Initialize();
            #endif
            return staticLogger ??= LogManager.GetLogger(typeof(TLoggable));
        }
    }
}