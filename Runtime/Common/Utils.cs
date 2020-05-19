using System.IO;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEngine;

namespace WhateverDevs.Core.Runtime.Common
{
    /// <summary>
    /// Class with utility functions and extensions.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Checks if a game object has the DontDestroyOnLoadFlag.
        /// </summary>
        /// <param name="gameObject">The game object to check.</param>
        /// <returns>True if it won't be destroyed.</returns>
        public static bool IsDontDestroyOnLoad(this GameObject gameObject) => gameObject.scene.buildIndex == -1;

        /// <summary>
        /// Determines if a string is null, empty or whitespace.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullEmptyOrWhiteSpace(this string value) =>
            string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);

        /// <summary>
        /// Copy the content of a directory to another.
        /// </summary>
        /// <param name="source">Source directory.</param>
        /// <param name="target">Target directory.</param>
        public static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            if (!target.Exists) target.Create();

            foreach (DirectoryInfo dir in source.GetDirectories())
                CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));

            foreach (FileInfo file in source.GetFiles()) file.CopyTo(Path.Combine(target.FullName, file.Name), true);
        }

        /// <summary>
        /// Wrapper for recursively deleting a directory that (almost) works around permissions and system delays.
        /// Refer to: https://stackoverflow.com/questions/1157246/unauthorizedaccessexception-trying-to-delete-a-file-in-a-folder-where-i-can-dele
        /// to: https://stackoverflow.com/questions/34981143/is-directory-delete-create-synchronous
        /// and to: https://stackoverflow.com/questions/755574/how-to-quickly-check-if-folder-is-empty-net
        /// </summary>
        /// <param name="targetDir">The directory to delete.</param>
        public static void DeleteDirectory(string targetDir)
        {
            if (!Directory.Exists(targetDir)) return;
            File.SetAttributes(targetDir, FileAttributes.Directory);
            File.SetAttributes(targetDir, FileAttributes.Normal);

            string[] files = Directory.GetFiles(targetDir);
            string[] dirs = Directory.GetDirectories(targetDir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
                while (File.Exists(file)) Thread.Sleep(100);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
                while (Directory.Exists(dir)) Thread.Sleep(100);
            }

            while (Directory.EnumerateFileSystemEntries(targetDir).Any()) Thread.Sleep(100);
            Directory.Delete(targetDir, false);
            while (Directory.Exists(targetDir)) Thread.Sleep(100);
        }

        /// <summary>
        /// Closes the game in a more elegant way than Alt+F4.
        /// </summary>
        public static void CloseGame()
        {
            #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}