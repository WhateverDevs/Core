using UnityEngine;

namespace Varguiniano.Core.Editor.Build
{
    /// <summary>
    /// Abstract class representing a custom preprocessor hook.
    /// This can be attached as preprocessor or post processor.
    /// </summary>
    public abstract class BuildProcessorHook : ScriptableObject
    {
        /// <summary>
        /// Run your hook.
        /// </summary>
        public abstract bool RunHook();
    }
}