using WhateverDevs.Core.Runtime.Common;

namespace Varguiniano.Core.Runtime.Build
{
    /// <summary>
    /// Abstract class representing a custom preprocessor hook.
    /// This can be attached as preprocessor or post processor.
    /// </summary>
    public abstract class BuildProcessorHook<TBuildProcessorHook> : LoggableScriptableObject<TBuildProcessorHook>,
                                                                    IBuildProcessorHook
        where TBuildProcessorHook : BuildProcessorHook<TBuildProcessorHook>
    {
        /// <summary>
        /// Run your hook.
        /// </summary>
        public abstract bool RunHook();
    }
}