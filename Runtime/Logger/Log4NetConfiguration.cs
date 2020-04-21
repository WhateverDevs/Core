using UnityEngine;

namespace WhateverDevs.Core.Runtime.Logger
{
    /// <summary>
    /// Configuration for log4net that has to be create on the resources folder.
    /// This is very low level, so we can't use the configuration system for it.
    /// </summary>
    [CreateAssetMenu(menuName = "WhateverDevs/Logger/Configuration", fileName = "LoggerConfiguration")]
    public class Log4NetConfiguration : ScriptableObject
    {
        /// <summary>
        /// Reference to the default configuration.
        /// </summary>
        public Log4NetDefaultConfig DefaultConfig;
        
        /// <summary>
        /// Always override the configs?
        /// </summary>
        public bool AlwaysOverride;
        
        // TODO: Create a one time switch that allows for a single override of the configuration on a device to install the new default.
        // We probably need to store this bool value on a json file for that.
    }
}