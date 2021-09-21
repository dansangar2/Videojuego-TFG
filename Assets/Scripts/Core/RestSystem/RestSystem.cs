using System.Linq;
using Core.Battle.BattleSystem;
using Core.ButtonsSystem.ButtonList;
using Core.Controls;
using Core.RestSystem.Actions;
using Core.Saves;
using Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.RestSystem
{
    public class RestSystem : MonoBehaviour
    {
        public BattleSystem nextBattle;
        
        private RestButtonsList _restOptionList;
        private CharacterButtonsList _characterList;

        private bool _finish = true;

        public void Awake()
        {
            _characterList = GetComponentInChildren<CharacterButtonsList>();
            _restOptionList = GetComponentInChildren<RestButtonsList>();
            BattleSystem.MaxMembers = 3;
            RestButtonsList.Option = "";
            foreach (Character character in SavesFiles.GetParty())
            {
                if (character.IsKo()) HospitalAction.ToHospital(character);
            }
            BattleSystem.MaxMembers = Mathf.Min(3, SavesFiles.GetParty().Length - HospitalAction.PeopleInHospital);
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
            if (_finish) return;
            BattleSystem.MaxMembers = Mathf.Min(3, SavesFiles.GetParty().Length - HospitalAction.PeopleInHospital);
            HospitalAction.PeopleInHospital = 0;
            BattleSystem.EnemiesId = new[]
            {
                (SavesFiles.GetSave().Level - 1)*3,
                (SavesFiles.GetSave().Level - 1)*3 + 1,
                (SavesFiles.GetSave().Level - 1)*3 + 2
            };
            for (int i = 0; i < nextBattle.AllEnemies.Length; i++)
            {
                nextBattle.AllEnemies[i].character.Level = SavesFiles.GetSave().Level;
            }

            SceneManager.LoadScene("BattleSystemTest");

        }

        
        public void ToDoAction()
        {
            switch (Option)
            {
                case "ToRest":
                    RestAction.ToRest(Character);
                    break;
                //Check if the character is 100% of energy.
                case "ToHospital" when SavesFiles.GetParty().Length - HospitalAction.PeopleInHospital > 1 
                                       && (Character.MaxBloodPoints > Character.CurrentBloodPoints
                                           || Character.MaxKarmaPoints > Character.CurrentKarmaPoints
                                           || Character.Statuses.Length > 0):
                    HospitalAction.ToHospital(Character);
                    _characterList.UpdateUI(4);
                    break;
            }

            _finish = SavesFiles.GetParty().Select(s => s.RestPoints).Sum() != 0;
            CharacterID = -1;
        }

        /**<summary>Get the rest option.</summary>*/
        public string Option { get => RestButtonsList.Option; set => RestButtonsList.Option = value; }
        
        /**<summary>Get the character ID position.</summary>*/
        public int CharacterID { get => CharacterButtonsList.Pos; set => CharacterButtonsList.Pos = value;}
        
        /**<summary>Get the character selected.</summary>*/
        public Character Character => SavesFiles.GetSave().GetCharacter(_characterList.Selected.prefab.MemberID);

        
    }
}