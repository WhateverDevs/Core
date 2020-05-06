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
        [Tooltip("Reference to the default log for net configuration. This is stored within the WhateverDevs' Core package.")]
        public Log4NetDefaultConfig DefaultConfig;

        /// <summary>
        /// Always override the configs?
        /// </summary>
        [Tooltip("This will always override the configuration set on the persistent datapath with the default one.")]
        public bool AlwaysOverride;
    }
}