using System;
using Packages.Core.Test.Editor.Configuration;
using UnityEngine;
using WhateverDevs.Core.Runtime.Configuration;

namespace WhateverDevs.Core.Test.Editor.Configuration
{
    /// <summary>
    /// Testable example of a configuration.
    /// </summary>
    [CreateAssetMenu(menuName = "WhateverDevs/Test/TestConfiguration", fileName = "TestConfiguration")]
    public class TestConfiguration : ConfigurationScriptableHolderUsingFirstValidSerializer<TestConfigurationData>
    {
    }
}

namespace Packages.Core.Test.Editor.Configuration
{
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