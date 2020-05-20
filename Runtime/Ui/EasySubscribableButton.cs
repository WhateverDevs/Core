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
    }
}