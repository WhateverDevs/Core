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
        /// Raised when an error or exception has been log.
        /// Be careful not to log an error on the method that receives this event or you may cause an overflow!
        /// </summary>
        public Action<string> ErrorLogged;

        /// <summary>
        /// Called when the app is quitting.
        /// </summary>
        public Action AppQuitting;

        /// <summary>
        /// Register a logging event, this should only be called by the logging event appender.
        /// </summary>
        /// <param name="log"></param>
        /// <param name="type"></param>
        public void RegisterLogEvent(string log, LogType type)
        {
            if (type == LogType.Error || type == LogType.Exception) ErrorLogged?.Invoke(log);
        }

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