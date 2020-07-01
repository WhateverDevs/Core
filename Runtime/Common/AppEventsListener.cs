using System;

namespace WhateverDevs.Core.Runtime.Common
{
    /// <summary>
    /// Class that other pure C# class can subscribe to to listen to unity app events.
    /// </summary>
    public class AppEventsListener : Singleton<AppEventsListener>
    {
        /// <summary>
        /// Called when the app is quitting.
        /// </summary>
        public Action AppQuitting;

        /// <summary>
        /// Call the app quitting event.
        /// </summary>
        private void OnApplicationQuit() => AppQuitting?.Invoke();
    }
}