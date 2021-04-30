using UnityEngine;
using Zenject;

namespace WhateverDevs.Core.Runtime.Build
{
    /// <summary>
    /// Installer that injects the version data.
    /// </summary>
    [CreateAssetMenu(menuName = "WhateverDevs/DI/Version", fileName = "VersionInstaller")]
    public class VersionInstaller : ScriptableObjectInstaller
    {
        /// <summary>
        /// Reference to the version.
        /// </summary>
        public Version Version;

        /// <summary>
        /// Inject the version.
        /// </summary>
        public override void InstallBindings() => Container.Bind<Version>().FromInstance(Version).AsSingle().Lazy();
    }
}
