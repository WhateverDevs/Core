using System.IO;
using log4net.Util;

namespace WhateverDevs.Core.Runtime.Logger
{
    public class PersistentDataStoreConverter : PatternConverter {
        protected override void Convert(TextWriter writer, object state) 
        {
            writer.Write(UnityEngine.Application.persistentDataPath); 
        }
    }
}
