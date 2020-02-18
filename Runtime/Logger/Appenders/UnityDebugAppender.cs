﻿using log4net.Appender;
using log4net.Core;
using UnityEngine;

namespace WhateverDevs.Core.Runtime.Logger.Appenders
{
    public class UnityDebugAppender : AppenderSkeleton
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            var message = RenderLoggingEvent(loggingEvent);
            Debug.Log(message);
        }
    }
}
