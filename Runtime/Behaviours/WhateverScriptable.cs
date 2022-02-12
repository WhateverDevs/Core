using UnityEngine;
using WhateverDevs.Core.Runtime.Common;

namespace WhateverDevs.Core.Behaviours
{
    /// <summary>
    /// Our own Scriptable Object implementation with added functionality.
    /// </summary>
    public class WhateverScriptable : WhateverScriptable<WhateverScriptable>
    {
    }

    /// <summary>
    /// Our own Scriptable Object implementation with added functionality.
    /// It is pretty empty right now, but having it will help not having to refactor a lot in the future.
    /// </summary>
    /// <typeparam name="TLogger">Object to be used on the logs.</typeparam>
    public class WhateverScriptable<TLogger> : LoggableScriptableObject<TLogger>
        where TLogger : WhateverScriptable<TLogger>
    {
        /// <summary>
        /// Cached object to wait a frame.
        /// </summary>
        protected readonly WaitForEndOfFrame WaitAFrame = new();
    }
}