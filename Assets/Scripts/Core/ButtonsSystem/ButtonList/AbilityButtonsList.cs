using System.Linq;
using Core.ButtonsSystem.ButtonType;
using Core.Controls;
using Entities;
using UnityEngine;
using UnityEngine.UI;

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
        /**<sumary>The panel where the slots will generated.</sumary>*/
        public Transform panel;

        public bool keepTheList;
        
        public Text cost;

        public Text[] levelData;
        
        public Text characterPoints;

        private string[] _legend = {"Before", "After"};

        #endregion

        private void OnEnable()
        {
            numsOfCol = numsOfCol > maxInPage ? maxInPage : numsOfCol;
            prefab.gameObject.SetActive(true);
            for (int i = 0; i < maxInPage; i++)
            {
                if (panel.childCount != maxInPage) Instantiate(prefab, panel.transform);
                else panel.GetChild(i).gameObject.SetActive(true);
            }
            
            _buttons = panel.GetComponentsInChildren<AbilityButton>();
            
            SelectNone();
            
            SetPages(character.Abilities().Length);

            if (int.TryParse(GenericButton.Message, out _))
            {
                Selected.IsSelect = false;
                while (true)
                {
                    SetCurrentAbilities();
                    position = _buttons.ToList().FindIndex(a => a.messageToSend==GenericButton.Message);
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

        public void Update()
        {
            if(cost) cost.text = CurrentAbility.Ability.NeedPointsToLevelUp.ToString();
            if (characterPoints) characterPoints.text = character.Name + ": " + 
                                                        character.AbilityPoints + " points.";
            for (int i = 0; i < levelData.Length; i++)
            {
                SpecialAbility sp = new SpecialAbility(Selected.MemberID, 
                    CurrentAbility.Level + i, 
                    CurrentAbility.NeedLevel, 
                    CurrentAbility.MaxLevel);
                if (sp.Level <= sp.MaxLevel)
                    levelData[i].text = _legend[i] + "\n" + sp.Level + "\n" +
                                        sp.Ability.DownInterval + "\n" +
                                        sp.Ability.UpperInterval + "\n" +
                                        sp.Ability.PowerIncrement;
                else levelData[i].text = _legend[i] + "\n-\n-\n-\n-";
            }

            if ((position >= numsOfCol* (CurrentNumOfRows-1) || position == character.Abilities().Length - maxInPage * CurrentPage - 1) && Input.GetKeyDown(ControlsKeys.MoveDown))
            {
                Move(_buttons);
                CurrentPage++;
                CurrentPage %= NumberOfPages;
                SetCurrentAbilities();
                SelectCurrent();
            }
            else if(position < numsOfCol && Input.GetKeyDown(ControlsKeys.MoveUp))
            {
                
                CurrentPage--;
                CurrentPage = CurrentPage < 0 ? NumberOfPages-1 : CurrentPage;
                SetCurrentAbilities();
                SelectCurrent();
                Move(_buttons);
            }
            else Move(_buttons);
            
            if (!GenericButton.Message.Equals("")
                && gameObject.activeInHierarchy)
            {
                transform.gameObject.SetActive(keepTheList);
                if(keepTheList) character.UpdateAbility(Selected.MemberID);
                GenericButton.Message = "";
            }
            else if(Input.GetKeyDown(ControlsKeys.Back)) transform.gameObject.SetActive(false);
            
        }

        /**<summary>Get the abilities of the current page.</summary>*/
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
                _buttons[i].SetUp(ability.ID, character, character.CurrentKarmaPoints >= ability.Cost);
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
        
        /**<summary>Get the current ability.</summary>*/
        public SpecialAbility CurrentAbility => character.GetSpAbility(Selected.MemberID);
        
        #endregion

    }
}