namespace WhateverDevs.Core.Runtime.Persistence
{
    /// <summary>
    /// Persister that saves and retrieves data from a persistent storage.
    /// </summary>
    public interface IPersister
    {
        /// <summary>
        /// Save the given data to persistent storage.
        /// </summary>
        /// <param name="data">Data to save.</param>
        /// <param name="destination">Where to save the data.</param>
        /// <param name="suppressErrors">Don't log errors, this is useful for systems that can still work
        /// without the resource existing, like the configuration manager.</param>
        /// <typeparam name="TOriginal">Type of the original data.</typeparam>
        /// <returns>True if it was successful.</returns>
        bool Save<TOriginal>(TOriginal data, string destination, bool suppressErrors = false);

        /// <summary>
        /// Load the data from persistent storage.
        /// </summary>
        /// <param name="data">Variable to store the data into.</param>
        /// <param name="origin">Where to retrieve the data.</param>
        /// <param name="suppressErrors">Don't log errors, this is useful for systems that can still work
        /// without the resource existing, like the configuration manager.</param>
        /// <typeparam name="TOriginal">Type of the original data.</typeparam>
        /// <returns>True if it was successful.</returns>
        bool Load<TOriginal>(out TOriginal data, string origin, bool suppressErrors = false);
    }
}