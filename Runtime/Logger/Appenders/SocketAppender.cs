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
        public string Ip { get; set; }

        /// <summary>
        /// Set by the xml file.
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public int Port { get; set; }

        /// <summary>
        /// Instance of a logging manager.
        /// </summary>
        private SocketLoggingManager socketLoggingManager;

        protected override void Append(LoggingEvent loggingEvent)
        {
            if (socketLoggingManager == null)
                socketLoggingManager = new SocketLoggingManager(Ip, Port, SocketType.Stream, ProtocolType.Tcp, 4096);

            string message = RenderLoggingEvent(loggingEvent);

            if (Application.isPlaying && !socketLoggingManager.ShuttingDown) socketLoggingManager.Send(message);
        }
    }
}