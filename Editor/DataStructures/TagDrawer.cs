using UnityEditor;
using UnityEngine;
using WhateverDevs.Core.Runtime.DataStructures;

namespace WhateverDevs.Core.Editor.DataStructures
{
    /// <summary>
    /// Property drawer for the tag.
    /// </summary>
    [CustomPropertyDrawer(typeof(Tag))]
    public class TagDrawer : PropertyDrawer
    {
        /// <summary>
        /// Paint the Ui.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="property"></param>
        /// <param name="label"></param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            {
                property.FindPropertyRelative("Value").stringValue =
                    EditorGUILayout.TagField(label, property.FindPropertyRelative("Value").stringValue);
            }

            EditorGUI.EndProperty();
        }
    }
}