using UnityEngine;
using WhateverDevs.Core.Runtime.Common;

namespace WhateverDevs.Core.Behaviours
{
    /// <summary>
    /// Our own Monobehaviour implementation with added functionality.
    /// </summary>
    public class WhateverBehaviour : WhateverBehaviour<WhateverBehaviour>
    {
    }

    /// <summary>
    /// Our own Monobehaviour implementation with added functionality.
    /// It is pretty empty right now, but having it will help not having to refactor a lot in the future.
    /// </summary>
    /// <typeparam name="TLogger">Object to be used on the logs.</typeparam>
    public class WhateverBehaviour<TLogger> : LoggableMonoBehaviour<TLogger> where TLogger : WhateverBehaviour<TLogger>
    {
        /// <summary>
        /// Cache to store component references.
        /// All components in the gameobject share the same cache.
        /// </summary>
        private ComponentReferenceCache ComponentReferenceCache
        {
            get
            {
                if (componentReferenceCache == null) componentReferenceCache = GetComponent<ComponentReferenceCache>();

                if (componentReferenceCache == null)
                    componentReferenceCache = gameObject.AddComponent<ComponentReferenceCache>();

                return componentReferenceCache;
            }
        }

        /// <summary>
        /// Backfield for ComponentReferenceCache.
        /// </summary>
        private ComponentReferenceCache componentReferenceCache;

        /// <summary>
        /// Cached object to wait a frame.
        /// </summary>
        protected readonly WaitForEndOfFrame WaitAFrame = new();

        /// <summary>
        /// Like TryGetComponent but the reference is cached.
        /// </summary>
        public virtual bool TryGetCachedComponent<T>(out T component) where T : Component =>
            ComponentReferenceCache.TryGetCachedComponent(out component);

        /// <summary>
        /// Like GetComponent but the reference is cached.
        /// </summary>
        public virtual T GetCachedComponent<T>() where T : Component => ComponentReferenceCache.GetCachedComponent<T>();

        /// <summary>
        /// Like TryGetComponentInChildren but the reference is cached.
        /// </summary>
        public virtual bool TryGetCachedComponentInChildren<T>(out T component, bool includeInactive = false)
            where T : Component =>
            ComponentReferenceCache.TryGetCachedComponentInChildren(out component, includeInactive);

        /// <summary>
        /// Like GetComponentInChildren but the reference is cached.
        /// </summary>
        public virtual T GetCachedComponentInChildren<T>(bool includeInactive = false) where T : Component =>
            ComponentReferenceCache.GetCachedComponentInChildren<T>(includeInactive);

        /// <summary>
        /// Like TryGetComponentInParent but the reference is cached.
        /// </summary>
        public virtual bool TryGetCachedComponentInParent<T>(out T component) where T : Component =>
            ComponentReferenceCache.TryGetCachedComponentInParent(out component);

        /// <summary>
        /// Like GetComponentInParent but the reference is cached.
        /// </summary>
        public virtual T GetCachedComponentInParent<T>() where T : Component =>
            ComponentReferenceCache.GetCachedComponentInParent<T>();
    }
}