using System.Linq;
using Core.Buttons;
using Core.Controls;
using Entities;
using UnityEngine;

namespace Core.HUDs
{
    public class AbilityList : Buttons.Buttons
    {
        /**<sumary>The list of buttons. It marks what button is select.
        <para>It'll get all entity Button.</para></sumary>*/
        private AbilitySlot[] _buttons;
        /**<sumary>The button prefab.</sumary>*/
        public AbilitySlot prefab;
        /**<sumary>The character that will use the ability.</sumary>*/
        public Character character;
        
        private void Start()
        {
            foreach (Ability ability in character.Abilities())
            {
                prefab.canSendMessage = true;
                prefab.messageToSend = ability.ID.ToString();
                prefab.elementIcon.sprite = character.Element.Icon;
                prefab.canSendMessage = character.CurrentKarmaPoints >= ability.Cost;
                prefab.SetUp(ability.ID, prefab.canSendMessage);
                Instantiate(prefab, transform.GetChild(2).transform);
            }

            _buttons = gameObject.transform.GetChild(2).GetComponentsInChildren<AbilitySlot>();
            
            if (int.TryParse(Button.Message, out _))
            {
                _buttons[Position].IsSelect = false;
                Position = _buttons.ToList().FindIndex(a => a.messageToSend==Button.Message);
                _buttons[Position].IsSelect = true;
            }
            else
            {
                Position = 0;
                _buttons[0].IsSelect = true;
            }
            SetColumnsAndRows(_buttons);
            Button.Message = "";
        }

        private void Update()
        {
            Move(_buttons);
            if(Input.GetKeyDown(ControlsKeys.Back)) Destroy(transform.gameObject);
        }
        
    }
}