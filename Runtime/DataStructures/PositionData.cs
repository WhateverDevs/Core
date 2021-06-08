using System;
using UnityEngine;

namespace WhateverDevs.Core.Runtime.DataStructures
{
    /// <summary>
    /// Data class to store information about a position and a rotation.
    /// </summary>
    [Serializable]
    public class PositionData
    {
        /// <summary>
        /// Position vector.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// Rotation vector.
        /// </summary>
        public Vector3 Rotation;

        /// <summary>
        /// Path to the mesh that can be used to debug,
        /// this will only be used on editor.
        /// </summary>
        public string DebugMeshPath;
        
        /// <summary>
        /// Path to the material that can be used to debug,
        /// this will only be used on editor.
        /// </summary>
        public string DebugMaterialPath;
    }
}