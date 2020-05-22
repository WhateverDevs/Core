using UnityEngine;

namespace Varguiniano.Core.Editor.Build
{
    /// <summary>
    /// Library with all the build processor hooks.
    /// </summary>
    public class BuildProcessorHookLibrary : ScriptableObject
    {
        /// <summary>
        /// Hooks to run before the build.
        /// </summary>
        public BuildProcessorHook[] PreProcessorHooks;

        /// <summary>
        /// Hooks to run after the build.
        /// </summary>
        public BuildProcessorHook[] PostProcessorHooks;
    }
}