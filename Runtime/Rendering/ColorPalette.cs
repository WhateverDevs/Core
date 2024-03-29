using UnityEngine;
using WhateverDevs.Core.Runtime.DataStructures;

namespace WhateverDevs.Core.Runtime.Rendering
{
    /// <summary>
    /// Scriptable object to store a palette of colors.
    /// </summary>
    [CreateAssetMenu(menuName = "WhateverDevs/Rendering/Color Palette", fileName = "ColorPalette")]
    public class ColorPalette : DataLibrary<Color>
    {
    }
}