using System.IO;
using NUnit.Framework;
using WhateverDevs.Core.Runtime.Formatting;
using WhateverDevs.Core.Runtime.Serialization;
using WhateverDevs.Core.Test.Editor.Common;

namespace WhateverDevs.Core.Test.Editor.Serialization
{
    public class JsonFileSerializerTests
    {
        /// <summary>
        /// Reference to the serializer.
        /// </summary>
        private ISerializer serializer;

        /// <summary>
        /// Path to the folder where the tests can happen.
        /// </summary>
        private const string TestingFolder = "SerializingTests";

        /// <summary>
        /// Create an instance of the serializer.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            serializer = new JsonFileSerializer {Formatter = new JsonFormatter()};
            if (!Directory.Exists(TestingFolder)) Directory.CreateDirectory(TestingFolder);
        }

        /// <summary>
        /// Normal examples of serialization.
        /// </summary>
        [Test]
        public void NormalSerialization()
        {
            TestDataStructureOne firstTestData = new TestDataStructureOne {IntValue = 3};
            const string firstDestination = TestingFolder + "/FirstTestData.json";
            serializer.Save(firstTestData, firstDestination);
            serializer.Load(out TestDataStructureOne firstResult, firstDestination);
            Assert.AreEqual(firstTestData.IntValue, firstResult.IntValue);

            TestDataStructureTwo secondTestData = new TestDataStructureTwo
                                                  {
                                                      BoolArray = new[] {true, false, true},
                                                      DataStructureOne = firstTestData
                                                  };

            const string secondDestination = TestingFolder + "/SecondTestData.json";
            serializer.Save(secondTestData, secondDestination);
            serializer.Load(out TestDataStructureTwo secondResult, secondDestination);
            Assert.AreEqual(secondTestData.BoolArray, secondTestData.BoolArray);
            Assert.AreEqual(secondTestData.DataStructureOne.IntValue, secondResult.DataStructureOne.IntValue);
        }
    }
}