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
            maxInPage = 10;
            prefab.gameObject.SetActive(true);
            for (int i = 0; i < maxInPage; i++)
            { 
                Instantiate(prefab, transform.GetChild(2).transform);
            }
            
            /*for (int i = maxInPage*CurrentPage; i < character.Abilities().Length; i++)
            {
                Instantiate(prefab, transform.GetChild(2).transform);
                //if(i >= maxInPage*(CurrentPage+1)) break;
                Ability ability = character.Abilities()[i];
                prefab.canSendMessage = true;
                prefab.messageToSend = ability.ID.ToString();
                prefab.elementIcon.sprite = character.Element.Icon;
                prefab.SetUp(ability.ID, character.CurrentKarmaPoints >= ability.Cost);
                Instantiate(prefab, transform.GetChild(2).transform);
            }*/
            
            _buttons = gameObject.transform.GetChild(2).GetComponentsInChildren<AbilityButton>();

            SetPages(character.Abilities().Length);//_buttons.Length);

            if (int.TryParse(GenericButton.Message, out _))
            {
                Selected.IsSelect = false;
                while (true)
                {
                    SetCurrentAbilities();
                    position = _buttons.ToList().FindIndex(a => a.messageToSend==GenericButton.Message);
                    Debug.Log(position);
                    if (position != -1 || CurrentPage == NumberOfPages - 1) break;
                    CurrentPage++;
                }
                SelectCurrent();
                Selected.IsSelect = true;
            }
            else
            {
                SetCurrentAbilities();
                position = 0;
                _buttons[0].IsSelect = true;
            }

            SetColumnsAndRows(_buttons);

            GenericButton.Message = "";
            
        }

        private void Update()
        {
            //ChangePage(ControlsKeys.CameraLeft, ControlsKeys.CameraRight);
            //ChangePage();
            if ((position == maxInPage-1 || position == character.Abilities().Length - maxInPage * CurrentPage - 1) && Input.GetKeyDown(ControlsKeys.MoveDown))
            {
                CurrentPage++;
                CurrentPage %= NumberOfPages;
                position = 0;
                SetCurrentAbilities();
                SelectCurrent();
            }
            else if(position <= 0 && Input.GetKeyDown(ControlsKeys.MoveUp))
            {
                CurrentPage--;
                CurrentPage = CurrentPage < 0 ? NumberOfPages-1 : CurrentPage;
                position = CurrentPage == NumberOfPages-1 ? character.Abilities().Length % maxInPage-1 : maxInPage - 1;
                SetCurrentAbilities();
                SelectCurrent();
            }
            else
            {
                Move(_buttons);
            }
            if(Input.GetKeyDown(ControlsKeys.Back) || 
               !GenericButton.Message.Equals("")) Destroy(transform.gameObject);
        }

        public void SetCurrentAbilities()
        {
            for (int i = 0; i < maxInPage; i++)
            {
                if (i+maxInPage*CurrentPage > character.Abilities().Length-1)
                {
                    _buttons[i].gameObject.SetActive(false);
                    continue;
                }
                _buttons[i].gameObject.SetActive(true);
                Ability ability = character.Abilities()[i+maxInPage*CurrentPage];
                _buttons[i].canSendMessage = true;
                _buttons[i].messageToSend = ability.ID.ToString();
                _buttons[i].elementIcon.sprite = character.Element.Icon;
                _buttons[i].SetUp(ability.ID, character.CurrentKarmaPoints >= ability.Cost);
                //Instantiate(prefab, transform.GetChild(2).transform);
            }
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
            SelectNone();
            SelectCurrent(_buttons);
        }

        /**<sumary>Get the selected button.</sumary>*/
        public AbilityButton Selected => _buttons[position];

        #endregion

    }
}