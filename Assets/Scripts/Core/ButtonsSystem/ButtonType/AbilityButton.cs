using System;
using Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Core.ButtonsSystem.ButtonType
{
    /**<summary>The button of the Ability.</summary>*/
    public class AbilityButton : BaseHUDButton
    {
        
        /**<summary>The ID of the ability.</summary>*/
        public int id;
        
        /**<summary>Set the necessary KP for the ability.</summary>*/
        public Text necessaryKp;

        /**<summary>Set the character.</summary>*/
        private Character _character;

        /**<summary>Set up the data of the button by abilityID, and it can set if can select or not.</summary>*/
        public void SetUp(int nId, Character character, bool can=true) 
        { 
            id = nId;
            _character = character;
            UpdateUI();
            CanPress(can);
        }
        
        /**<summary>Update the data of the character ID.</summary>*/
        private void UpdateUI()
        {
            Ability ability = _character.GetAbility(id);
            itemName.text = ability.Name;
            necessaryKp.text = ability.Cost.ToString();
            
            Tuple<Sprite, Sprite> sprites = ability.AbilityIcons();

            if (sprites.Item2 != null) elementIcon.sprite = sprites.Item2;
            itemIcon.sprite = sprites.Item1;
        }

        /**<summary>Set the interface if can or not press and indicates if it can press or not.</summary>*/
        public new void CanPress(bool can)
        {
            base.CanPress(can);
            necessaryKp.color = can ? Color.white : new Color(C, C, C, 1);
        }

        /**<summary>Get the ID of the ability.</summary>*/
        public int AbilityID => id;

    }
}