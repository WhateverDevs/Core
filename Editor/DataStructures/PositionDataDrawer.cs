using System;
using UnityEditor;
using UnityEngine;
using WhateverDevs.Core.Runtime.Common;
using WhateverDevs.Core.Runtime.DataStructures;

namespace WhateverDevs.Core.Editor.DataStructures
{
    /// <summary>
    /// Drawer for the position data class.
    /// </summary>
    [CustomPropertyDrawer(typeof(PositionData))]
    public class PositionDataDrawer : PropertyDrawer
    {
        /// <summary>
        /// Style for rich text labels.
        /// </summary>
        private static GUIStyle RichTextLabel => new GUIStyle("label") {richText = true};

        /// <summary>
        /// Property name of the debug mesh.
        /// </summary>
        private const string DebugMeshPropertyName = "DebugMeshPath";

        /// <summary>
        /// Property name of the debug mesh.
        /// </summary>
        private const string DebugMaterialPropertyName = "DebugMaterialPath";

        /// <summary>
        /// Property name of the position.
        /// </summary>
        private const string PositionPropertyName = "Position";

        /// <summary>
        /// property name of the rotation.
        /// </summary>
        private const string RotationPropertyName = "Rotation";

        /// <summary>
        /// Flag to show an error when using the unity default resources.
        /// </summary>
        private bool defaultResourcesError;

        /// <summary>
        /// Flag to activate scene handles.
        /// </summary>
        private bool showHandles;

        /// <summary>
        /// Mesh to use when positioning handles.
        /// </summary>
        private Mesh meshToUse;

        /// <summary>
        /// Mesh to use when positioning handles.
        /// </summary>
        private Material materialToUse;

        /// <summary>
        /// Reference to the property.
        /// </summary>
        private SerializedProperty serializedProperty;

        /// <summary>
        /// flag to know when it has been initialized.
        /// </summary>
        private bool initialized;

        /// <summary>
        /// Initialize the drawer.
        /// </summary>
        private void Init()
        {
            SceneView.duringSceneGui += SceneGUI;
            initialized = true;
        }

        /// <summary>
        /// Draw the ui.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="label"></param>
        /// <param name="position"></param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!initialized) Init();

            serializedProperty = property;

            EditorGUI.BeginProperty(position, label, property);

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical("box");

            {
                if (property.FindPropertyRelative(DebugMeshPropertyName).stringValue.IsNullEmptyOrWhiteSpace()
                 || property.FindPropertyRelative(DebugMaterialPropertyName).stringValue.IsNullEmptyOrWhiteSpace())
                    PaintSelectMeshUi(property, label);
                else
                    PaintPropertyUi(property, label);
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUI.EndProperty();
        }

        /// <summary>
        /// Section of the ui that paints the mesh selection.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="label"></param>
        private void PaintSelectMeshUi(SerializedProperty property, GUIContent label)
        {
            EditorGUILayout.LabelField(new GUIContent("<b>" + label.text + "</b>", label.tooltip),
                                       RichTextLabel);

            EditorGUILayout
               .HelpBox("You have to select a mesh and material from the project to be able to see the position on the scene. "
                      + "This will only be used for debugging.",
                        MessageType.Info);

            if (defaultResourcesError)
                EditorGUILayout
                   .HelpBox("You can't use a mesh or material from the default Unity ones, sorry.",
                            MessageType.Error);

            meshToUse = (Mesh) EditorGUILayout.ObjectField("Debug mesh", meshToUse, typeof(Mesh), false);

            materialToUse =
                (Material) EditorGUILayout.ObjectField("Debug material", materialToUse, typeof(Material), false);

            if (meshToUse != null)
                property.FindPropertyRelative(DebugMeshPropertyName).stringValue =
                    AssetDatabase.GetAssetPath(meshToUse);

            if (materialToUse != null)
                property.FindPropertyRelative(DebugMaterialPropertyName).stringValue =
                    AssetDatabase.GetAssetPath(materialToUse);

            if ("Library/unity default resources" == property.FindPropertyRelative(DebugMeshPropertyName).stringValue)
            {
                defaultResourcesError = true;
                meshToUse = null;
                property.FindPropertyRelative(DebugMeshPropertyName).stringValue = null;
            }

            if ("Resources/unity_builtin_extra" != property.FindPropertyRelative(DebugMaterialPropertyName).stringValue)
                return;

            defaultResourcesError = true;
            materialToUse = null;
            property.FindPropertyRelative(DebugMaterialPropertyName).stringValue = null;
        }

        /// <summary>
        /// Paint the Ui for the property.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="label"></param>
        private void PaintPropertyUi(SerializedProperty property, GUIContent label)
        {
            if (meshToUse == null)
                meshToUse = AssetDatabase.LoadAssetAtPath<Mesh>(property
                                                               .FindPropertyRelative(DebugMeshPropertyName)
                                                               .stringValue);

            if (materialToUse == null)
                materialToUse = AssetDatabase.LoadAssetAtPath<Material>(property
                                                                       .FindPropertyRelative(DebugMaterialPropertyName)
                                                                       .stringValue);

            EditorGUILayout.BeginHorizontal();

            {
                EditorGUILayout.LabelField(new GUIContent("<b>" + label.text + "</b>", label.tooltip),
                                           RichTextLabel);

                if (GUILayout.Button("Change debug mesh"))
                {
                    meshToUse = null;

                    property
                       .FindPropertyRelative(DebugMeshPropertyName)
                       .stringValue = null;
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(property.FindPropertyRelative("Position"));

            EditorGUILayout.PropertyField(property.FindPropertyRelative("Rotation"));

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            {
                showHandles =
                    EditorGUILayout.Toggle(new GUIContent("Show handles",
                                                          "Show handles on the scene to be able to edit the position and rotation."),
                                           showHandles);

                if (GUILayout.Button("Focus on position"))
                    SceneView.lastActiveSceneView
                             .Frame(new Bounds(serializedProperty
                                              .FindPropertyRelative(PositionPropertyName)
                                              .vector3Value,
                                               Vector3.one));
            }

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Called when we need to draw gizmos on the scene.
        /// </summary>
        /// <param name="sceneView"></param>
        private void SceneGUI(SceneView sceneView)
        {
            try
            {
                if (meshToUse == null) return;

                if (showHandles)
                {
                    EditorGUI.BeginChangeCheck();

                    {
                        serializedProperty
                               .FindPropertyRelative(PositionPropertyName)
                               .vector3Value =
                            Handles.PositionHandle(serializedProperty
                                                  .FindPropertyRelative(PositionPropertyName)
                                                  .vector3Value,
                                                   Quaternion.Euler(serializedProperty
                                                                   .FindPropertyRelative(RotationPropertyName)
                                                                   .vector3Value));

                        serializedProperty
                               .FindPropertyRelative(RotationPropertyName)
                               .vector3Value =
                            Handles.RotationHandle(Quaternion.Euler(serializedProperty
                                                                   .FindPropertyRelative(RotationPropertyName)
                                                                   .vector3Value),
                                                   serializedProperty
                                                      .FindPropertyRelative(PositionPropertyName)
                                                      .vector3Value)
                                   .eulerAngles;

                        serializedProperty.serializedObject.ApplyModifiedProperties();
                    }

                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(serializedProperty.serializedObject.targetObject, "Change spawns");
                        EditorUtility.SetDirty(serializedProperty.serializedObject.targetObject);
                        serializedProperty.serializedObject.ApplyModifiedProperties();
                    }
                }

                materialToUse.SetPass(0);

                Graphics.DrawMeshNow(meshToUse,
                                     serializedProperty
                                        .FindPropertyRelative(PositionPropertyName)
                                        .vector3Value,
                                     Quaternion.Euler(serializedProperty
                                                     .FindPropertyRelative(RotationPropertyName)
                                                     .vector3Value),
                                     0);
            }
            catch (Exception)
            {
                SceneView.duringSceneGui -= SceneGUI;
                initialized = false;
            }
        }
    }
}