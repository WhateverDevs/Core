using WhateverDevs.Core.Runtime.Common;

namespace WhateverDevs.Core.Runtime.Ui
{
    /// <summary>
    /// Close the game when the button is clicked.
    /// </summary>
    public class CloseGameOnButtonClick : ActionOnButtonClick<CloseGameOnButtonClick>
    {
        /// <summary>
        /// Close game when the button is clicked.
        /// </summary>
        protected override void ButtonClicked() => Utils.CloseGame();
    }
}