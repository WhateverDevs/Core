using UnityEngine;

namespace WhateverDevs.Core.Runtime.Logger
{
    /// <summary>
    /// Scriptable object that stores the default configuration for log4net.
    /// </summary>
    public class Log4NetDefaultConfig : ScriptableObject
    {
        /// <summary>
        /// String that stores the contents of the default config file.
        /// </summary>
        public string DefaultConfig;
    }
}