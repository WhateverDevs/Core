using System;

namespace WhateverDevs.Core.Runtime.Ui
{
    /// <summary>
    /// A class that allows for easy subscribing to buttons through code.
    /// </summary>
    public class EasySubscribableButton : ActionOnButtonClick<EasySubscribableButton>
    {
        /// <summary>
        /// Called when the button is clicked.
        /// </summary>
        public Action OnButtonClicked;

        /// <summary>
        /// Call the event.
        /// </summary>
        protected override void ButtonClicked() => OnButtonClicked?.Invoke();

        /// <summary>
        /// Easy subscribing to the button.
        /// </summary>
        /// <param name="button">This button.</param>
        /// <param name="subscriber">The function to subscribe.</param>
        /// <returns>The resulting action.</returns>
        public static EasySubscribableButton operator +(EasySubscribableButton button, Action subscriber)
        {
            button.OnButtonClicked += subscriber;
            return button;
        }

        /// <summary>
        /// Easy unsubscribing from the button.
        /// </summary>
        /// <param name="button">This button.</param>
        /// <param name="subscriber">The function to unsubscribe.</param>
        /// <returns>The resulting action.</returns>
        public static EasySubscribableButton operator -(EasySubscribableButton button, Action subscriber)
        {
            button.OnButtonClicked -= subscriber;
            return button;
        }
    }
}