using log4net;
using UnityEngine;
using WhateverDevs.Core.Behaviours;

namespace WhateverDevs.Core.Runtime.Common
{
    /// <summary>
    ///     You probably shouldn't be using this class for anything.
    ///     In 99.9% of the cases you can solve it with an injectable interface.
    ///     Be aware this will not prevent a non singleton constructor
    ///     such as `TSingleton myTSingleton = new TSingleton();`
    ///     To prevent that, add `protected TSingleton () {}` to your singleton class.
    ///     As a note, this is made as MonoBehaviour because we need Coroutines.
    /// </summary>
    public abstract class Singleton<TSingleton> : WhateverBehaviour<TSingleton>
        where TSingleton : Singleton<TSingleton>
    {
        /// <summary>
        ///     Path where the prefab would sit, you can override this in your implementation.
        /// </summary>
        private const string PrefabPath = "Prefabs/Singleton/";

        /// <summary>
        ///     Prefab name, you can override this in your implementation.
        /// </summary>
        protected static readonly string PrefabName = typeof(TSingleton).Name;

        /// <summary>
        ///     Singleton instance.
        /// </summary>
        private static TSingleton instance;

        /// <summary>
        ///     Lock to prevent multiple instantiations.
        /// </summary>
        // ReSharper disable once StaticMemberInGenericType
        private static readonly object Lock = new object();

        /// <summary>
        ///     Path to the prefab of this singleton.
        ///     You can create the prefab by yourself or it will autocreate if you don't.
        /// </summary>
        private static TSingleton Prefab => Resources.Load<TSingleton>(PrefabPath + PrefabName);

        /// <summary>
        ///     Public singleton access, this is where magic happens.
        /// </summary>
        public static TSingleton Instance
        {
            get
            {
                if (ApplicationIsQuitting)
                {
                    StaticLogger.Warn("Instance '"
                                    + PrefabName
                                    + "' already destroyed on application quit."
                                    + " Won't create again - returning null.");

                    return null;
                }

                lock (Lock)
                {
                    if (instance != null)
                    {
                        CheckDontDestroyOnLoad();
                        return instance;
                    }

                    instance = FindFirstObjectByType<TSingleton>();
                    CheckDontDestroyOnLoad();

                    if (FindObjectsByType<TSingleton>(FindObjectsSortMode.None).Length > 1)
                    {
                        StaticLogger.Error("Something went really wrong "
                                         + " - there should never be more than 1 singleton!"
                                         + " Reopening the scene might fix it.");

                        return instance;
                    }

                    if (instance != null) return instance;
                    GameObject singleton;

                    if (Prefab == null)
                    {
                        StaticLogger.Info("There is no prefab for " + PrefabName + ". Creating a new GameObject.");

                        singleton = new GameObject();
                        instance = singleton.AddComponent<TSingleton>();
                    }
                    else
                    {
                        StaticLogger.Info("Found a prefab for " + Prefab.name + ". Instantiating it.");
                        singleton = Instantiate(Prefab).gameObject;
                        instance = singleton.GetComponent<TSingleton>();
                    }

                    singleton.name = "[Singleton] " + PrefabName;

                    DontDestroyOnLoad(singleton);

                    return instance;
                }
            }
        }

        /// <summary>
        /// Checks if the singleton is a DontDestroyOnLoad and sets it up if it isn't.
        /// </summary>
        private static void CheckDontDestroyOnLoad()
        {
            if (instance == null) return;
            if (instance.transform.parent != null) return;
            if (instance.gameObject.IsDontDestroyOnLoad()) return;

            StaticLogger.Info(instance.name + " is not DontDestroyOnLoad so setting it up.");

            DontDestroyOnLoad(instance);
        }

        /// <summary>
        ///     Flag to know when the application is quitting.
        /// </summary>
        // ReSharper disable once StaticMemberInGenericType
        private static bool ApplicationIsQuitting { get; set; }

        /// <summary>
        ///     When Unity quits, it destroys objects in a random order.
        ///     In principle, a Singleton is only destroyed when application quits.
        ///     If any script calls Instance after it have been destroyed,
        ///     it will create a buggy ghost object that will stay on the Editor scene
        ///     even after stopping playing the Application. Really bad!
        ///     So, this was made to be sure we're not creating that buggy ghost object.
        /// </summary>
        public virtual void OnDestroy() => ApplicationIsQuitting = true;
    }
}