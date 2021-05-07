﻿using Sirenix.OdinInspector;
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
        /// Is it being shown right now?
        /// </summary>
        [ReadOnly]
        public bool Shown = true;

        /// <summary>
        /// Should we set the heigh to 0 when hiding?
        /// </summary>
        public bool SetHeightToZeroWhenHiding;

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
        /// Height before hiding.
        /// </summary>
        [SerializeField]
        [HideInInspector]
        private float PreviousHeight;

        /// <summary>
        /// Reference to the rect transform.
        /// </summary>
        private RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
                return rectTransform;
            }
        }

        /// <summary>
        /// Backfield for RectTransform.
        /// </summary>
        private RectTransform rectTransform;

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

        /// <summary>
        /// Show or hide the button.
        /// </summary>
        /// <param name="show"></param>
        public void Show(bool show)
        {
            Button.interactable = show;
            CanvasGroup.alpha = show ? 1 : 0;

            if (SetHeightToZeroWhenHiding)
            {
                Vector2 rectTransformAnchoredSize = RectTransform.sizeDelta;

                if (Shown) PreviousHeight = rectTransformAnchoredSize.y;
                rectTransformAnchoredSize.y = show ? PreviousHeight : 0;

                RectTransform.sizeDelta = rectTransformAnchoredSize;
            }

            Shown = show;
        }
    }
}