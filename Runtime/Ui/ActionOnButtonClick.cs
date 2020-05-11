using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using WhateverDevs.Core.Runtime.Common;

namespace WhateverDevs.Core.Runtime.Ui
{
    /// <summary>
    /// Abstract class for classes that listen to a button click event.
    /// </summary>
    /// <typeparam name="T">The inheriting class.</typeparam>
    public abstract class ActionOnButtonClick<T> : LoggableMonoBehaviour<T> where T : ActionOnButtonClick<T>
    {
        /// <summary>
        /// Is the button in the same game object?
        /// </summary>
        [Tooltip("Is the button in the same game object?")]
        public bool ButtonInTheSameObject = true;

        /// <summary>
        /// Reference to the button.
        /// </summary>
        [HideIf("ButtonInTheSameObject")]
        public Button Button;

        /// <summary>
        /// Subscribe.
        /// </summary>
        protected virtual void OnEnable()
        {
            if (ButtonInTheSameObject) Button = GetComponent<Button>();

            if (Button == null) GetLogger().Error("Button reference has not been assigned.");

            Button.onClick.AddListener(ButtonClicked);
        }

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