namespace WhateverDevs.Core.Runtime.Formatting
{
    /// <summary>
    /// Interface that defines a formatter that is used to translate between different types of data.
    /// </summary>
    /// <typeparam name="TFormatted"></typeparam>
    public interface IFormatter<TFormatted>
    {
        /// <summary>
        /// Format the data.
        /// </summary>
        /// <param name="original">Data in the original type, typeof(TOriginal).IsSerializable = true.</param>
        /// <typeparam name="TOriginal">Type to format the data to.</typeparam>
        /// <returns>Data in the formatted type.</returns>
        TFormatted To<TOriginal>(TOriginal original);

        /// <summary>
        /// Get the data in the original format.
        /// </summary>
        /// <param name="formatted">Data in the formatted type.</param>
        /// <typeparam name="TOriginal">Original type of the data.</typeparam>
        /// <returns>Data in the original type.</returns>
        TOriginal From<TOriginal>(TFormatted formatted);
    }
}