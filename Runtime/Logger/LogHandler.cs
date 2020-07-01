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
        static LogHandler() => Initialize();

        private static bool initialized;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Initialize()
        {
            if (initialized) return;

            #if UNITY_EDITOR
            if (EditorApplication.isUpdating) return;
            #endif

            if (!Log4NetConfigProvider.ConfigExists)
            {
                #if UNITY_EDITOR
                if (EditorUtility.DisplayDialog("WhateverDevs' Core",
                                                "There is no LoggerConfiguration asset on the resources folder. The logger won't work. Do you want to automatically create one? If you are seen this message while importing the project, hit \"No\" (TODO: Fix this.)",
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

            // ReSharper disable once UnusedVariable
            FileInfo fileInfo = Log4NetConfigProvider.GetConfig(out string message);

            XmlConfigurator.Configure(fileInfo);

            #if UNITY_EDITOR
            // We don't want this message on the editor each time we recompile.
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                #endif
            {
                ILog objectLog = LogManager.GetLogger(typeof(LogHandler));
                objectLog.Info(message);
            }

            // TODO: Here is the bind to the default debug log in case we want to extend anytime it. 
            // In that case we need to rebuild the whole console window because no log would be printed.
            // If we want to copy the default log (it will be printed in the console but we can get the event when written)
            // we could use the Application.LogReceived event and do whatever we want with any log event.

            /*
            DefaultHandler = Debug.unityLogger.logHandler;
            handler = new LogHandler();
            Debug.unityLogger.logHandler = handle
            */

            initialized = true;
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