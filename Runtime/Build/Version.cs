using System;
using System.Text;
using UnityEngine;

namespace WhateverDevs.Core.Runtime.Build
{
    /// <summary>
    /// Scriptable object that saves the game version.
    /// </summary>
    [CreateAssetMenu(menuName = "WhateverDevs/Version", fileName = "Version")]
    public class Version : ScriptableObject
    {
        /// <summary>
        /// Game version number.
        /// </summary>
        public int GameVersion;

        /// <summary>
        /// Mayor version number.
        /// </summary>
        public int MayorVersion;

        /// <summary>
        /// Minor version number.
        /// </summary>
        public int MinorVersion;

        /// <summary>
        /// Date.
        /// </summary>
        public string Date;

        /// <summary>
        /// Version stability.
        /// </summary>
        public VersionStability Stability;
        
        /// <summary>
        /// Get the short version string.
        /// </summary>
        public string ShortVersion =>
            new StringBuilder(GameVersion.ToString()).Append(".")
                                                     .Append(MayorVersion)
                                                     .Append(".")
                                                     .Append(MinorVersion)
                                                     .Append(StabilityToString(Stability))
                                                     .ToString();

        /// <summary>
        /// Get the full version string.
        /// </summary>
        public string FullVersion =>
            new StringBuilder(GameVersion.ToString()).Append(".")
                                                     .Append(MayorVersion)
                                                     .Append(".")
                                                     .Append(MinorVersion)
                                                     .Append(StabilityToString(Stability))
                                                     .Append(".")
                                                     .Append(Date)
                                                     .ToString();

        /// <summary>
        /// Retrieves the version in a string.
        /// </summary>
        /// <param name="versionDisplayMode"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public string ToString(VersionDisplayMode versionDisplayMode)
        {
            switch (versionDisplayMode)
            {
                case VersionDisplayMode.Short: return ShortVersion;
                case VersionDisplayMode.Full: return FullVersion;
                default: throw new ArgumentException("Incorrect version display mode.");
            }
        }

        /// <summary>
        /// Retrieves the version in a string.
        /// </summary>
        public override string ToString() => ToString(VersionDisplayMode.Full);

        /// <summary>
        /// Convert the stability to string.
        /// </summary>
        /// <param name="stability">Given stability-</param>
        /// <returns>String version.</returns>
        public static string StabilityToString(VersionStability stability)
        {
            switch (stability)
            {
                case VersionStability.Alpha: return "a";
                case VersionStability.Beta: return "b";
                case VersionStability.Release: return "";
            }

            return "unknownStability";
        }
    }
    
    /// <summary>
    /// VersionDisplayMode.
    /// </summary>
    public enum VersionDisplayMode
    {
        Short,
        Full
    }

    /// <summary>
    /// Enumeration with the version stability.
    /// </summary>
    public enum VersionStability
    {
        Alpha,
        Beta,
        Release
    }
}