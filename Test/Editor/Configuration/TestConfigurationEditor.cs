using Packages.Core.Test.Editor.Configuration;
using UnityEditor;
using WhateverDevs.Core.Editor.Configuration;
using WhateverDevs.Core.Test.Editor.Configuration;

namespace WhateverDevs.Core.Test.Runtime.Configuration
{
    [CustomEditor(typeof(TestConfiguration))]
    public class TestConfigurationEditor : ConfigurationScriptableHolderEditor<TestConfiguration, TestConfigurationData>
    {
        
    }
}