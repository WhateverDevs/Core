using log4net;
using WhateverDevs.Core.Runtime.Logger;

namespace WhateverDevs.Core.Runtime.Common
{
    /// <summary>
    /// Class that has a logger attached.
    /// </summary>
    /// <typeparam name="TLoggable"></typeparam>
    public abstract class Loggable<TLoggable> : ILoggable where TLoggable : Loggable<TLoggable>
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