using System;
using UnityEngine;
using WhateverDevs.Core.Runtime.Configuration;

namespace WhateverDevs.Core.Test.Editor.Configuration
{
    /// <summary>
    /// Testable example of a configuration.
    /// </summary>
    [CreateAssetMenu(menuName = "WhateverDevs/Test/TestConfiguration", fileName = "TestConfiguration")]
    public class TestConfiguration : ConfigurationScriptableHolderUsingFirstValidPersister<TestConfigurationData>
    {
    }

    /// <summary>
    /// Testable example of configuration data.
    /// </summary>
    [Serializable]
    public class TestConfigurationData : ConfigurationData
    {
        public string SomeString;

        public int SomeInt;

        protected override TConfigurationData Clone<TConfigurationData>() =>
            new TestConfigurationData
            {
                SomeString = SomeString,
                SomeInt = SomeInt
            } as TConfigurationData;
    }
}