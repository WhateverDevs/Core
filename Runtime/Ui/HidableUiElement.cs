using UnityEngine;
using WhateverDevs.Core.Behaviours;

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
        /// Flag to know if it is being shown.
        /// </summary>
        #if ODIN_INSPECTOR_3
        [ReadOnly]
        #endif
        public bool Shown => GetCachedComponent<CanvasGroup>().alpha > 0;

        /// <summary>
        /// Show or hide the element.
        /// </summary>
        /// <param name="show"></param>
        #if ODIN_INSPECTOR_3
        [Button]
        #endif
        public virtual void Show(bool show = true)
        {
            GetCachedComponent<CanvasGroup>().alpha = show ? 1 : 0;

            if (ToggleInteractable) GetCachedComponent<CanvasGroup>().interactable = show;

            if (ToggleBlockRaycasts) GetCachedComponent<CanvasGroup>().blocksRaycasts = show;
        }
    }
}