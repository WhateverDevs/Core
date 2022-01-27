#if ODIN_INSPECTOR_3
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Modules;
#endif
using WhateverDevs.Core.Runtime.Common;

namespace WhateverDevs.Core.Editor.OdinCompatibility
{
    /// <summary>
    /// Class to store Odin compatibility settings.
    /// </summary>
    public class OdinSettings : LoggableScriptableObject<OdinSettings>
    {
        /// <summary>
        /// Flag to know if the user acknowledge the need to use Odin.
        /// </summary>
        #if ODIN_INSPECTOR_3
        [ReadOnly]
        #endif
        public bool AcknowledgedOdinNeed;

        /// <summary>
        /// Reference to the Odin Module Config.
        /// </summary>
        #if ODIN_INSPECTOR_3
        public OdinModuleConfig OdinModuleConfig;
        #endif
    }
}