using System.Linq;
using Core.ButtonsSystem.ButtonType;
using Core.Controls;
using Core.Saves;
using Entities;
using UnityEngine;

namespace Core.ButtonsSystem.ButtonList
{
    public class CharacterButtonsList : ButtonsPages
    {
        /**<sumary>The list of buttons. It marks what button is select.
        <para>It'll get all entity Button.</para></sumary>*/ 
        private CharacterRestButton[] _buttons;
        /**<sumary>The button prefab.</sumary>*/
        public MemberHUDButton prefab;

        /**<sumary>The Character gets.</sumary>*/
        public static int Pos = -1;
        
        public void Start()
        {
            TestSetUp();
            SetPages(SavesFiles.GetParty().Length);
            prefab.onlyHUD = false;
            for (int i = 0; i < Mathf.Min(maxInPage, SavesFiles.GetParty().Length); i++)
            {
                prefab.SetUp(SavesFiles.GetCharacterOfParty(i).ID);
                prefab.blink = true;
                SavesFiles.GetSave().GetCharacter(i).RestPoints = 5;
                Instantiate(prefab, transform);
            }

            _buttons = GetComponentsInChildren<CharacterRestButton>();
            SetColumnsAndRows(_buttons);
        }

        public void Update()
        {
            ChangePage(ControlsKeys.CameraLeft, ControlsKeys.CameraRight);
            if(Input.GetKeyDown(ControlsKeys.Ok) && Selected.canPress) Pos = Selected.MemberID;
        }


        /**<sumary>Change the page.</sumary>*/
        private void ChangePage(KeyCode back, KeyCode next, bool condition = true)
        {
            if ((condition && (Input.GetKeyDown(next) || Input.GetKeyDown(back))) && 
                (_buttons.Length >= maxInPage))
            {
                base.ChangePage(back, next);
                /*int rest = (currentPage + 1) % numberOfPages == 0
                    ? SavesFiles.GetParty().Length % maxInPage
                    : maxInPage;*/
                for (int i = CurrentPage * maxInPage; i < (CurrentPage + 1) * maxInPage; i++)
                {
                    if (i >= SavesFiles.GetParty().Length)
                    {
                        _buttons[i - CurrentPage * maxInPage].IsSelect = false;
                        _buttons[i-CurrentPage*maxInPage].gameObject.SetActive(false);
                        continue;
                    }
                    _buttons[i-CurrentPage*maxInPage].gameObject.SetActive(true);
                    _buttons[i - CurrentPage * maxInPage].SetUp(SavesFiles.GetCharacterOfParty(i).ID);
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

        /**<sumary>Set all buttons SelectNone.</sumary>*/
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
        
        /**<sumary>Get all active Buttons.</sumary>*/
        public CharacterRestButton[] AllActivated => _buttons.Where(b => b.gameObject.activeInHierarchy).ToArray();
        

        #endregion
        
        #region TESTS

        [ContextMenu("Set Up")]
        public void TestSetUp()
        {
            TestDelete();
            TestInit1();
            TestLoad();
            TestParty();
        }
        
        [ContextMenu("Init Party1")]
        public void TestInit1()
        {
            SavesFiles.GetSave().AddCharacter(4, 1, 2, 3, 0);
            SavesFiles.SaveData();
        }
        
        [ContextMenu("Init Party2")]
        public void TestInit2()
        {
            SavesFiles.GetSave().AddCharacter(1);
            SavesFiles.SaveData();
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

        #endregion
    }
}