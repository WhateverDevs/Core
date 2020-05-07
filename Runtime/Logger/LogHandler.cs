using System;
using System.IO;
using log4net;
using log4net.Config;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace WhateverDevs.Core.Runtime.Logger
{
    #if UNITY_EDITOR
    [InitializeOnLoad]
    #endif
    public class LogHandler : ILogHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LogHandler));

        /*
    public static ILogHandler DefaultHandler { get; private set; }
    private static LogHandler handler;
    */

        static LogHandler()
        {
            Initialize();
        }

        #if !UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod]
        #endif
        private static void Initialize()
        {
            if (!Log4NetConfigProvider.ConfigExists)
            {
                #if UNITY_EDITOR
                if (EditorUtility.DisplayDialog("WhateverDevs' Core",
                                                "There is no LoggerConfiguration asset on the resources folder. The logger won't work. Do you want to automatically create one?",
                                                "Sure!",
                                                "No, I don't need logs"))
                    Log4NetConfigProvider.CreateLoggerConfigInResources();
                else
                {
                    Debug.LogError("There is no LoggerConfiguration asset on the resources folder, create one or the logger won't work!");
                    return;
                }
                #else
                    Debug.LogError("There is no LoggerConfiguration asset on the resources folder, create one or the logger won't work!");
                    return;
                #endif
            }

            if (!Log4NetConfigProvider.DefaultConfigSet)
            {
                #if UNITY_EDITOR
                if (EditorUtility.DisplayDialog("WhateverDevs' Core",
                                                "The LoggerConfiguration asset does not reference the default config. The logger won't work. Do you want to automatically fix it?",
                                                "Sure!",
                                                "No, I don't need logs"))
                    Log4NetConfigProvider.FixDefaultLoggerReference();
                else
                {
                    Debug.LogError("The LoggerConfiguration asset does not reference the default config, set it or the logger won't work!");
                    return;
                }
                #else
                Debug.LogError("The LoggerConfiguration asset does not reference the default config, set it or the logger won't work!");
                return;
                #endif
            }

            FileInfo fileInfo = Log4NetConfigProvider.GetConfig(out string message);

            XmlConfigurator.Configure(fileInfo);

            #if !UNITY_EDITOR
            Log.Info(message);
            #endif

            // TODO: Here is the bind to the default debug log in case we want to extend anytime it. 
            // In that case we need to rebuild the whole console window because no log would be printed.
            // If we want to copy the default log (it will be printed in the console but we can get the event when written)
            // we could use the Application.LogReceived event and do whatever we want with any log event.

            /*
        DefaultHandler = Debug.unityLogger.logHandler;
        handler = new LogHandler();
        Debug.unityLogger.logHandler = handle
        */
        }

        public void LogException(Exception exception, UnityEngine.Object context)
        {
            ILog objectLog = LogManager.GetLogger(context.GetType());
            objectLog.Error(exception.Message);
        }

        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            ILog objectLog = LogManager.GetLogger(context.GetType());

            switch (logType)
            {
                case LogType.Error:
                    objectLog.Error(format);
                    break;
                case LogType.Warning:
                    objectLog.Warn(format);
                    break;
                case LogType.Log:
                    objectLog.Info(format);
                    break;
                case LogType.Exception:
                    objectLog.Fatal(format);
                    break;
                case LogType.Assert:
                    objectLog.Error(format);
                    break;
                default:
                    objectLog.Debug(format);
                    break;
            }
        }
    }
}