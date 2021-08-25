using UnityEngine;
using WhateverDevs.Core.Runtime.DataStructures;

namespace WhateverDevs.Core.Runtime.Rendering
{
    /// <summary>
    /// Scriptable object to store a library of sprites.
    /// </summary>
    [CreateAssetMenu(menuName = "WhateverDevs/Rendering/Sprite Palette", fileName = "SpritePalette")]
    public class SpriteLibrary : DataLibrary<Sprite>
    {
    }
}