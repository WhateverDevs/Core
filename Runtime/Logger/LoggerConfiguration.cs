using System;
using System.Collections.Generic;
using UnityEngine;
using WhateverDevs.Core.Runtime.Configuration;
using WhateverDevs.Core.Runtime.Persistence;
using Configuration = WhateverDevs.Core.Runtime.Configuration;

namespace WhateverDevs.Core.Runtime.Logger
{

    [Serializable]
    public class LoggerConfigurationData : ConfigurationData
    {
        public string LogPath;
    }

    [CreateAssetMenu(fileName = "LoggerConfiguration", menuName = "Configuration/Logger")]
    public class LoggerConfiguration : ConfigurationScriptableHolderUsingFirstValidPersister<LoggerConfigurationData>
    {
        
    }
}
