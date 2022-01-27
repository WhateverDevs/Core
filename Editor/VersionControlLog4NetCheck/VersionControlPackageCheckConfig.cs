using UnityEngine;

namespace WhateverDevs.Core.Editor.VersionControlLog4NetCheck
{
    /// <summary>
    /// Scriptable object to store the preferences related to the version control package check.
    /// </summary>
    public class VersionControlPackageCheckConfig : ScriptableObject
    {
        /// <summary>
        /// Has the user dismissed the check?
        /// </summary>
        public bool CheckDismissed;
    }
}