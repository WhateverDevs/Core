using System;
using System.Collections;
using System.Runtime.Serialization;
using NUnit.Framework;
using UnityEngine.TestTools;
using WhateverDevs.Core.Runtime.Serialization;
using WhateverDevs.Core.Test.Editor.Common;

namespace WhateverDevs.Core.Test.Editor.Serialization
{
    /// <summary>
    /// Test class for the Json Serializer.
    /// </summary>
    public class JsonSerializerTests
    {
        /// <summary>
        /// Serializer instance.
        /// </summary>
        private ISerializer<string> serializer;

        /// <summary>
        /// Create an instance of the serializer.
        /// </summary>
        [SetUp]
        public void Setup() => serializer = new JsonSerializer();

        /// <summary>
        /// Tests the normal serialization and deserialization of data.
        /// </summary>
        [Test]
        public void NormalJsonSerializationAndDeserialization()
        {
            TestDataStructureOne firstTestData = new TestDataStructureOne {IntValue = 2};
            string resultString = serializer.To(firstTestData);
            Assert.AreEqual(resultString, "{\n    \"IntValue\": 2\n}");

            TestDataStructureOne firstResult = serializer.From<TestDataStructureOne>(resultString);

            Assert.AreEqual(firstTestData.IntValue,
                            firstResult.IntValue); // Classes are never the same as they two different objects.

            TestDataStructureTwo secondTestData = new TestDataStructureTwo
                                                  {
                                                      BoolArray = new[] {true, false, true},
                                                      DataStructureOne = firstTestData
                                                  };

            resultString = serializer.To(secondTestData);

            Assert.AreEqual(resultString,
                            "{\n    \"BoolArray\": [\n        true,\n        false,\n        true\n    ],\n    \"DataStructureOne\": {\n        \"IntValue\": 2\n    }\n}");

            TestDataStructureTwo secondResult = serializer.From<TestDataStructureTwo>(resultString);
            Assert.AreEqual(secondTestData.BoolArray, secondResult.BoolArray);
            Assert.AreEqual(secondTestData.DataStructureOne.IntValue, secondResult.DataStructureOne.IntValue);
        }

        /// <summary>
        /// Tries to use non serializable classes.
        /// </summary>
        /// <returns>Coroutine to be able to use LogAsserts.</returns>
        [UnityTest]
        public IEnumerator TryNonSerializableClass()
        {
            LogAssert.ignoreFailingMessages = true;
            NonSerializableClass testData = new NonSerializableClass {Whatever = "Whatever"};

            Assert.Null(serializer.To(testData));

            Assert.Throws<SerializationException>(() => serializer
                                                     .From<NonSerializableClass
                                                      >("This string is actually irrelevant."));

            LogAssert.ignoreFailingMessages = false;

            yield return null;
        }

        /// <summary>
        /// Tries to parse a not parseable string.
        /// </summary>
        [Test]
        public void NotParseableString() =>
            Assert.Throws<ArgumentException>(() =>
                                                 serializer
                                                    .From<TestDataStructureOne
                                                     >("Some random data that can't be parsed."));
    }
}