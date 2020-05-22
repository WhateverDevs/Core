﻿using UnityEngine;

namespace Varguiniano.Core.Runtime.Build
{
    /// <summary>
    /// Library with all the build processor hooks.
    /// </summary>
    public class BuildProcessorHookLibrary : ScriptableObject
    {
        /// <summary>
        /// Hooks to run before the build.
        /// </summary>
        [SerializeReference]
        public IBuildProcessorHook[] PreProcessorHooks;

        /// <summary>
        /// Hooks to run after the build.
        /// </summary>
        [SerializeReference]
        public IBuildProcessorHook[] PostProcessorHooks;
    }
}