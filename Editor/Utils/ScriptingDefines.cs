using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace WhateverDevs.Core.Editor.Utils
{
    /// <summary>
    /// Class with scripting defines utilities.
    /// </summary>
    public static class ScriptingDefines
    {
        /// <summary>
        /// Set a scripting define.
        /// </summary>
        /// <param name="defineToSet">Define to set.</param>
        /// <param name="enable">Enable or disable this define?</param>
        public static void SetDefine(string defineToSet, bool enable)
        {
            string defines =
                PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildPipeline
                                                                    .GetBuildTargetGroup(EditorUserBuildSettings
                                                                        .activeBuildTarget));

            List<string> currentDefines = defines.Split(';').ToList();

            if (enable)
            {
                if (currentDefines.Contains(defineToSet)) return;

                currentDefines.Add(defineToSet);
            }
            else
            {
                if (!currentDefines.Contains(defineToSet)) return;

                currentDefines.Remove(defineToSet);
            }

            string newDefines = "";

            for (int i = 0; i < currentDefines.Count; ++i)
            {
                newDefines += currentDefines[i];
                if (i < currentDefines.Count - 1) newDefines += ";";
            }

            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildPipeline
                                                                .GetBuildTargetGroup(EditorUserBuildSettings
                                                                    .activeBuildTarget),
                                                             newDefines);
        }
    }
}