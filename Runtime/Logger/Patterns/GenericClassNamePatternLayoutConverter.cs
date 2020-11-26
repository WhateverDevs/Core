using System.IO;
using log4net.Core;
using log4net.Layout.Pattern;

namespace WhateverDevs.Core.Runtime.Logger.Patterns
{
    public class GenericClassNamePatternLayoutConverter : PatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            string name = loggingEvent.LoggerName;

            name = name.Split('`')[0];
            
            writer.Write(name);
        }
    }
}