using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using WhateverDevs.Core.Runtime.Configuration;
using WhateverDevs.Core.Test.Editor.Configuration;
using Zenject;
using Assert = NUnit.Framework.Assert;
using Object = UnityEngine.Object;

namespace WhateverDevs.Core.Test.Runtime.Configuration
{
    /// <summary>
    /// Runtime tests for configuration.
    /// </summary>
    public class ConfigurationTests
    {
        /// <summary>
        /// Configuration to test.
        /// </summary>
        private TestConfiguration testConfiguration;

        /// <summary>
        /// An object that needs the configuration manager injected.
        /// </summary>
        private ConfigTestObject testObject;

        /// <summary>
        /// Flag to know if the setup has been run once.
        /// Unity does not have a OneTimeSetUpAttribute equivalent.
        /// </summary>
        private bool firstTimeSetUpRan;

        /// <summary>
        /// Set up a scene with an installer.
        /// </summary>
        [UnitySetUp]
        public IEnumerator Setup()
        {
            if(firstTimeSetUpRan) yield break;
            
            // Extenject throws some exceptions during tests.
            LogAssert.ignoreFailingMessages = true;
            
            GameObject context = new GameObject();
            SceneContext sceneContext = context.AddComponent<SceneContext>();
            testObject = context.AddComponent<ConfigTestObject>();

            ConfigurationTestsInstaller installer = ScriptableObject.CreateInstance<ConfigurationTestsInstaller>();

            testConfiguration = ScriptableObject.CreateInstance<TestConfiguration>();
            testConfiguration.ConfigurationName = "TestConfig.cfg";

            installer.ConfigurationsToInstall =
                new ConfigurationScriptableHolder[] {testConfiguration};

            sceneContext.ScriptableObjectInstallers = new[] {installer};

            Object.Instantiate(context);
            
            yield return new WaitForEndOfFrame();

            firstTimeSetUpRan = true;
        }

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
        [Test]
        public void ConfigurationManagerUsage()
        {
            const string someStringValue = "Some string";
            const int someIntValue = 56;
            const string secondStringValue = "Another string";
            const int secondIntValue = 785;

            testConfiguration.ConfigurationData.SomeString = someStringValue;
            testConfiguration.ConfigurationData.SomeInt = someIntValue;
            testConfiguration.Save();

            Assert.IsTrue(testObject.ConfigurationManager.GetConfiguration(out TestConfigurationData testData));
            Assert.AreEqual(someStringValue, testData.SomeString);
            Assert.AreEqual(someIntValue, testData.SomeInt);

            testData.SomeString = secondStringValue;
            testData.SomeInt = secondIntValue;

            Assert.IsTrue(testObject.ConfigurationManager.SetConfiguration(testData));
            Assert.IsTrue(testObject.ConfigurationManager.GetConfiguration(out testData));
            Assert.AreEqual(secondStringValue, testData.SomeString);
            Assert.AreEqual(secondIntValue, testData.SomeInt);
        }
    }
}