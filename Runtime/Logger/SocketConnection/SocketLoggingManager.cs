using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using WhateverDevs.Core.Runtime.Common;
using WhateverDevs.Core.Runtime.Serialization;

namespace WhateverDevs.Core.Runtime.Logger.SocketConnection
{
    /// <summary>
    /// Class that handles the socket that will send the logs.
    /// This will only send messages, it won't receive them.
    /// </summary>
    public class SocketLoggingManager : Loggable<SocketLoggingManager>
    {
        public bool ShuttingDown;
        
        private readonly Socket socket;

        private Socket handler;

        private readonly Queue<string> messagesToSend;

        private readonly Thread sendingThread;

        private readonly Thread receivingThread;

        private readonly byte[] buffer;

        private readonly ISerializer<string> jsonSerializer;

        private bool connectedToTerminal;

        private volatile bool closeSendingThread;

        private volatile bool closeReceivingThread;

        /// <summary>
        ///     Setting a connection via socket
        /// </summary>
        /// <param name="ip">ip address</param>
        /// <param name="port">port number</param>
        /// <param name="socketType">Socket Type</param>
        /// /// <param name="protocolType">Protocol Type</param>
        /// <param name="bufferSize">Buffer Size</param>
        public SocketLoggingManager(string ip,
                                    int port,
                                    SocketType socketType,
                                    ProtocolType protocolType,
                                    int bufferSize)
        {
            AppEventsListener.Instance.AppQuitting += ShutDownThreadsIfRunning;

            buffer = new byte[bufferSize];
            messagesToSend = new Queue<string>();
            jsonSerializer = new JsonSerializer();

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            if (socket?.Connected != true)
            {
                socket = new Socket(AddressFamily.InterNetwork, socketType, protocolType);
                GetLogger().Info("Opening logging socket on port " + port + ".");
                socket.Bind(endPoint);
                socket.Listen(20);
            }

            endPoint.Port++;

            sendingThread = new Thread(SendMessagesLoop);
            sendingThread.Start();
            receivingThread = new Thread(ReceiveTerminalMessage);
            receivingThread.Start();
        }

        /// <summary>
        ///     Send the bytes using the socket
        /// </summary>
        /// <param name="message">Message to send.</param>
        public void Send(string message) => messagesToSend.Enqueue(message);

        /// <summary>
        /// Shutdown the threads.
        /// </summary>
        private void ShutDownThreadsIfRunning()
        {
            AppEventsListener.Instance.AppQuitting -= ShutDownThreadsIfRunning;
            ShuttingDown = true;
            
            GetLogger().Info("Shutting down logging threads.");
            closeSendingThread = true;
            closeReceivingThread = true;

            sendingThread.Join();

            receivingThread.Abort();

            handler?.Close();
            socket?.Close();
        }

        /// <summary>
        /// Loop to send the log messages through the socket.
        /// </summary>
        private void SendMessagesLoop()
        {
            while (!closeSendingThread)
            {
                if (!connectedToTerminal) continue;
                SendLogMessages();
            }
        }

        /// <summary>
        /// Sends the log messages stored in the queue.
        /// </summary>
        private void SendLogMessages()
        {
            while (messagesToSend.Count > 0)
            {
                byte[] data = Encoding.UTF8.GetBytes(messagesToSend.Dequeue());
                handler.Send(data, 0, data.Length, SocketFlags.None);
            }
        }

        /// <summary>
        /// Wait for a message from the terminal to see if it connected.
        /// </summary>
        private void ReceiveTerminalMessage()
        {
            handler = socket.Accept();

            GetLogger().Info("Received socket connection.");

            while (!closeReceivingThread)
                try
                {
                    handler.Receive(buffer);
                    string json = Encoding.UTF8.GetString(buffer);

                    GetLogger().Info(json);

                    TerminalMessage message = jsonSerializer.From<TerminalMessage>(json);
                    connectedToTerminal = message.Connected;

                    if (connectedToTerminal) GetLogger().Info("External terminal connected.");

                    // TODO: Process commands received.
                }
                catch (Exception)
                {
                    // Exception? I don't know what you are talking about.
                }
        }
    }
}