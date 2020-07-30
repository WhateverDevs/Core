using System.IO;
using UnityEditor;
using WhateverDevs.Core.Runtime.Common;

namespace WhateverDevs.Core.Editor.Linker
{
    /// <summary>
    /// This class makes sure that the link.xml file is correctly setup for using IL2CPP.
    /// </summary>
    [InitializeOnLoad]
    public class LinkerSetup : Loggable<LinkerSetup>
    {
        private const string DataFolder = "Assets/Data/";

        private const string LinkerFolder = DataFolder + "Linker/";
        
        /// <summary>
        /// Folder for the linker.
        /// </summary>
        private const string LinkerLocation = LinkerFolder + "WhateverDevs.Core/";

        /// <summary>
        /// Name for the file.
        /// </summary>
        private const string LinkerName = "link.xml";

        /// <summary>
        /// Path of the original file.
        /// </summary>
        private const string OriginalFile = "Packages/whateverdevs.core/Editor/Linker/LinkerSetup.cs";
        
        /// <summary>
        /// Constructor that initializes when unity loads.
        /// </summary>
        static LinkerSetup()
        {
            if (!Directory.Exists(LinkerLocation)) Directory.CreateDirectory(LinkerLocation);

            if (File.Exists(LinkerLocation + LinkerName)) return;
            File.Copy(OriginalFile, LinkerLocation + LinkerName);
            GetStaticLogger().Info("A link.xml file has been created for IL2CPP builds to work.");
        }
    }
}