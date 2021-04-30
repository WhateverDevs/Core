using UnityEngine;
using WhateverDevs.Core.Runtime.Build;
using Zenject;
using Version = WhateverDevs.Core.Runtime.Build.Version;

namespace WhateverDevs.Core.Runtime.Ui
{
    /// <summary>
    /// Class that displays the game version on the given TMP Text.
    /// </summary>
    public class DisplayVersionOnText : EasyUpdateText<DisplayVersionOnText>
    {
        /// <summary>
        /// Reference to the version.
        /// </summary>
        [HideInInspector]
        [Inject]
        public Version Version;

        /// <summary>
        /// Version display mode to use.
        /// </summary>
        public VersionDisplayMode VersionDisplayMode;

        /// <summary>
        /// Set the text.
        /// </summary>
        private void OnEnable() => UpdateText(Version.ToString(VersionDisplayMode));
    }
}