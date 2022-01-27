using TMPro;
using UnityEngine;
using WhateverDevs.Core.Behaviours;
using WhateverDevs.Core.Runtime.Common;

#if ODIN_INSPECTOR_3
using Sirenix.OdinInspector;
#endif

namespace WhateverDevs.Core.Runtime.Ui
{
    /// <summary>
    /// Class that enables easy updating of a TMP text.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EasyUpdateText<T> : WhateverBehaviour<T> where T : EasyUpdateText<T>
    {
        /// <summary>
        /// Is the text in the same game object?
        /// </summary>
        [Tooltip("Is the text in the same game object?")]
        public bool TextInTheSameObject = true;

        /// <summary>
        /// Accessor to the Text.
        /// </summary>
        public TMP_Text Text
        {
            get
            {
                if (TextInTheSameObject) TextReference = GetComponent<TMP_Text>();

                if (TextReference == null) GetLogger().Error("Text reference has not been assigned.");

                return TextReference;
            }
        }

        /// <summary>
        /// Reference to the Text.
        /// </summary>
        #if ODIN_INSPECTOR_3
        [HideIf("TextInTheSameObject")]
        #endif
        public TMP_Text TextReference;

        /// <summary>
        /// Updates the text with the given string.
        /// </summary>
        /// <param name="text"></param>
        public void UpdateText(string text) => Text.SetText(text);
    }
}