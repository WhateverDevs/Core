using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEngine;
using WhateverDevs.Core.Runtime.DataStructures;

namespace WhateverDevs.Core.Runtime.Common
{
    /// <summary>
    /// Class with utility functions and extensions.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Get the modulus of a number and an operator.
        /// From: https://stackoverflow.com/questions/10065080/mod-explanation/10065670#10065670
        /// </summary>
        /// <param name="a">The number.</param>
        /// <param name="n">The operator.</param>
        /// <returns>The modulus.</returns>
        public static int Modulus(this int a, int n)
        {
            int result = a % n;

            if (result < 0 && n > 0 || result > 0 && n < 0) result += n;

            return result;
        }

        /// <summary>
        /// Checks if a game object has the DontDestroyOnLoadFlag.
        /// </summary>
        /// <param name="gameObject">The game object to check.</param>
        /// <returns>True if it won't be destroyed.</returns>
        public static bool IsDontDestroyOnLoad(this GameObject gameObject) => gameObject.scene.buildIndex == -1;

        /// <summary>
        /// Sets the position and rotation of a transform from a position data object.
        /// </summary>
        /// <param name="transform">The transform to change.</param>
        /// <param name="positionData">Position data to set.</param>
        public static void SetPositionAndRotation(this Transform transform, PositionData positionData) =>
            transform.SetPositionAndRotation(positionData.Position, Quaternion.Euler(positionData.Rotation));

        /// <summary>
        /// Sets the position of a transform from a position data object.
        /// </summary>
        /// <param name="transform">The transform to change.</param>
        /// <param name="positionData">Position data to set.</param>
        public static void SetPosition(this Transform transform, PositionData positionData) =>
            transform.position = positionData.Position;

        /// <summary>
        /// Sets the rotation of a transform from a position data object.
        /// </summary>
        /// <param name="transform">The transform to change.</param>
        /// <param name="positionData">Position data to set.</param>
        public static void SetRotation(this Transform transform, PositionData positionData) =>
            transform.rotation = Quaternion.Euler(positionData.Rotation);

        /// <summary>
        /// Determines if a string is null, empty or whitespace.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullEmptyOrWhiteSpace(this string value) =>
            string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);

        /// <summary>
        /// Shallow clones a list.
        /// </summary>
        /// <param name="original">Original list.</param>
        /// <typeparam name="T">List genericity.</typeparam>
        /// <returns>A shallow clone of the list.</returns>
        public static List<T> ShallowClone<T>(this List<T> original)
        {
            List<T> clone = new List<T>();

            for (int i = 0; i < original.Count; ++i) clone.Add(original[i]);

            return clone;
        }

        /// <summary>
        /// Get a random element of a list.
        /// </summary>
        /// <param name="original">Original list.</param>
        /// <typeparam name="T">Type of element in the list.</typeparam>
        /// <returns>A random element of that list.</returns>
        public static T Random<T>(this List<T> original) => original[UnityEngine.Random.Range(0, original.Count)];

        /// <summary>
        /// Converts a Vector3 list to a Vector2 list.
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static List<Vector2> ToVector2List(this List<Vector3> original)
        {
            List<Vector2> newList = new List<Vector2>();

            for (int i = 0; i < original.Count; ++i) newList.Add(original[i]);

            return newList;
        }

        /// <summary>
        /// Converts a Vector2 list to a Vector3 list.
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static List<Vector3> ToVector3List(this List<Vector2> original)
        {
            List<Vector3> newList = new List<Vector3>();

            for (int i = 0; i < original.Count; ++i) newList.Add(original[i]);

            return newList;
        }

        /// <summary>
        /// Resize an array with the given rows and columns.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[,] ResizeArray<T>(this T[,] original, int rows, int cols)
        {
            T[,] newArray = new T[rows, cols];
            int minRows = Math.Min(rows, original.GetLength(0));
            int minCols = Math.Min(cols, original.GetLength(1));

            for (int i = 0; i < minRows; i++)
                for (int j = 0; j < minCols; j++)
                    newArray[i, j] = original[i, j];

            return newArray;
        }
        
        /// <summary>
        /// Gets all items for an enum value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetAllItems<T>()
        {
            foreach (object item in Enum.GetValues(typeof(T))) yield return (T)item;
        }

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