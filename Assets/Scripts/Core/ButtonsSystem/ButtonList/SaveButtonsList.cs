using System.Linq;
using Core.ButtonsSystem.ButtonType;
using Core.Controls;
using Core.Saves;
using UnityEngine;
using UnityEngine.UI;

namespace Core.ButtonsSystem.ButtonList
{
    /**<summary>A list system for load or save data in saves slots.</summary>*/
    public class SaveButtonsList : BaseButtonsList
    {
        /**<summary>Prefab of the Save Button.</summary>*/
        public SaveButton prefab;
        /**<summary>Buttons for select save file.</summary>*/
        private SaveButton[] _buttons;
        /**<summary>Buttons that can click for go back.</summary>*/
        private ClickButton _buttonBack;
        
        private void Awake()
        {
            Save[] saves = SavesFiles.GetData();
            for (int i = 0; i < SavesFiles.Saves.Length; i++)
            {
                prefab.SetUp(saves[i]);
                Instantiate(prefab, transform.Find("Data").transform);
            }

            _buttons = GetComponentsInChildren<SaveButton>();
            _buttons[0].IsSelect = true;
            SetColumnsAndRows(_buttons);
            
            Camera = Camera.main;
            
            Image im = controls.GetComponentInChildren<Image>();
            BoxCollider2D bx = controls.GetComponentInChildren<BoxCollider2D>();
            _buttonBack = new ClickButton(0, im, bx,"act5");
            ClickButton.KeyUsed = "";
        }

        public void Update()
        {

            #region Back Button

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
                        _buttonBack.collider == hit.collider && _buttonBack.can ? _buttonBack.id : -1;
                    Press = Select;
                }
                else if (!hit && Press == -1)
                {
                    Press = -2;
                }
                else if (Press>-1) 
                    if(hit && hit.collider == _buttonBack.collider) 
                        Select = Press;
            }
            _buttonBack.image.color = _buttonBack.id==Select? Color.green : _buttonBack.id == Press ? Color.yellow : Color.white;
            if (Input.GetMouseButtonUp(0) && Select>-1)
            {
                ClickButton.KeyUsed = _buttonBack.key;
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
            
            
            //============================================================================
            
            //Check if go back, load or save.
            Move(_buttons);
            if (Input.GetKeyDown(ControlsKeys.Back) || ClickButton.KeyUsed.Equals("act5"))
            {
                RestButtonsList.Option = "";
                GenericButton.Message = "";
                gameObject.SetActive(false);
            }
            else if (Option.Equals("Save") && (Input.GetKeyDown(ControlsKeys.Ok)
                || _buttons.Any(s => s.isCollider)) && Selected.canPress)
            {
                SavesFiles.CurrentSave = position;
                SavesFiles.SaveData();
                Selected.SetUp(SavesFiles.GetSave());
            }
            else if (!Selected.isEmpty && Option.Equals("Load") &&
                     (Input.GetKeyDown(ControlsKeys.Ok)|| _buttons.Any(s => s.isCollider)) 
                     && Selected.canPress)
            {
                SavesFiles.CurrentSave = position;
                SavesFiles.LoadData();
            }
        }

        #region GETTER

        
        /**<summary>Get the signal of the button system.
        <para>Load => for Load</para>
        Save => for Save
        </summary>*/
        public string Option => GenericButton.Message;

        /**<summary>Get the current button.</summary>*/
        public SaveButton Selected => _buttons[position];
        
        #endregion
        
        #region TESTS
        [ContextMenu("Delete all")]
        public void TestDeleteData(){
            for (int i = 0; i < 3; i++)
            {
                SavesFiles.CurrentSave = i;
                SavesFiles.DeleteData();
            }

            SavesFiles.CurrentSave = 0;
        }

        #endregion
        
    }

}