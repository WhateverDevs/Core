using UnityEngine;

#if ODIN_INSPECTOR_3
using Sirenix.OdinInspector;
#endif

namespace WhateverDevs.Core.Runtime.Ui
{
    /// <summary>
    /// Hidable and subscribable button.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class HidableAndSubscribableButton : EasySubscribableButton
    {
        /// <summary>
        /// Is it being shown right now?
        /// </summary>
        #if ODIN_INSPECTOR_3
        [ReadOnly]
        #endif
        public bool Shown = true;

        /// <summary>
        /// Should we set the heigh to 0 when hiding?
        /// </summary>
        public bool SetHeightToZeroWhenHiding;

        /// <summary>
        /// Toggle interactable when showing and hiding?
        /// </summary>
        public bool ToggleInteractable;

        /// <summary>
        /// Toggle blocking raycasts when showing and hiding?
        /// </summary>
        public bool ToggleBlockRaycasts;

        /// <summary>
        /// Show the button.
        /// </summary>
        #if ODIN_INSPECTOR_3
        [Button]
        #endif
        public void Show() => Show(true);

        /// <summary>
        /// Hide the button.
        /// </summary>
        #if ODIN_INSPECTOR_3
        [Button]
        #endif
        public void Hide() => Show(false);

        /// <summary>
        /// Height before hiding.
        /// </summary>
        [SerializeField]
        [HideInInspector]
        private float PreviousHeight;

        /// <summary>
        /// Show or hide the button.
        /// </summary>
        /// <param name="show"></param>
        public virtual void Show(bool show)
        {
            GetCachedComponent<CanvasGroup>().alpha = show ? 1 : 0;

            if (ToggleInteractable)
            {
                GetCachedComponent<CanvasGroup>().interactable = show;
                Button.interactable = show;
            }

            if (ToggleBlockRaycasts) GetCachedComponent<CanvasGroup>().blocksRaycasts = show;

            if (SetHeightToZeroWhenHiding)
            {
                Vector2 rectTransformAnchoredSize = GetCachedComponent<RectTransform>().sizeDelta;

                if (Shown) PreviousHeight = rectTransformAnchoredSize.y;
                rectTransformAnchoredSize.y = show ? PreviousHeight : 0;

                GetCachedComponent<RectTransform>().sizeDelta = rectTransformAnchoredSize;
            }

            Shown = show;
        }
    }
}