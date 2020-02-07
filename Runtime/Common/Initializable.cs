using UnityEngine;

namespace WhateverDevs.Core.Runtime.Common
{
    /// <summary>
    /// Bas class for initializable classes.
    /// </summary>
    public abstract class Initializable<TInitializable> : IInitializable
        where TInitializable : Initializable<TInitializable>
    {
        /// <summary>
        /// Flag to know if is initialized.
        /// </summary>
        public bool Initialized { get; set; }

        /// <summary>
        /// Initialize the class.
        /// </summary>
        public void Initialize()
        {
            string className = typeof(TInitializable).ToString();
            
            if (Initialized)
            {
                Debug.Log(className + " is already initialized."); // TODO: change to custom log system.
                return;
            }

            Debug.Log("Initializing " + className + "..."); // TODO: change to custom log system.

            Initialized = PreInitialization();

            if (!Initialized)
            {
                Debug.LogError(className + " didn't preinitialize correctly."); // TODO: change to custom log system.
                return;
            }

            Initialized = OnInitialization();

            if (!Initialized)
            {
                Debug.LogError(className + " didn't initialize correctly."); // TODO: change to custom log system.
                return;
            }

            Initialized = PostInitialization();

            if (!Initialized)
            {
                Debug.LogError(className + " didn't postinitialize correctly."); // TODO: change to custom log system.
                return;
            }

            Debug.Log(className + " initialized."); // TODO: change to custom log system.
        }

        /// <summary>
        /// Called before initialization.
        /// </summary>
        /// <returns>True if preinitialization was correct.</returns>
        protected virtual bool PreInitialization() => true;

        /// <summary>
        /// Called during initialization.
        /// </summary>
        protected virtual bool OnInitialization() => true;

        /// <summary>
        /// Called after initialization.
        /// </summary>
        /// <returns>True if postinitialization was correct.</returns>
        protected virtual bool PostInitialization() => true;
    }
}