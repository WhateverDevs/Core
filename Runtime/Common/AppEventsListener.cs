using System;
using UnityEngine;

namespace WhateverDevs.Core.Runtime.Common
{
    /// <summary>
    /// Class that other pure C# class can subscribe to to listen to unity app events.
    /// </summary>
    public class AppEventsListener : Singleton<AppEventsListener>
    {
        /// <summary>
        /// Update event.
        /// </summary>
        public Action<float> AppUpdate;

        /// <summary>
        /// Physics update event.
        /// </summary>
        public Action<float> PhysicsUpdate;

        /// <summary>
        /// Called when the app is quitting.
        /// </summary>
        public Action AppQuitting;

        /// <summary>
        /// Call the event.
        /// </summary>
        private void Update() => AppUpdate?.Invoke(Time.deltaTime);

        /// <summary>
        /// Call the event.
        /// </summary>
        private void FixedUpdate() => PhysicsUpdate?.Invoke(Time.fixedDeltaTime);

        /// <summary>
        /// Call the app quitting event.
        /// </summary>
        private void OnApplicationQuit() => AppQuitting?.Invoke();
    }
}