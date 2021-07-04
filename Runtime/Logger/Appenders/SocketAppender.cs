using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using JetBrains.Annotations;
using log4net.Appender;
using log4net.Core;
using UnityEngine;
using WhateverDevs.Core.Runtime.Logger.SocketConnection;

namespace WhateverDevs.Core.Runtime.Logger.Appenders
{
    /// <summary>
    /// Appender that relays the messages to a socket.
    /// </summary>
    [UsedImplicitly]
    public class SocketAppender : AppenderSkeleton
    {
        /// <summary>
        /// Set by the xml file.
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public int Port { get; set; }

        /// <summary>
        /// Flat to know if the exception catcher has initialized.
        /// </summary>
        private bool exceptionCatcherInitialized;

        /// <summary>
        /// Store the messages before the app is ready to send them later.
        /// </summary>
        private Queue<LoggingEvent> messageQueue = new Queue<LoggingEvent>();

        /// <summary>
        /// Called when logging happens.
        /// </summary>
        /// <param name="loggingEvent"></param>
        protected override void Append(LoggingEvent loggingEvent)
        {
            if (!Application.isPlaying)
            {
                messageQueue.Enqueue(loggingEvent);
                return;
            }

            // Weird recurrence but it should work.
            if (messageQueue.Count > 0) Append(messageQueue.Dequeue());

            if (!exceptionCatcherInitialized && ExceptionCatcher.Instance != null)
            {
                // This should be enough to initialize the exception catcher.
                exceptionCatcherInitialized = true;
            }

            if (!SocketLoggingManager.Instance.Initialized && SocketLoggingManager.Instance != null)
                SocketLoggingManager.Instance.Initialize(IPAddress.Any,
                                                         Port,
                                                         SocketType.Stream,
                                                         ProtocolType.Tcp,
                                                         4096);

            string message = RenderLoggingEvent(loggingEvent);

            SocketLoggingManager.Instance.Send(message);
        }
    }
}