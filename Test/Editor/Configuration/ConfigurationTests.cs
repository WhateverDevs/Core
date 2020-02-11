using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Packages.Core.Test.Editor.Configuration;
using UnityEngine;
using UnityEngine.TestTools;
using WhateverDevs.Core.Runtime.Configuration;
using WhateverDevs.Core.Runtime.Formatting;
using WhateverDevs.Core.Runtime.Persistence;

namespace WhateverDevs.Core.Test.Editor.Configuration
{
    /// <summary>
    /// Class that tests the configuration system on editor.
    /// </summary>
    public class ConfigurationTests
    {
        /// <summary>
        /// Persisters to use during the tests.
        /// </summary>
        private IPersister[] persisters;

        /// <summary>
        /// Setup before starting tests.
        /// </summary>
        [SetUp]
        public void Setup() =>
            persisters = new IPersister[] {new ConfigurationJsonFilePersister {Formatter = new JsonFormatter()}};

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
            testConfiguration.Persisters = persisters.ToList();
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

        /// <summary>
        /// Test the configuration manager.
        /// </summary>
        [UnityTest]
        public IEnumerator ConfigurationManagerUsage()
        {
            const string someStringValue = "Some string";
            const int someIntValue = 56;
            const string secondStringValue = "Another string";
            const int secondIntValue = 785;

            TestConfiguration testConfiguration = ScriptableObject.CreateInstance<TestConfiguration>();
            testConfiguration.ConfigurationName = "TestConfig.cfg";
            testConfiguration.Persisters = persisters.ToList();
            testConfiguration.ConfigurationData.SomeString = someStringValue;
            testConfiguration.ConfigurationData.SomeInt = someIntValue;
            testConfiguration.Save();

            IConfigurationManager configurationManager = new ConfigurationManager();
            configurationManager.Configurations = new List<IConfiguration> {testConfiguration};

            // Call the initialization manually again as our fake injection does not work before the constructor is called.
            ((ConfigurationManager) configurationManager).Initialize();

            Assert.IsTrue(configurationManager.GetConfiguration(out TestConfigurationData testData));
            Assert.AreEqual(someStringValue, testData.SomeString);
            Assert.AreEqual(someIntValue, testData.SomeInt);

            testData.SomeString = secondStringValue;
            testData.SomeInt = secondIntValue;
            
            Assert.IsTrue(configurationManager.SetConfiguration(testData));
            Assert.IsTrue(configurationManager.GetConfiguration(out testData));
            Assert.AreEqual(secondStringValue, testData.SomeString);
            Assert.AreEqual(secondIntValue, testData.SomeInt);

            yield return null;
        }
    }
}