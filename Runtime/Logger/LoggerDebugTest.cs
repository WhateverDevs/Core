using log4net;
using UnityEngine;

namespace WhateverDevs.Core.Runtime.Logger
{
    public class LoggerDebugTest : MonoBehaviour
    {

        public static ILog Log = LogManager.GetLogger(typeof(LoggerDebugTest));
        // Start is called before the first frame update
        void Start()
        {
            Log.Error("Error");
            Log.Info("Info");
            Log.Fatal("Fatal");
        }

        // Update is called once per frame
        void Update()
        {
            Log.Debug("Debug");
        }
    }
}
