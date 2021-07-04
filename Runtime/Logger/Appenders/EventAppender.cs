using JetBrains.Annotations;
using log4net.Appender;
using log4net.Core;
using UnityEngine;
using WhateverDevs.Core.Runtime.Common;

namespace WhateverDevs.Core.Runtime.Logger.Appenders
{
    /// <summary>
    /// Appender that will send back the logs to the application as events.
    /// </summary>
    [UsedImplicitly]
    public class EventAppender : AppenderSkeleton
    {
        /// <summary>
        /// Check if the application is playing and if so send the logs back as events.
        /// </summary>
        /// <param name="loggingEvent"></param>
        protected override void Append(LoggingEvent loggingEvent)
        {
            if (!Application.isPlaying) return;

            AppEventsListener.Instance.RegisterLogEvent(RenderLoggingEvent(loggingEvent),
                                                        loggingEvent.Level.ToLogType());
        }
    }
}