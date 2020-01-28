using System;

namespace Packages.Core.Test.Editor.Common
{
    /// <summary>
    /// Some data structures to be used in tests.
    /// </summary>
    [Serializable]
    public class TestDataStructureOne
    {
        public int IntValue;
    }

    [Serializable]
    public class TestDataStructureTwo
    {
        public bool[] BoolArray;

        public TestDataStructureOne DataStructureOne;
    }

    public class NonSerializableClass
    {
        public string Whatever;
    }
}