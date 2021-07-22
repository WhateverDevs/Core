using System;

namespace WhateverDevs.Core.Runtime.DataStructures
{
    /// <summary>
    /// Struct to store a tag.
    /// It just stores a string but the drawer comes alone without having to add attributes.
    /// </summary>
    [Serializable]
    public struct Tag
    {
        /// <summary>
        /// Value of the tag.
        /// </summary>
        public string Value;

        /// <summary>
        /// Override the to string method to give the actual value.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Value;

        /// <summary>
        /// Auto cast this struct to a string.
        /// </summary>
        /// <param name="tag">The tag to cast.</param>
        /// <returns>The string value.</returns>
        public static implicit operator string(Tag tag) => tag.ToString();

        /// <summary>
        /// Auto cast a string to this struct.
        /// </summary>
        /// <param name="value">The string to cast.</param>
        /// <returns>A Tag struct with the string as a tag.</returns>
        public static implicit operator Tag(string value) => new Tag {Value = value};
    }
}