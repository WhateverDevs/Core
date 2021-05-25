using WhateverDevs.Core.Runtime.Common;

namespace WhateverDevs.Core.Runtime.Build
{
    /// <summary>
    /// Abstract class representing a custom preprocessor hook.
    /// This can be attached as preprocessor or post processor.
    /// </summary>
    public abstract class BuildProcessorHook : LoggableScriptableObject<BuildProcessorHook>
    {
        /// <summary>
        /// Run your hook.
        /// </summary>
        public abstract bool RunHook(string buildPath);
    }
}