using Sirenix.OdinInspector;
using UnityEngine;
using WhateverDevs.Core.Runtime.Common;

namespace WhateverDevs.Core.Runtime.Ui
{
    /// <summary>
    /// Ui element with easy methods to hide and show.
    /// Concrete implementation in case you don't want to inherit.
    /// </summary>
    public class HidableUiElement : HidableUiElement<HidableUiElement>
    {
    }

    /// <summary>
    /// Ui element with easy methods to hide and show.
    /// </summary>
    /// <typeparam name="T">The inheriting class, used for logging.</typeparam>
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class HidableUiElement<T> : LoggableMonoBehaviour<T> where T : HidableUiElement<T>
    {
        /// <summary>
        /// Reference to the canvas group to be able to show and hide the panel.
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

        /// <summary>
        /// Show or hide the element.
        /// </summary>
        /// <param name="show"></param>
        [Button]
        public virtual void Show(bool show = true) => CanvasGroup.alpha = show ? 1 : 0;
    }
}