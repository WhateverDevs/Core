using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using WhateverDevs.Core.Runtime.Common;

namespace Editor.Utils
{
    /// <summary>
    /// Class with the util to clean the cache and restart.
    /// </summary>
    public class CleanCachesAndRestart : Loggable<CleanCachesAndRestart>
    {
        /// <summary>
        /// Clean all the editor caches and restart the editor.
        /// </summary>
        [MenuItem("WhateverDevs/CleanCachesAndRestart", priority = 0)]
        public static void CleanAndRestart()
        {
            UpdatePowershellScripts();
            
            if (!EditorUtility.DisplayDialog("Clean caches and restart",
                                             "This will force reimporting the project, make sure you have saved everything first.",
                                             "Continue",
                                             "Cancel"))
                return;

            string arguments = "-ExecutionPolicy Bypass -File WhateverDevsScripts/CleanUpAndRestart.ps1 \""
                             + EditorApplication.applicationPath
                             + "\"";

            StaticLogger.Info(arguments);

            ProcessStartInfo startInfo =
                new ProcessStartInfo("powershell.exe", arguments)
                {
                    WorkingDirectory = Environment.CurrentDirectory,
                    UseShellExecute = true
                };

            Process process = new Process {StartInfo = startInfo};
            process.Start();
        }

        /// <summary>
        /// Updates the powershell scripts folder.
        /// </summary>
        private static void UpdatePowershellScripts()
        {
            if (Directory.Exists("WhateverDevsScripts")) Directory.Delete("WhateverDevsScripts");

            Directory.CreateDirectory("WhateverDevsScripts");

            File.Copy("Packages/whateverdevs.core/PowershellScripts/CleanUpAndRestart.ps1",
                      "WhateverDevsScripts/CleanUpAndRestart.ps1");
        }
    }
}