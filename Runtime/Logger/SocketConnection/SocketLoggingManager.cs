using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using WhateverDevs.Core.Runtime.Common;
using WhateverDevs.Core.Runtime.Serialization;

#if ODIN_INSPECTOR_3
using Sirenix.OdinInspector;
#endif

namespace WhateverDevs.Core.Runtime.Logger.SocketConnection
{
    /// <summary>
    /// Class that handles the socket that will send the logs.
    /// This will only send messages, it won't receive them.
    /// </summary>
    public class SocketLoggingManager : Singleton<SocketLoggingManager>
    {
        /// <summary>
        /// Flag to know if the manager is initialized.
        /// </summary>
        #if ODIN_INSPECTOR_3
        [ReadOnly]
        #endif
        public bool Initialized;

        /// <summary>
        /// Flag to know if the manager is shutting down.
        /// </summary>
        #if ODIN_INSPECTOR_3
        [ReadOnly]
        #endif
        public bool ShuttingDown;

        /// <summary>
        /// Reference to the socket used for connection.
        /// </summary>
        private Socket socket;

        /// <summary>
        /// Reference to the socket used for sending and receiving messages.
        /// </summary>
        private Socket handler;

        /// <summary>
        /// Buffer of messages to send.
        /// </summary>
        private Queue<string> messagesToSend;

        /// <summary>
        /// Thread used to send the messages.
        /// </summary>
        private Thread sendingThread;

        /// <summary>
        /// Thread used to receive the messages.
        /// </summary>
        private Thread receivingThread;

        /// <summary>
        /// Buffer where the received messages are stored.
        /// </summary>
        private byte[] buffer;

        /// <summary>
        /// Reference to a json serializer.
        /// </summary>
        private ISerializer<string> jsonSerializer;

        /// <summary>
        /// Flag to know if we are connected to the terminal.
        /// </summary>
        private bool connectedToTerminal;

        /// <summary>
        /// Flag to mark the sending thread for closing.
        /// </summary>
        private volatile bool closeSendingThread;

        /// <summary>
        /// Flag to mark the receiving thread for closing.
        /// </summary>
        private volatile bool closeReceivingThread;

        /// <summary>
        ///     Setting a connection via socket
        /// </summary>
        /// <param name="ip">ip address</param>
        /// <param name="port">port number</param>
        /// <param name="socketType">Socket Type</param>
        /// /// <param name="protocolType">Protocol Type</param>
        /// <param name="bufferSize">Buffer Size</param>
        public void Initialize(IPAddress ip,
                               int port,
                               SocketType socketType,
                               ProtocolType protocolType,
                               int bufferSize)
        {
            if (Initialized)
            {
                GetLogger().Warn("Already initialized!");
                return;
            }

            buffer = new byte[bufferSize];
            messagesToSend = new Queue<string>();
            jsonSerializer = new JsonSerializer();

            IPEndPoint endPoint = new IPEndPoint(ip, port);

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

            GetLogger().Info("Socket logging manager initialized.");
            Initialized = true;
        }

        /// <summary>
        ///     Send the bytes using the socket
        /// </summary>
        /// <param name="message">Message to send.</param>
        public void Send(string message) => messagesToSend.Enqueue(message);

        /// <summary>
        /// Shutdown the threads.
        /// </summary>
        private void OnDisable()
        {
            ShuttingDown = true;

            GetLogger().Info("Shutting down logging threads.");
            closeSendingThread = true;
            closeReceivingThread = true;

            sendingThread?.Join();

            receivingThread?.Abort();

            handler?.Close();
            socket?.Close();

            Initialized = false;
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
                string message = messagesToSend.Dequeue();
                byte[] data = Encoding.UTF8.GetBytes(message);
                handler.Send(data, 0, data.Length, SocketFlags.None);
                Thread.Sleep(100); // Give it some time to send the message.
            }
        }

        /// <summary>
        /// Wait for a message from the terminal to see if it connected.
        /// </summary>
        private void ReceiveTerminalMessage()
        {
            while (!closeReceivingThread)
                try
                {
                    if (!connectedToTerminal)
                    {
                        handler = socket.Accept();

                        GetLogger().Info("Received socket connection.");
                    }

                    int result = handler.Receive(buffer);

                    if (result == 0)
                    {
                        // This code will run when the receive method stops because we are not longer connected.
                        if (connectedToTerminal) GetLogger().Info("External terminal disconnected.");
                        connectedToTerminal = false;
                    }
                    else
                    {
                        string json = Encoding.UTF8.GetString(buffer);
                        TerminalMessage message = jsonSerializer.From<TerminalMessage>(json);
                        connectedToTerminal = message.Connected;

                        if (connectedToTerminal) GetLogger().Info("External terminal connected.");

                        // TODO: Process commands received.
                    }
                }
                catch
                {
                    // Exception? I don't know what you are talking about.
                }
        }
    }
}