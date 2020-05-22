using log4net;
using UnityEngine;

namespace WhateverDevs.Core.Runtime.Common
{
    /// <summary>
    /// This is just the same as Loggable, but this one inherits from MonoBehaviour.
    /// </summary>
    /// <typeparam name="TLoggable"></typeparam>
    public abstract class LoggableMonoBehaviour<TLoggable> : MonoBehaviour, ILoggable
        where TLoggable : LoggableMonoBehaviour<TLoggable>
    {
        /// <summary>
        /// Backfield for GetLogger.
        /// </summary>
        private ILog logger;

        /// <summary>
        /// Get the logger for this class.
        /// </summary>
        /// <returns></returns>
        public ILog GetLogger() => logger ?? (logger = LogManager.GetLogger(typeof(TLoggable)));
    }
}