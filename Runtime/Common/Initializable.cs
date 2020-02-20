namespace WhateverDevs.Core.Runtime.Common
{
    /// <summary>
    /// Bas class for initializable classes.
    /// </summary>
    public abstract class Initializable<TInitializable> : Loggable<TInitializable>, IInitializable
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
                GetLogger().Error(className + " is already initialized.");
                return;
            }

            GetLogger().Info("Initializing " + className + "...");

            Initialized = PreInitialization();

            if (!Initialized)
            {
                GetLogger().Error(className + " didn't preinitialize correctly.");
                return;
            }

            Initialized = OnInitialization();

            if (!Initialized)
            {
                GetLogger().Error(className + " didn't initialize correctly.");
                return;
            }

            Initialized = PostInitialization();

            if (!Initialized)
            {
                GetLogger().Error(className + " didn't postinitialize correctly.");
                return;
            }

            GetLogger().Info(className + " initialized.");
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