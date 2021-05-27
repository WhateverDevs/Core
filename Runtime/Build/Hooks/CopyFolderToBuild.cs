using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;
using WhateverDevs.Core.Runtime.Common;

namespace WhateverDevs.Core.Runtime.Build.Hooks
{
    /// <summary>
    /// Copy a folder to the build path after building.
    /// </summary>
    [CreateAssetMenu(menuName = "WhateverDevs/BuildHooks/CopyFolder", fileName = "CopyFolderToBuild")]
    public class CopyFolderToBuild : BuildProcessorHook
    {
        [FolderPath]
        public string Folder;

        /// <summary>
        /// Run the hook.
        /// </summary>
        /// <param name="buildPath"></param>
        /// <returns></returns>
        public override bool RunHook(string buildPath)
        {
            DirectoryInfo originalFolderInfo = new DirectoryInfo(Folder);
            DirectoryInfo buildParent = new DirectoryInfo(buildPath).Parent;
            DirectoryInfo targetFolder = new DirectoryInfo(buildParent + "/" + originalFolderInfo.Name);

            if (targetFolder.Exists) Utils.DeleteDirectory(targetFolder.FullName);

            targetFolder.Create();
            
            Logger.Info("Copying folder " + Folder + " to build.");

            Utils.CopyFilesRecursively(originalFolderInfo, targetFolder);
            return true;
        }
    }
}