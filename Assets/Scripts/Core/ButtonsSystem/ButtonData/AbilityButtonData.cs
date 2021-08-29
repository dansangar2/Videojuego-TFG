using System;
using UnityEngine;

namespace Core.ButtonsSystem.ButtonData
{
    
    /**<summary>It uses for set the data of the rest system buttons.</summary>*/
    [Serializable]
    public class AbilityButtonData
    {
        
        #region ATTRIBUTES

        [SerializeField] private string buttonText;
        [SerializeField] private string action;
        [SerializeField] private Sprite icon;

        #endregion

        public AbilityButtonData(AbilityButtonData data)
        {
            action = data.action;
            buttonText = data.buttonText;
            icon = data.icon;
        }

        /**<summary>The text that the button will have.</summary>*/
        public string ButtonText => buttonText;

        /**<summary>The action of the rest system that the button will have.
         <para>ToRest => Rest system.</para></summary>*/
        public string Action => action;

        /**<summary>The icon that will show in the the rest section.</summary>*/
        public Sprite Icon => icon;
    }
}