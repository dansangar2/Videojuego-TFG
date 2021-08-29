using System.Linq;
using Core.ButtonsSystem.ButtonType;
using Core.Controls;
using Entities;
using UnityEngine;

namespace Core.ButtonsSystem.ButtonList
{
    /**<sumary>The ability list of one character system.</sumary>*/
    public class AbilityButtonsList : ButtonsPages
    {
        #region ATTRIBUTES

        /**<sumary>The list of buttons. It marks what button is select.
        <para>It'll get all entity Button.</para></sumary>*/
        private AbilityButton[] _buttons;
        /**<sumary>The button prefab.</sumary>*/
        public AbilityButton prefab;
        /**<sumary>The character that will use the ability.</sumary>*/
        public Character character;

        #endregion

        private void Awake()
        {
            foreach (Ability ability in character.Abilities())
            {
                prefab.canSendMessage = true;
                prefab.messageToSend = ability.ID.ToString();
                prefab.elementIcon.sprite = character.Element.Icon;
                prefab.SetUp(ability.ID, character.CurrentKarmaPoints >= ability.Cost);
                Instantiate(prefab, transform.GetChild(2).transform);
            }

            _buttons = gameObject.transform.GetChild(2).GetComponentsInChildren<AbilityButton>();
            
            if (int.TryParse(GenericButton.Message, out _))
            {
                Selected.IsSelect = false;
                position = _buttons.ToList().FindIndex(a => a.messageToSend==GenericButton.Message);
                Selected.IsSelect = true;
            }
            else
            {
                position = 0;
                _buttons[0].IsSelect = true;
            }
            SetColumnsAndRows(_buttons);
            GenericButton.Message = "";
            
        }

        private void Update()
        {
            Move(_buttons);
            if(Input.GetKeyDown(ControlsKeys.Back) || 
               !GenericButton.Message.Equals("")) Destroy(transform.gameObject);
        }

        #region SELECT

        /**<sumary>Set all "Select" to false (not selected).</sumary>*/
        public void SelectNone()
        {
            SelectNone(_buttons);
        }
        
        /**<sumary>Set "Select" the button of the current button.</sumary>*/
        public void SelectCurrent()
        {
            SelectCurrent(_buttons);
        }

        /**<sumary>Get the selected button.</sumary>*/
        public AbilityButton Selected => _buttons[position];

        #endregion

    }
}