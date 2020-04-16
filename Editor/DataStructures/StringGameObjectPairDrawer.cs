using UnityEditor;
using UnityEngine;
using WhateverDevs.Core.Runtime.DataStructures;

namespace WhateverDevs.Core.Editor.DataStructures
{
    /// <summary>
    /// Custom property drawer for the string,gameobject pair.
    /// </summary>
    [CustomPropertyDrawer(typeof(StringGameObjectPair))]
    public class StringGameObjectPairDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            EditorGUIUtility.labelWidth = 1;

            EditorGUI.PropertyField(new Rect(position.x, position.y, (position.width / 2) - 10, position.height),
                                    property.FindPropertyRelative("Key"));

            EditorGUI.PropertyField(new Rect((position.width / 2) + 10, position.y, (position.width / 2) - 3, position.height),
                                    property.FindPropertyRelative("Value"));

            EditorGUI.EndProperty();
        }
    }
}