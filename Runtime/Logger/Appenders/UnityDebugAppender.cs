using log4net.Appender;
using log4net.Core;
using UnityEngine;

namespace WhateverDevs.Core.Runtime.Logger.Appenders
{
    public class UnityDebugAppender : AppenderSkeleton
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            if (loggingEvent.Level == Level.Fatal)
            {
                // We don't actually need to show the exceptions on the editor.
                return;
            }

            string message = RenderLoggingEvent(loggingEvent);
            Debug.Log(message);
        }
    }
}