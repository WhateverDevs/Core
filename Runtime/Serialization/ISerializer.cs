namespace WhateverDevs.Core.Runtime.Serialization
{
    /// <summary>
    /// Interface that defines a serializer that is used to translate between different types of data.
    /// </summary>
    /// <typeparam name="TSerialized"></typeparam>
    public interface ISerializer<TSerialized>
    {
        /// <summary>
        /// Serialize the data.
        /// </summary>
        /// <param name="original">Data in the original type, typeof(TOriginal).IsSerializable = true.</param>
        /// <typeparam name="TOriginal">Type to serialize the data to.</typeparam>
        /// <returns>Data in the serialized type.</returns>
        TSerialized To<TOriginal>(TOriginal original);

        /// <summary>
        /// Get the data in the original format.
        /// </summary>
        /// <param name="serialized">Data in the serialized type.</param>
        /// <typeparam name="TOriginal">Original type of the data.</typeparam>
        /// <returns>Data in the original type.</returns>
        TOriginal From<TOriginal>(TSerialized serialized);
    }
}