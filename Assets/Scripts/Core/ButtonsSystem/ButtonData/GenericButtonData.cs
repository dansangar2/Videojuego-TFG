using System;
using UnityEngine;

namespace Core.ButtonsSystem.ButtonData
{
    /**<summary>It uses for set the data of the buttons.</summary>*/
    [Serializable]
    public class GenericButtonData
    {

        #region ATTRIBUTES

        /**<summary>Text of the button.</summary>*/
        [SerializeField] private string buttonText;
        /**<summary>Scene To redirect.</summary>*/
        [SerializeField] protected string sceneRedirect;
        /**<summary>Signal that the button will send when it's pressed.</summary>*/
        [SerializeField] protected string message;
        /**<summary>Transform To activate.</summary>*/
        [SerializeField] protected Transform window;
        /**<summary>Close the game.</summary>*/
        [SerializeField] protected bool quitGame;

        #endregion

        /**<summary>Clone constructor.</summary>*/
        public GenericButtonData(GenericButtonData data)
        {
            sceneRedirect = data.sceneRedirect;
            buttonText = data.buttonText;
            message = data.message;
            window = data.window;
            quitGame = data.quitGame;
        }

        /**<summary>It's the text of the button.</summary>*/
        public string ButtonText => buttonText;

        /**<summary>It decides if when the player press it, it'll send the player
        to another window. If it's empty, then the player won't move.</summary>*/
        public string SceneRedirect => sceneRedirect;

        /**<summary>The message that the button send.</summary>*/
        public string Message => message;
        
        /**<summary>This windows will appear when the button is pressing.</summary>*/
        public Transform Window => window;

        /**<summary>Close the game.</summary>*/
        public bool QuitGame => quitGame;
    }
}
