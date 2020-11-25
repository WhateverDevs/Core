using System.Net;
using System.Net.Sockets;
using log4net.Appender;
using log4net.Core;
using UnityEngine;
using WhateverDevs.Core.Runtime.Logger.SocketConnection;

namespace WhateverDevs.Core.Runtime.Logger.Appenders
{
    /// <summary>
    /// Appender that relays the messages to a socket.
    /// </summary>
    public class SocketAppender : AppenderSkeleton
    {
        /// <summary>
        /// Set by the xml file.
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public int Port { get; set; }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if (!Application.isPlaying) return;

            if (!SocketLoggingManager.Instance.Initialized)
                SocketLoggingManager.Instance.Initialize(IPAddress.Any, Port, SocketType.Stream, ProtocolType.Tcp, 4096);

            string message = RenderLoggingEvent(loggingEvent);

            if (Application.isPlaying && !SocketLoggingManager.Instance.ShuttingDown)
                SocketLoggingManager.Instance.Send(message);
        }
    }
}