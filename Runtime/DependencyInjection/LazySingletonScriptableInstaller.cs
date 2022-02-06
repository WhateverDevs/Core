using UnityEngine;
using Zenject;

namespace WhateverDevs.Core.DependencyInjection
{
    /// <summary>
    /// Installer for objects that need to be installed as a lazy singleton.
    /// </summary>
    /// <typeparam name="T">Object type to install.</typeparam>
    public class LazySingletonScriptableInstaller<T> : ScriptableObjectInstaller where T : ScriptableObject
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