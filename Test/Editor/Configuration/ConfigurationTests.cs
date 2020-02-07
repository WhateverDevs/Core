using System.Linq;
using NUnit.Framework;
using Packages.Core.Runtime.Serialization;
using Packages.Core.Test.Editor.Configuration;
using UnityEngine;
using WhateverDevs.Core.Runtime.Configuration;
using WhateverDevs.Core.Runtime.Formatting;

namespace WhateverDevs.Core.Test.Editor.Configuration
{
    /// <summary>
    /// Class that tests the configuration system on editor.
    /// </summary>
    public class ConfigurationTests
    {
        /// <summary>
        /// Serializers to use during the tests.
        /// </summary>
        private ISerializer[] serializers;

        /// <summary>
        /// Setup before starting tests.
        /// </summary>
        [SetUp]
        public void Setup() =>
            serializers = new ISerializer[] {new ConfigurationJsonFileSerializer {Formatter = new JsonFormatter()}};

        /// <summary>
        /// Test normal json file serialization.
        /// </summary>
        [Test]
        public void NormalJsonFileSerialization()
        {
            const string someStringValue = "Some string";
            const int someIntValue = 56;
            const string secondStringValue = "Another string";
            const int secondIntValue = 785;
            
            TestConfiguration testConfiguration = ScriptableObject.CreateInstance<TestConfiguration>();
            testConfiguration.ConfigurationName = "TestConfig.cfg";
            testConfiguration.Serializers = serializers.ToList();
            testConfiguration.ConfigurationData.SomeString = someStringValue;
            testConfiguration.ConfigurationData.SomeInt = someIntValue;

            Assert.IsTrue(testConfiguration.Save());

            testConfiguration.ConfigurationData.SomeString = secondStringValue;
            testConfiguration.ConfigurationData.SomeInt = secondIntValue;

            Assert.IsTrue(testConfiguration.Load());

            Assert.AreEqual(someStringValue, testConfiguration.ConfigurationData.SomeString);
            Assert.AreEqual(someIntValue, testConfiguration.ConfigurationData.SomeInt);
            
            testConfiguration.ConfigurationData.SomeString = secondStringValue;
            testConfiguration.ConfigurationData.SomeInt = secondIntValue;
            
            Assert.IsTrue(testConfiguration.Save());
            
            testConfiguration.ConfigurationData.SomeString = someStringValue;
            testConfiguration.ConfigurationData.SomeInt = someIntValue;
            
            Assert.IsTrue(testConfiguration.Load());

            Assert.AreEqual(secondStringValue, testConfiguration.ConfigurationData.SomeString);
            Assert.AreEqual(secondIntValue, testConfiguration.ConfigurationData.SomeInt);
        }
    }
}