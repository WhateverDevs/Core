using log4net;

namespace WhateverDevs.Core.Runtime.Common
{
    /// <summary>
    /// Class that has a logger attached.
    /// </summary>
    /// <typeparam name="TLoggable"></typeparam>
    public class Loggable<TLoggable> : ILoggable where TLoggable : Loggable<TLoggable>
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
        public ILog GetLogger() => logger ?? (logger = LogManager.GetLogger(typeof(TLoggable)));
        
        /// <summary>
        /// Get the logger for this class.
        /// </summary>
        /// <returns></returns>
        public static ILog GetStaticLogger() => staticLogger ?? (staticLogger = LogManager.GetLogger(typeof(TLoggable)));
    }
}