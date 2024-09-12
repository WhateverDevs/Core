using System.IO;
using JetBrains.Annotations;
using log4net.Core;
using log4net.Layout.Pattern;

namespace WhateverDevs.Core.Runtime.Logger.Patterns
{
    [UsedImplicitly]
    public class GenericClassNamePatternLayoutConverter : PatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            string name = loggingEvent.LoggerName;

            name = name.Split('`')[0];
            name = name.Split('.')[^1];

            writer.Write(name);
        }
    }
}