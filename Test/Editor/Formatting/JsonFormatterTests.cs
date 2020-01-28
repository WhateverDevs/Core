using System;
using System.Collections;
using System.Runtime.Serialization;
using NUnit.Framework;
using Packages.Core.Test.Editor.Common;
using UnityEngine;
using UnityEngine.TestTools;
using WhateverDevs.Core.Runtime.Formatting;

namespace WhateverDevs.Core.Test.Editor.Formatting
{
    /// <summary>
    /// Test class for the Json Formatter.
    /// </summary>
    public class JsonFormatterTests
    {
        /// <summary>
        /// Formatter instance.
        /// </summary>
        private IFormatter<string> formatter;

        /// <summary>
        /// Create an instance of the formatter.
        /// </summary>
        [SetUp]
        public void Setup() => formatter = new JsonFormatter();

        /// <summary>
        /// Tests the normal serialization and deserialization of data.
        /// </summary>
        [Test]
        public void NormalJsonSerializationAndDeserialization()
        {
            TestDataStructureOne firstTestData = new TestDataStructureOne {IntValue = 2};
            string resultString = formatter.To(firstTestData);
            Assert.AreEqual(resultString, "{\n    \"IntValue\": 2\n}");

            TestDataStructureOne firstResult = formatter.From<TestDataStructureOne>(resultString);

            Assert.AreEqual(firstTestData.IntValue,
                            firstResult.IntValue); // Classes are never the same as they two different objects.

            TestDataStructureTwo secondTestData = new TestDataStructureTwo()
                                                  {
                                                      BoolArray = new[] {true, false, true},
                                                      DataStructureOne = firstTestData
                                                  };

            resultString = formatter.To(secondTestData);

            Assert.AreEqual(resultString,
                            "{\n    \"BoolArray\": [\n        true,\n        false,\n        true\n    ],\n    \"DataStructureOne\": {\n        \"IntValue\": 2\n    }\n}");

            TestDataStructureTwo secondResult = formatter.From<TestDataStructureTwo>(resultString);
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
            NonSerializableClass testData = new NonSerializableClass {Whatever = "Whatever"};

            LogAssert.Expect(LogType.Error, "Data is not serializable, will not serialize.");
            Assert.Null(formatter.To(testData));

            LogAssert.Expect(LogType.Error, "Data type is not serializable, will not deserialize.");

            Assert.Throws<SerializationException>(() => formatter
                                                     .From<NonSerializableClass
                                                      >("This string is actually irrelevant."));

            yield return null;
        }

        /// <summary>
        /// Tries to parse a not parseable string.
        /// </summary>
        [Test]
        public void NotParseableString() =>
            Assert.Throws<ArgumentException>(() =>
                                                 formatter
                                                    .From<TestDataStructureOne
                                                     >("Some random data that can't be parsed."));
    }
}