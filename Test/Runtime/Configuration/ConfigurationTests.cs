using NUnit.Framework;
using UnityEngine;
using WhateverDevs.Core.Runtime.Configuration;
using WhateverDevs.Core.Test.Editor.Configuration;
using Zenject;

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
        /// Set up a scene with an installer.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            GameObject context = new GameObject();
            SceneContext sceneContext = context.AddComponent<SceneContext>();

            ConfigurationTestsInstaller installer = ScriptableObject.CreateInstance<ConfigurationTestsInstaller>();

            testConfiguration = ScriptableObject.CreateInstance<TestConfiguration>();

            installer.ConfigurationsToInstall =
                new ConfigurationScriptableHolder[] {testConfiguration};

            sceneContext.ScriptableObjectInstallers = new[] {installer};

            Object.Instantiate(context);
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
            
            testConfiguration.ConfigurationName = "TestConfig.cfg";
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