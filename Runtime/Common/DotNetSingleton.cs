using System;

namespace WhateverDevs.Core.Runtime.Common
{
    /// <summary>
    /// Pure .net lazy singleton implementation.
    /// Refer to: https://csharpindepth.com/articles/singleton#lazy
    /// </summary>
    public class DotNetSingleton<T> : Loggable<T> where T : DotNetSingleton<T>, new()
    {
        private static readonly Lazy<T> Lazy = new Lazy<T>(() => new T());

        public static T Instance => Lazy.Value;

        protected DotNetSingleton()
        {
        }
    }
}