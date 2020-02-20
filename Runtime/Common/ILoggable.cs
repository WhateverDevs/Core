using log4net;

namespace WhateverDevs.Core.Runtime.Common
{
    /// <summary>
    /// Interface for classes that should use a logger.
    /// </summary>
    public interface ILoggable
    {
        /// <summary>
        /// Method that retrieves the logger for that class.
        /// </summary>
        ILog GetLogger();
    }
}