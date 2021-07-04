using log4net.Core;
using UnityEngine;

namespace WhateverDevs.Core.Runtime.Logger
{
    /// <summary>
    /// Class with helper utilities related to logging.
    /// </summary>
    public static class LoggerHelper
    {
        /// <summary>
        /// Convert a log4net level to unity log type.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static LogType ToLogType(this Level level)
        {
            if (level == Level.Off
             || level == Level.Notice
             || level == Level.Info
             || level == Level.Debug
             || level == Level.Fine
             || level == Level.Trace
             || level == Level.Finer
             || level == Level.Verbose
             || level == Level.All)
                return LogType.Log;

            if (level == Level.Warn) return LogType.Warning;

            if (level == Level.Error || level == Level.Severe || level == Level.Critical || level == Level.Alert)
                return LogType.Error;

            if (level == Level.Fatal || level == Level.Emergency) return LogType.Exception;

            return LogType.Log;
        }
    }
}