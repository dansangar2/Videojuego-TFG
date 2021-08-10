using Core.ButtonsSystem.ButtonList;
using Core.Controls;
using Core.RestSystem.Actions;
using Core.Saves;
using Entities;
using UnityEngine;

namespace Core.RestSystem
{
    public class RestSystem : MonoBehaviour
    {
        private RestButtonsList _restOptionList;
        private CharacterButtonsList _characterList;

        public void Start()
        {
            _characterList = GetComponentInChildren<CharacterButtonsList>();
            _restOptionList = GetComponentInChildren<RestButtonsList>();
        }

        public void Update()
        {
            
            if (Option.Equals(""))
            {
                _restOptionList.enabled = true;
                _characterList.enabled = false;
                _characterList.SelectNone();
                return;
            }

            _restOptionList.enabled = false;
            _characterList.enabled = true;
            if (Input.GetKeyDown(ControlsKeys.Back))
            {
                _characterList.Selected.IsSelect = false;
                Option = "";
            }
            else
            {
                _characterList.SelectCurrent();
            }
            if (!Option.Equals("") && CharacterID>-1) ToDoAction();
        }

        
        public void ToDoAction()
        {
            switch (Option)
            {
                case "ToRest":
                    RestAction.ToRest(Character);
                    break;
            }
            CharacterID = -1;
        }

        /**<summary>Get the rest option.</summary>*/
        public string Option { get => RestButtonsList.Option; set => RestButtonsList.Option = value; }
        
        /**<summary>Get the character ID position.</summary>*/
        public int CharacterID { get => CharacterButtonsList.Pos; set => CharacterButtonsList.Pos = value;}
        
        /**<summary>Get the character selected.</summary>*/
        public Character Character => SavesFiles.GetSave().GetCharacter(_characterList.Selected.MemberID);

        
    }
}