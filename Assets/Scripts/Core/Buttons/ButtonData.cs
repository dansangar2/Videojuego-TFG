using System;
using UnityEngine;

namespace Core.Buttons
{
    /**<summary>Base of the buttons, the base movement system is here</summary>*/
    [Serializable]
    public class ButtonData //: MonoBehaviour
    {

        #region ATTRIBUTES

        /**<summary>It's the name of the button.</summary>*/
        public string buttonName;
        /**<summary>It decides if when the player press it, it'll send you
        to another window. If it's empty, then the player isn't moving.</summary>*/
        public string sceneRedirect;
        /**<summary>This windows will appear when the button is pressing.</summary>*/
        public Transform window;
        /**<summary>Close the game.</summary>*/
        public bool quitGame;

        #endregion

        public ButtonData(ButtonData data)
        {
            sceneRedirect = data.sceneRedirect;
            buttonName = data.buttonName;
            window = data.window;
            quitGame = data.quitGame;
        }
    }
}
