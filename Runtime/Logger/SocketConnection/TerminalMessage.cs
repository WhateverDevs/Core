using System;

namespace WhateverDevs.Core.Runtime.Logger.SocketConnection
{
    /// <summary>
    /// Message sent by the terminal to the connection socket.
    /// </summary>
    [Serializable]
    public class TerminalMessage
    {
        /// <summary>
        /// Is the terminal connected?
        /// </summary>
        public bool Connected;

        /// <summary>
        /// Type of message.
        /// </summary>
        public TerminalMessageType Type;

        /// <summary>
        /// Command to execute.
        /// </summary>
        public string Command;
    }

    /// <summary>
    /// Types of messages the terminal can receive.
    /// </summary>
    [Serializable]
    public enum TerminalMessageType
    {
        Connection,
        Command
    }
}