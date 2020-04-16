using System;
using UnityEngine;

namespace WhateverDevs.Core.Runtime.DataStructures
{
    /// <summary>
    /// Serializable class for reference pair to ease the creation of serializable dictionaries.
    /// </summary>
    [Serializable]
    public class StringGameObjectPair : ObjectPair<string, GameObject>
    {
    }
}