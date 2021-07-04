using JetBrains.Annotations;
using log4net.Appender;
using log4net.Core;
using UnityEngine;

namespace WhateverDevs.Core.Runtime.Logger.Appenders
{
    /// <summary>
    /// Appender that sends the logging events back to the unity console.
    /// </summary>
    [UsedImplicitly]
    public class UnityDebugAppender : AppenderSkeleton
    {
        /// <summary>
        /// Called when a log is sent.
        /// </summary>
        /// <param name="loggingEvent"></param>
        protected override void Append(LoggingEvent loggingEvent)
        {
            if (loggingEvent.Level == Level.Fatal)
                // We don't actually need to log exceptions again.
                return;

            string message = RenderLoggingEvent(loggingEvent);
            Debug.Log(message);
        }
    }
}