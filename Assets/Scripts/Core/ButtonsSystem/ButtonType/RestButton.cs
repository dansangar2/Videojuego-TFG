using Core.ButtonsSystem.ButtonData;
using UnityEngine.UI;

namespace Core.ButtonsSystem.ButtonType
{
    /**<summary>The button for the rest options.</summary>*/
    public class RestButton : GenericButton
    {
        /**<summary>Set the image of the rest action.</summary>*/
        public Image icon;

        public string description;

        /**<summary>Set the data of this button with the data passed.</summary>*/
        public void SetUp(AbilityButtonData data)
        {
            blink = true;
            canSendMessage = true;
            sceneRedirect = "";
            itemName.text = data.ButtonText;
            messageToSend = data.Action;
            icon.sprite = data.Icon;
            description = data.Description;
        }
        
    }
}