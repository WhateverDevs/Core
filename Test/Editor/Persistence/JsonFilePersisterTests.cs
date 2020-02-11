using System.IO;
using NUnit.Framework;
using WhateverDevs.Core.Runtime.Formatting;
using WhateverDevs.Core.Runtime.Persistence;
using WhateverDevs.Core.Test.Editor.Common;

namespace WhateverDevs.Core.Test.Editor.Persistence
{
    public class JsonFilePersisterTests
    {
        /// <summary>
        /// Reference to the persister.
        /// </summary>
        private IPersister persister;

        /// <summary>
        /// Path to the folder where the tests can happen.
        /// </summary>
        private const string TestingFolder = "PersistenceTests";

        /// <summary>
        /// Create an instance of the persister.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            persister = new JsonFilePersister {Formatter = new JsonFormatter()};
            if (!Directory.Exists(TestingFolder)) Directory.CreateDirectory(TestingFolder);
        }

        /// <summary>
        /// Normal examples of serialization.
        /// </summary>
        [Test]
        public void NormalPersistence()
        {
            TestDataStructureOne firstTestData = new TestDataStructureOne {IntValue = 3};
            const string firstDestination = TestingFolder + "/FirstTestData.json";
            persister.Save(firstTestData, firstDestination);
            persister.Load(out TestDataStructureOne firstResult, firstDestination);
            Assert.AreEqual(firstTestData.IntValue, firstResult.IntValue);

            TestDataStructureTwo secondTestData = new TestDataStructureTwo
                                                  {
                                                      BoolArray = new[] {true, false, true},
                                                      DataStructureOne = firstTestData
                                                  };

            const string secondDestination = TestingFolder + "/SecondTestData.json";
            persister.Save(secondTestData, secondDestination);
            persister.Load(out TestDataStructureTwo secondResult, secondDestination);
            Assert.AreEqual(secondTestData.BoolArray, secondTestData.BoolArray);
            Assert.AreEqual(secondTestData.DataStructureOne.IntValue, secondResult.DataStructureOne.IntValue);
        }
    }
}