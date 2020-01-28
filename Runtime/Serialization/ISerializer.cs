using WhateverDevs.Core.Runtime.Formatting;

namespace Packages.Core.Runtime.Serialization
{
    /// <summary>
    /// Serializer that saves and retrieves data from a persistent storage.
    /// </summary>
    /// <typeparam name="TFormatter">Type of formatter the serializer will use.</typeparam>
    /// <typeparam name="TFormatted">Type of the data to format.</typeparam>
    public interface ISerializer<TFormatter, TFormatted> where TFormatter : IFormatter<TFormatted>
    {
        /// <summary>
        /// Formatter the serializer will use.
        /// </summary>
        TFormatter Formatter { get; set; }

        /// <summary>
        /// Save the given data to persistent storage.
        /// </summary>
        /// <param name="data">Data to save.</param>
        /// <param name="destination">Where to save the data.</param>
        /// <typeparam name="TOriginal">Type of the original data.</typeparam>
        /// <returns>True if it was successful.</returns>
        bool Save<TOriginal>(TOriginal data, string destination);

        /// <summary>
        /// Load the data from persistent storage.
        /// </summary>
        /// <param name="data">Variable to store the data into.</param>
        /// <param name="origin">Where to retrieve the data.</param>
        /// <typeparam name="TOriginal">Type of the original data.</typeparam>
        /// <returns>True if it was successful.</returns>
        bool Load<TOriginal>(out TOriginal data, string origin);
    }
}