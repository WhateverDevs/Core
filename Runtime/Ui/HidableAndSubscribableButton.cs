using Sirenix.OdinInspector;
using UnityEngine;
using WhateverDevs.Core.Runtime.Ui;

namespace Varguiniano.LosJoegos.Runtime.Ui.Common
{
    /// <summary>
    /// Hidable and subscribable button.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class HidableAndSubscribableButton : EasySubscribableButton
    {
        /// <summary>
        /// Show the button.
        /// </summary>
        [Button]
        public void Show() => Show(true);

        /// <summary>
        /// Hide the button.
        /// </summary>
        [Button]
        public void Hide() => Show(false);
        
        /// <summary>
        /// Show or hide the button.
        /// </summary>
        /// <param name="show"></param>
        public void Show(bool show)
        {
            Button.interactable = show;
            CanvasGroup.alpha = show ? 1 : 0;
        }

        /// <summary>
        /// Reference to the canvas group.
        /// </summary>
        protected CanvasGroup CanvasGroup
        {
            get
            {
                if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
                return canvasGroup;
            }
        }

        /// <summary>
        /// Backfield for CanvasGroup.
        /// </summary>
        private CanvasGroup canvasGroup;
    }
}