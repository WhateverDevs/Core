namespace WhateverDevs.Core.Runtime.Common
{
    /// <summary>
    /// Interface for classes that need to be initialized.
    /// </summary>
    public interface IInitializable
    {
        /// <summary>
        /// Flag to know if is initialized.
        /// </summary>
        bool Initialized { get; set; }
        
        /// <summary>
        /// Initialize the class.
        /// </summary>
        void Initialize();
    }
}