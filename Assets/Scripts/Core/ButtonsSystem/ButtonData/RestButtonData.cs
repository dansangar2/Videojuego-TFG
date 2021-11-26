using System;
using UnityEngine;

namespace Core.ButtonsSystem.ButtonData
{
    
    /**<summary>It uses for set the data of the rest system buttons.</summary>*/
    [Serializable]
    public class RestButtonData
    {
        
        #region ATTRIBUTES

        
        /**<summary>Text of the button.</summary>*/
        [SerializeField] private string buttonText;
        /**<summary>The signal that the button will send when it'll press.</summary>*/
        [SerializeField] private string action;
        /**<summary>Icon to show in the button.</summary>*/
        [SerializeField] private Sprite icon;
        /**<summary>The description of the button.</summary>*/
        [TextArea][SerializeField] private string description;

        #endregion

        /**<summary>Clone constructor.</summary>*/
        public RestButtonData(RestButtonData data)
        {
            action = data.action;
            buttonText = data.buttonText;
            icon = data.icon;
        }

        /**<summary>The text that the button will have.</summary>*/
        public string ButtonText => buttonText;

        /**<summary>The action of the rest system that the button will have.
         <para>ToRest => Rest system.</para>
         ToHospital => Hospital system.
         <para>Training => Train system.</para>
         Nursing => Nurse system.
         <para>Party => Change party system.</para>
         Save => Save system.
         </summary>*/
        public string Action => action;

        /**<summary>The icon that will show in the rest section.</summary>*/
        public Sprite Icon => icon;
        
        /**<summary>The icon that will show in the rest section.</summary>*/
        public string Description => description;
    }
}