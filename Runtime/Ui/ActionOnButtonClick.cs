using UnityEngine;
using UnityEngine.UI;
using WhateverDevs.Core.Behaviours;

#if ODIN_INSPECTOR_3
using Sirenix.OdinInspector;
#endif

namespace WhateverDevs.Core.Runtime.Ui
{
    /// <summary>
    /// Abstract class for classes that listen to a button click event.
    /// </summary>
    /// <typeparam name="T">The inheriting class.</typeparam>
    public abstract class ActionOnButtonClick<T> : WhateverBehaviour<T> where T : ActionOnButtonClick<T>
    {
        /// <summary>
        /// Is the button in the same game object?
        /// </summary>
        [Tooltip("Is the button in the same game object?")]
        public bool ButtonInTheSameObject = true;

        /// <summary>
        /// Accessor to the button.
        /// </summary>
        public Button Button => ButtonInTheSameObject ? GetCachedComponent<Button>() : ButtonReference;

        /// <summary>
        /// Reference to the button.
        /// </summary>
        #if ODIN_INSPECTOR_3
        [HideIf("ButtonInTheSameObject")]
        #endif
        [SerializeField]
        private Button ButtonReference;

        /// <summary>
        /// Subscribe.
        /// </summary>
        protected virtual void OnEnable() => Button.onClick.AddListener(ButtonClicked);

        /// <summary>
        /// Unsubscribe.
        /// </summary>
        protected virtual void OnDisable() => Button.onClick.RemoveListener(ButtonClicked);

        /// <summary>
        /// Called when the button is clicked.
        /// </summary>
        protected abstract void ButtonClicked();
    }
}