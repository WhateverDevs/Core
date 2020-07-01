using UnityEngine;
using WhateverDevs.Core.Runtime.Common;
using WhateverDevs.Core.Runtime.Configuration;
using Zenject;

namespace WhateverDevs.Core.Test.Runtime.Configuration
{
    public class ConfigTestObject : LoggableMonoBehaviour<ConfigTestObject>
    {
        public IConfigurationManager ConfigurationManager;

        [Inject]
        public void Construct(IConfigurationManager configurationManager)
        {
            Debug.Log("Construct");
            ConfigurationManager = configurationManager;
        }
    }
}