using UnityEngine;
using Zenject;

namespace WhateverDevs.Core.Runtime.DependencyInjection
{
    /// <summary>
    /// Implementation of Extenjects's PlaceholderFactory that adds
    /// instantiating methods with similar parameters to Unity's Object.Instantiate().
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    public abstract class GameObjectFactory<TComponent> : PlaceholderFactory<TComponent> where TComponent : MonoBehaviour
    {
        /// <summary>
        /// Create the object and assign its parent.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public TComponent CreateGameObject(Transform parent)
        {
            TComponent component = Create();
            component.transform.parent = parent;
            return component;
        }

        /// <summary>
        /// Create the object and assign its position and rotation.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public TComponent CreateGameObject(Vector3 position, Quaternion rotation)
        {
            TComponent component = Create();
            component.transform.SetPositionAndRotation(position, rotation);
            return component;
        }

        /// <summary>
        /// Create the object and assign its parent, its position and rotation.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public TComponent CreateGameObject(Transform parent, Vector3 position, Quaternion rotation)
        {
            TComponent component = CreateGameObject(parent);
            component.transform.SetPositionAndRotation(position, rotation);
            return component;
        }
    }
}