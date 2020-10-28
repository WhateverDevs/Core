using log4net;
using UnityEngine;
using WhateverDevs.Core.Runtime.Logger;

namespace WhateverDevs.Core.Runtime.Common
{
    /// <summary>
    /// This is just the same as Loggable, but this one inherits from StateMachineBehaviour.
    /// </summary>
    /// <typeparam name="TLoggable"></typeparam>
    public abstract class LoggableStateMachineBehaviour<TLoggable> : StateMachineBehaviour, ILoggable
        where TLoggable : LoggableStateMachineBehaviour<TLoggable>
    {
        /// <summary>
        /// Backfield for GetLogger.
        /// </summary>
        private ILog logger;
        
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
            return logger ?? (logger = LogManager.GetLogger(typeof(TLoggable)));
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
            return staticLogger ?? (staticLogger = LogManager.GetLogger(typeof(TLoggable)));
        }
    }
}