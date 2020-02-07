using System;
using WhateverDevs.Core.Runtime.Configuration;

namespace Packages.Core.Test.Editor.Configuration
{
    /// <summary>
    /// Testable example of a configuration.
    /// </summary>
    public class TestConfiguration : ConfigurationScriptableHolderUsingFirstValidSerializer<TestConfigurationData>
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
    }
}