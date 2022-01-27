using UnityEngine;
using WhateverDevs.Core.Behaviours;
using WhateverDevs.Core.Runtime.Common;

#if ODIN_INSPECTOR_3
using Sirenix.OdinInspector;
#endif

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
    public abstract class HidableUiElement<T> : WhateverBehaviour<T> where T : HidableUiElement<T>
    {
        /// <summary>
        /// Toggle interactable when showing and hiding?
        /// </summary>
        public bool ToggleInteractable;

        /// <summary>
        /// Toggle blocking raycasts when showing and hiding?
        /// </summary>
        public bool ToggleBlockRaycasts;

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
        #if ODIN_INSPECTOR_3
        [Button]
        #endif
        public virtual void Show(bool show = true)
        {
            CanvasGroup.alpha = show ? 1 : 0;

            if (ToggleInteractable) CanvasGroup.interactable = show;

            if (ToggleBlockRaycasts) CanvasGroup.blocksRaycasts = show;
        }
    }
}