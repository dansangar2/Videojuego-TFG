using Core.ButtonsSystem.ButtonData;
using UnityEngine;
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
        public void SetUp(RestButtonData data)
        {
            blink = true;
            canSendMessage = true;
            sceneRedirect = "";
            itemName.text = data.ButtonText;
            messageToSend = data.Action;
            icon.sprite = data.Icon;
            description = data.Description;
        }
        
        /**<summary>Set it that if it can press or not.</summary>*/
        public new void CanPress(bool can)
        {
            base.CanPress(can);
            canPress = can;
            icon.color = can ? Color.white : new Color(C, C, C, 1);
            itemName.color = can ? Color.white : new Color(C, C, C, 1);
        }
        
    }
}