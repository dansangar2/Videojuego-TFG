using System.Linq;
using Core.Battle.BattleSystem;
using Core.ButtonsSystem.ButtonType;
using Core.Controls;
using Core.Messages;
using Core.RestSystem;
using Core.RestSystem.Actions;
using Core.Saves;
using Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Core.ButtonsSystem.ButtonList
{
    public class CharacterButtonsList : ButtonsPages
    {
        /**<sumary>The button prefab.</sumary>*/
        public CharacterRestButton prefab;
        /**<sumary>The list of buttons. It marks what button is select.
        <para>It'll get all entity Button.</para></sumary>*/
        private CharacterRestButton[] _buttons;
        
        /**<sumary>The prefab of the message for the dialogues in the rest system.</sumary>*/
        public Message message;
        /**<sumary>Ability list for manage the abilities of the character.</sumary>*/
        public AbilityButtonsList abilityButtonsList;

        /**<sumary>Get the Character choose.</sumary>*/
        public static int Pos = -1;
        
        private void Awake()
        {
            if (SavesFiles.GetSave().Party.Length == 0) TestInit1();
            message.messages = new TextData[]{ };
            message.type = TextBoxType.Dialog;
            message.sceneRedirect = "";
            if (!SavesFiles.IsLoaded) message.messages = Dialogues.ExecuteDialogue();
            if (message.messages.Length != 0 && Dialogues.Added) Instantiate(message);
            BattleSystem.MaxMembers = Mathf.Min(3, SavesFiles.GetParty().Length - HospitalAction.PeopleInHospital);
            
            RestSystem.RestSystem.UpdateNotMove();
            
            prefab.blink = true;
            SetPages(SavesFiles.GetParty().Length);
            for (int i = 0; i < Mathf.Min(maxInPage, SavesFiles.GetParty().Length); i++)
            {
                Instantiate(prefab, transform);
            }

            _buttons = GetComponentsInChildren<CharacterRestButton>();

            for (int i = 0; i < _buttons.Length; i++) { _buttons[i].SetUp(SavesFiles.GetParty()[i]); }
            
            SetColumnsAndRows(_buttons);
        }
        
        private new void OnEnable()
        {
            Camera = Camera.main;
            BoxCollider2D[] t = controls.transform.GetComponentsInChildren<BoxCollider2D>();
            ButtonsCanClick = new ClickButton[4];
            for (int i = 0; i < 4; i++)
            {
                ButtonsCanClick[i] = new ClickButton(i,
                    t[i].GetComponent<Image>(), t[i],
                    t[i].name);
            }
            base.OnEnable();
        }

        public void Update()
        {
            #region Manage Buttons

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
                ClickButton.KeyUsed = "";
                Press = -1;
                Select = -1;
            }

            #endregion
            
            //================================================
            
            ChangePage(ControlsKeys.CameraLeft, ControlsKeys.CameraRight);
            //Check if go back, go to the ability list or select a character.
            if (Input.GetKeyDown(ControlsKeys.Back) || ClickButton.KeyUsed.Equals("Return") ||
                 (Input.GetKeyDown(ControlsKeys.Ok) || _buttons.Any(c => c.isCollider)) && 
                _buttons[position].MemberID == Pos) Pos = -1;
            else if ((Input.GetKeyDown(ControlsKeys.Ok) || _buttons.Any(c => c.isCollider))//Input.GetMouseButtonDown(0))
                     && Selected.canPress) Pos = Selected.MemberID;
            else if ((Input.GetKeyDown(ControlsKeys.ActionButton1) ||
                      ClickButton.KeyUsed.Equals("Ability")) && abilityButtonsList)
            {
                abilityButtonsList.character = SavesFiles.GetSave().GetCharacter(Selected.MemberID);
                if(abilityButtonsList.character.Abilities().Length == 0) return;
                abilityButtonsList.gameObject.SetActive(true);
            }

        }

        /**<summary>Update the buttons with the current section of the party.</summary>*/
        public void UpdateUI()
        {
            for (int i = CurrentPage * maxInPage;
                i < Mathf.Min(SavesFiles.GetParty().Length, (CurrentPage + 1) * maxInPage);
                i++)
            {
                _buttons[i - CurrentPage * maxInPage].SetUp(SavesFiles.GetParty()[i]);
            }
        }

        /**<sumary>Change the page.</sumary>*/
        private void ChangePage(KeyCode back, KeyCode next, bool condition = true)
        {
            if (condition && (Input.GetKeyDown(next) || 
                              ClickButton.KeyUsed.Equals("Back") || 
                              Input.GetKeyDown(back) ||
                              ClickButton.KeyUsed.Equals("Next")) &&
                _buttons.Length >= maxInPage)
            {
                base.ChangePage(Input.GetKeyDown(back) || ClickButton.KeyUsed.Equals("Back"), 
                    Input.GetKeyDown(next) || ClickButton.KeyUsed.Equals("Next"));
                for (int i = CurrentPage * maxInPage; i < (CurrentPage + 1) * maxInPage; i++)
                {
                    if (i >= SavesFiles.GetParty().Length)
                    {
                        _buttons[i - CurrentPage * maxInPage].IsSelect = false;
                        _buttons[i - CurrentPage * maxInPage].gameObject.SetActive(false);
                        continue;
                    }

                    _buttons[i - CurrentPage * maxInPage].gameObject.SetActive(true);
                    _buttons[i - CurrentPage * maxInPage].SetUp(SavesFiles.GetCharacterOfParty(i));
                }

                SetColumnsAndRows(_buttons);
                if (position >= AllActivated.Length)
                {
                    position = AllActivated.Length - 1;
                }
            }
            Move(_buttons);
        }

        #region SELECT

        /**<sumary>Set all buttons not selected.</sumary>*/
        public void SelectNone()
        {
            SelectNone(_buttons);
        }

        /**<sumary>Set "isSelect" the current position button.</sumary>*/
        public void SelectCurrent()
        {
            SelectCurrent(_buttons);
        }

        /**<sumary>Get the selected button.</sumary>*/
        public CharacterRestButton Selected => _buttons[position];

        /**<sumary>Marks the current button with red border if toMark is true, false if not.</sumary>*/
        public void ToMark(bool toMark) => Selected.ToMark(toMark, Color.red);

        /**<sumary>Get all active Buttons.</sumary>*/
        public CharacterRestButton[] AllActivated => _buttons.Where(b => b.gameObject.activeInHierarchy).ToArray();

        /**<sumary>Change all border to white.</sumary>*/
        public void DontMarkAny() { for (int i = 0; i < _buttons.Length; i++) { _buttons[i].ToMark(false, Color.red); } }
        
        #endregion
        
        #region TESTS

        [ContextMenu("Set Up")]
        public void TestSetUp()
        {
            TestDelete();
            TestInit1();
            SavesFiles.SaveData();
            TestLoad();
            DamageParty();
            TestParty();
        }
        
        [ContextMenu("Init Party1")]
        public void TestInit1()
        {
            SavesFiles.GetSave().AddCharacter(0, 1, 2, 3, 4);
            //SavesFiles.SaveData();
        }
        
        [ContextMenu("Init Party2")]
        public void TestInit2()
        {
            SavesFiles.GetSave().AddCharacter(1);
            //SavesFiles.SaveData();
        }
        
        [ContextMenu("Load data")]
        public void TestLoad()
        {
            
            SavesFiles.LoadData();
        }
        
        [ContextMenu("Delete")]
        public void TestDelete()
        {
            SavesFiles.Init();
        }
        
        [ContextMenu("Print current party")]
        public void TestParty()
        {
            foreach (Character cha in SavesFiles.GetSave().Party)
            {
                Debug.Log(cha);
            }
        }
        
        [ContextMenu("Damage Party")]
        public void DamageParty()
        {
            foreach (Character cha in SavesFiles.GetSave().Party)
            {
                cha.ReduceCurrentBlood(100);
            }
        }
        
        [ContextMenu("Add Abilities Points")]
        public void TestAbilityPoints()
        {
            foreach (Character c in SavesFiles.GetSave().Party)
            {
                c.AbilityPoints = 10000;
            }
        }

        #endregion
    }
}