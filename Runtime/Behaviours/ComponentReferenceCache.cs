using System;
using System.Collections.Generic;

namespace WhateverDevs.Core.Behaviours
{
    /// <summary>
    /// Component that caches references to other components in the same gameobject.
    /// Using GetCachedComponent instead of GetComponent will use this cache.
    /// </summary>
    public class ComponentReferenceCache : WhateverBehaviour<ComponentReferenceCache>
    {
        /// <summary>
        /// Cache of all the component references.
        /// </summary>
        private readonly Dictionary<Type, object> cache = new();

        /// <summary>
        /// Like TryGetComponent but the reference is cached.
        /// </summary>
        public override bool TryGetCachedComponent<T>(out T component)
        {
            component = GetCachedComponent<T>();

            // ReSharper disable once CompareNonConstrainedGenericWithNull
            return component != null;
        }

        /// <summary>
        /// Like GetComponent but the reference is cached.
        /// </summary>
        public override T GetCachedComponent<T>()
        {
            if (cache.TryGetValue(typeof(T), out object component))
            {
                if (component == null)
                    cache.Remove(typeof(T));
                else
                    return (T)component;
            }

            cache[typeof(T)] = GetComponent<T>();

            return (T)cache[typeof(T)];
        }

        /// <summary>
        /// Like TryGetComponentInChildren but the reference is cached.
        /// </summary>
        public override bool TryGetCachedComponentInChildren<T>(out T component, bool includeInactive = false)
        {
            component = GetCachedComponentInChildren<T>(includeInactive);

            // ReSharper disable once CompareNonConstrainedGenericWithNull
            return component != null;
        }

        /// <summary>
        /// Like GetComponentInChildren but the reference is cached.
        /// </summary>
        public override T GetCachedComponentInChildren<T>(bool includeInactive = false)
        {
            if (cache.TryGetValue(typeof(T), out object component))
            {
                if (component == null)
                    cache.Remove(typeof(T));
                else
                    return (T)component;
            }

            cache[typeof(T)] = GetComponentInChildren<T>(includeInactive);

            return (T)cache[typeof(T)];
        }

        /// <summary>
        /// Like TryGetComponentInParent but the reference is cached.
        /// </summary>
        public override bool TryGetCachedComponentInParent<T>(out T component)
        {
            component = GetCachedComponentInParent<T>();

            // ReSharper disable once CompareNonConstrainedGenericWithNull
            return component != null;
        }

        /// <summary>
        /// Like GetComponentInParent but the reference is cached.
        /// </summary>
        public override T GetCachedComponentInParent<T>()
        {
            if (cache.TryGetValue(typeof(T), out object component))
            {
                if (component == null)
                    cache.Remove(typeof(T));
                else
                    return (T)component;
            }

            cache[typeof(T)] = GetComponentInParent<T>();

            return (T)cache[typeof(T)];
        }
    }
}