using UnityEngine;

namespace WhateverDevs.Core.Runtime.Serialization
{
    /// <summary>
    /// Scriptable to get a json serializer for editor referencing.
    /// </summary>
    [CreateAssetMenu(menuName = "WhateverDevs/Serializers/Json", fileName = "JsonSerializer")]
    public class JsonSerializerScriptable : SerializerScriptable<JsonSerializer, string>
    {
    }
}