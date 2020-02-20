using UnityEditor;
using UnityEngine;

namespace WhateverDevs.Core.Runtime.Logger.Editor
{
   [CustomEditor(typeof(LoggerDebugTest))]
   public class LoggerDebugTestEditor : UnityEditor.Editor
   {
      public LoggerDebugTest _this
      {
         get
         {
            return target as LoggerDebugTest;
         }
      }
      public override void OnInspectorGUI()
      {
         base.OnInspectorGUI();
         if (GUILayout.Button("Log"))
         {
            LoggerDebugTest.Log.Error("Error");
            LoggerDebugTest.Log.Info("Info");
            LoggerDebugTest.Log.Fatal("Fatal");;
         }
      }
   }
}
