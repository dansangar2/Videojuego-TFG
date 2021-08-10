using System;
using UnityEngine;

namespace Core.ButtonsSystem.ButtonData
{
    /**<summary>It uses for set the data of the buttons.</summary>*/
    [Serializable]
    public class GenericButtonData
    {

        #region ATTRIBUTES

        [SerializeField] protected string buttonText;
        [SerializeField] protected string sceneRedirect;
        [SerializeField] protected Transform window;
        [SerializeField] protected bool quitGame;

        #endregion

        public GenericButtonData(GenericButtonData data)
        {
            sceneRedirect = data.sceneRedirect;
            buttonText = data.buttonText;
            window = data.window;
            quitGame = data.quitGame;
        }

        /**<summary>It's the text of the button.</summary>*/
        public string ButtonText => buttonText;

        /**<summary>It decides if when the player press it, it'll send the player
        to another window. If it's empty, then the player won't move.</summary>*/
        public string SceneRedirect => sceneRedirect;

        /**<summary>This windows will appear when the button is pressing.</summary>*/
        public Transform Window => window;

        /**<summary>Close the game.</summary>*/
        public bool QuitGame => quitGame;
    }
}
