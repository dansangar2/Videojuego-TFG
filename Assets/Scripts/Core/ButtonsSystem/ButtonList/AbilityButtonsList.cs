using System;
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

        /**<sumary>The button prefab.</sumary>*/
        public AbilityButton prefab;
        /**<sumary>The list of buttons. It marks what button is select.
        <para>It'll get all entity Button.</para></sumary>*/
        private AbilityButton[] _buttons;
        /**<sumary>The character that will use the ability.</sumary>*/
        public Character character;
        /**<sumary>The panel where the slots will generated.</sumary>*/
        public Transform panel;
        /**<sumary>It unblocked the abilities that haven't enough KP.</sumary>*/
        public bool unblockWithoutNecessaryKp;
        /**<sumary>Check if the list will be for improve the abilities or use it.</sumary>*/
        public bool improveSystem;
        /**<sumary>The cost of the ability.</sumary>*/
        public Text cost;
        /**<sumary>Get the stats of the next and current level.</sumary>*/
        public Text[] levelData;
        /**<sumary>The ability points of the character.</sumary>*/
        public Text characterPoints;
        /**<sumary>Check if it uses back button.</sumary>*/
        public bool haveBackButton;
        /**<sumary>Legend for the stats of current and next level.</sumary>*/
        private string[] _legend = {"Before", "After"};

        #endregion

        private new void OnEnable()
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
            
            Camera = Camera.main;
            
            ButtonsCanClick = new ClickButton[1 + Convert.ToInt32(haveBackButton)];
            
            Image[] im = controls.GetComponentsInChildren<Image>();
            BoxCollider2D[] bx = controls.GetComponentsInChildren<BoxCollider2D>();
            ButtonsCanClick[0] = new ClickButton(0, im[0], bx[0], "act4");
            if(haveBackButton) ButtonsCanClick[1] = new ClickButton(1, im[1], bx[1], "act3");
            
            ClickButton.KeyUsed = "";

            base.OnEnable();
        }

        public void Update()
        {

            #region Click Button

            //Check if the back button is pressed.
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

                //Check if it's selected after it had been pressed and released upper the same button.
                if (!hit && Select > -1)
                {
                    Select = -1;
                }
                else if (hit && Press == -1)
                {
                    Select =
                        ButtonsCanClick.FirstOrDefault(b => b.collider == hit.collider && b.can)?.id ?? -1;
                    Press = Select;
                }
                else if (!hit && Press == -1)
                {
                    Press = -2;
                }
                else if (Press>-1) 
                    if(hit && hit.collider == ButtonsCanClick[Press].collider) 
                        Select = Press;
            }
            ButtonsCanClick = ButtonsCanClick.Select(s => {s.image.color 
                = s.id==Select? Color.green : s.id == Press ? Color.yellow 
                : Color.white; return s; }).ToArray();
            if (Input.GetMouseButtonUp(0) && Select>-1)
            {
                ClickButton.KeyUsed = ButtonsCanClick[Select].key;
                Press = -1;
                Select = -1;
            }
            else if(Input.GetMouseButtonUp(0) || ClickButton.KeyUsed!="")
            {
                if(haveBackButton&&!ClickButton.KeyUsed.Equals("act3")) ClickButton.KeyUsed = "";
                Press = -1;
                Select = -1;
            }

            #endregion
            
            //============================================================================


            if(cost) cost.text = CurrentAbility.Ability.NeedPointsToLevelUp.ToString();
            if (characterPoints) characterPoints.text = character.Name + ": " + 
                                                        character.AbilityPoints + " points.";

            for (int i = 0; i < levelData.Length; i++)
            {
                SpecialAbility sp = new SpecialAbility(Selected.AbilityID, 
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
            
            if (Input.GetKeyDown(ControlsKeys.ActionButton1) || ClickButton.KeyUsed.Equals("act4"))
            {
                CurrentPage++;
                CurrentPage %= NumberOfPages;
                SetCurrentAbilities();
                SelectCurrent();
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
                transform.gameObject.SetActive(improveSystem);
                if(improveSystem)
                {
                    character.UpdateAbility(Selected.AbilityID);
                    Selected.CanPress(CurrentAbility.Level != CurrentAbility.MaxLevel);
                }
                GenericButton.Message = "";
            }
            else if(Input.GetKeyDown(ControlsKeys.Back) || ClickButton.KeyUsed.Equals("act3")) transform.gameObject.SetActive(false);

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
                _buttons[i].SetUp(ability.ID, character,
                    character.CurrentKarmaPoints >= ability.Cost || unblockWithoutNecessaryKp);
                if (!improveSystem) continue;
                _buttons[i].CanPress(character.GetSpAbility(_buttons[i].AbilityID).Level !=
                    character.GetSpAbility(_buttons[i].AbilityID).MaxLevel);
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
        public SpecialAbility CurrentAbility => character.GetSpAbility(Selected.AbilityID);
        
        #endregion

    }
}