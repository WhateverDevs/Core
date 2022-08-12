using UnityEngine;
using Zenject;

namespace WhateverDevs.Core.Runtime.DependencyInjection
{
    /// <summary>
    /// Installer for objects in scene that need to be installed as a lazy singleton.
    /// </summary>
    /// <typeparam name="T">Object type to install.</typeparam>
    public abstract class LazySingletonMonoInstaller<T> : MonoInstaller where T : MonoBehaviour
    {
        /// <summary>
        /// Reference to the object to install.
        /// </summary>
        [SerializeField]
        protected T Reference;

        /// <summary>
        /// Install the reference as a lazy singleton.
        /// </summary>
        public override void InstallBindings() => Container.Bind<T>().FromInstance(Reference).AsSingle().Lazy();
    }
}