using System.Collections.Generic;
using System.Linq;
using Core.Battle.BattleSystem;
using Core.ButtonsSystem.ButtonList;
using Core.ButtonsSystem.ButtonType;
using Core.Controls;
using Core.Messages;
using Core.RestSystem.Actions;
using Core.Saves;
using Entities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Core.RestSystem
{
    /**<summary>Main rest system.</summary>*/
    public class RestSystem : MonoBehaviour
    {
        #region ATTRIBUTES

        //Lists for take decision.
        public SaveButtonList saveList;
        private RestButtonsList _restOptionList;
        private CharacterButtonsList _characterList;
        private AbilityButtonsList _abilityButtonsList;

        /**<summary>Choose rest points to use system.</summary>*/
        public UseRestPointsInterface pointsToUse;
        /**<summary>Where the description of the rest action.</summary>*/
        public Text description;

        /**<summary>Save the first character ID selected in the "party" option.</summary>*/
        private int _characterToChangeID = -1;
        /**<summary>Check if some character is selected in a "Party" option.</summary>*/
        private bool _isMark;

        /**<summary>Characters that go to the hospital and cannot move.</summary>*/
        private static List<int> _cannotMove;
        
        #endregion
        
        public static void UpdateNotMove()
        {
            _cannotMove = new List<int>();
            foreach (Character character in SavesFiles.GetParty())
            {
                if (!character.IsKo() || _cannotMove.Contains(character.ID)) continue;
                HospitalAction.ToHospital(character);
                _cannotMove.Add(character.ID);
            }
        }
        
        public void Awake()
        {
            _characterList = GetComponentInChildren<CharacterButtonsList>();
            _restOptionList = GetComponentInChildren<RestButtonsList>();
            _abilityButtonsList = transform.GetChild(0).Find("AbilityPanel").GetComponentInChildren<AbilityButtonsList>();
            RestButtonsList.Option = "";
            GenericButton.Message = "";
            CharacterButtonsList.Pos = -1;

            foreach (int character in _cannotMove)
            {
                SavesFiles.GetSave().MoveCharacter(character, SavesFiles.GetParty().Length-1);
            }
        }

        public void Update()
        {
            
            if(Message.ThereAreMessage())
            {
                _restOptionList.enabled = false;
                _characterList.enabled = false;
                return;
            }
            if (Option.Equals("Save"))
            {
                saveList.gameObject.SetActive(true);
                _restOptionList.enabled = false;
            }
            
            if (_abilityButtonsList.gameObject.activeInHierarchy || saveList.gameObject.activeInHierarchy)
            {
                _restOptionList.enabled = false;
                _characterList.enabled = false;
                if(GenericButton.Message.Equals("") || !int.TryParse(GenericButton.Message, out _)) return;
                //GenericButton.Message = "";
                return;
                
            }
            if (pointsToUse.transform.parent.gameObject.activeInHierarchy) return;
            description.text = _restOptionList.Selected.description;

            if (Option.Equals("") || Option.Equals("Save"))
            {
                _restOptionList.enabled = true;
                _characterList.enabled = false;
                _characterList.SelectNone();
                _characterToChangeID = -1;
                CharacterID = -1;
                return;
            }

            _restOptionList.enabled = false;
            _characterList.enabled = true;
            //Go back if the character is selected to switch.
            if ((Input.GetKeyDown(ControlsKeys.Back) 
                 || ClickButton.KeyUsed.Equals("Return")) && CharacterID<0)
            {
                _characterList.Selected.IsSelect = false;
                _characterList.Selected.ToMark(false, Color.red);
                GenericButton.Message = "";
                Option = "";
            }
            else
            {
                _characterList.SelectCurrent();
            }

            //Prepare to switch characters
            if (Option.Equals("Party") && CharacterID>-1 && !_cannotMove.Contains(CharacterID))
            {
                if(_characterToChangeID!=CharacterID && _characterToChangeID>=0)
                {
                    
                    SavesFiles.GetSave().SwitchCharactersByID(_characterToChangeID, CharacterID);
                    _characterList.UpdateUI();
                    _characterToChangeID = -1;
                    CharacterID = -1;
                    _isMark = false;
                    _characterList.DontMarkAny();
                }
                else
                {
                    if(!_isMark)
                    {
                        _isMark = true;
                        _characterList.ToMark(_isMark);
                    }
                    _characterToChangeID = CharacterID;
                }
                return;
            }

            _characterToChangeID = -1;
            _isMark = false;

            if (!Option.Equals("") && CharacterID>-1) ToDoAction();

            if (SavesFiles.GetParty().Select(s => s.RestPoints).Sum() != 0) return;
            
            BattleSystem.MaxMembers = Mathf.Min(3, SavesFiles.GetParty().Length - HospitalAction.PeopleInHospital);
            HospitalAction.PeopleInHospital = 0;
            
            GenerateEnemies();
            SceneManager.LoadScene("BattleSystemTest");
        }

        /**<summary>It does the action.</summary>*/
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
                    _cannotMove.Add(CharacterID);
                    _characterList.UpdateUI();
                    break;
                case "Training":
                    pointsToUse.SetUp(Character.ID);
                    pointsToUse.transform.parent.gameObject.SetActive(true);
                    _characterList.enabled = false;
                    break;
                case "Nursing":
                    pointsToUse.SetUp(Character.ID);
                    pointsToUse.transform.parent.gameObject.SetActive(true);
                    _characterList.enabled = false;
                    break;
            }
            CharacterID = -1;
        }

        /**<summary>Prepare the enemies for the next battle and level up the current enemies.</summary>*/
        public void GenerateEnemies()
        {
            int[] enemiesCanGet = {};
            int difference = 1;
            while (enemiesCanGet.Length<3 && difference<=10)
            {
                enemiesCanGet = SavesFiles.GetSave().Enemies
                    .Where(e => !BattleSystem.EnemiesId.Contains(e.ID) && 
                                e.Level + 2>=SavesFiles.GetSave().Level-difference && e.Level +2<=SavesFiles.GetSave().Level 
                                + Mathf.Min(difference,3))
                    .Select(e => e.ID).ToArray();
                difference++;
            }
            
            for (int i = 0; i < BattleSystem.EnemiesId.Length; i++)
            {
                SavesFiles.GetSave().Enemies.First(c => c.ID==BattleSystem.EnemiesId[i]).Level = SavesFiles.GetSave().Level;
            }
            
            int enemy1 = Random.Range(0, enemiesCanGet.Length);
            int enemy2 = Random.Range(0, enemiesCanGet.Length);
            int enemy3 = Random.Range(0, enemiesCanGet.Length);
            BattleSystem.EnemiesId = new[]
            {
                //(SavesFiles.GetSave().Level - 1)*3,
                //(SavesFiles.GetSave().Level - 1)*3 + 1,
                //(SavesFiles.GetSave().Level - 1)*3 + 2
                enemiesCanGet[enemy1],
                enemiesCanGet[enemy2],
                enemiesCanGet[enemy3]
            };

        }

        /**<summary>Get the rest option.</summary>*/
        public string Option { get => RestButtonsList.Option; set => RestButtonsList.Option = value; }
        
        /**<summary>Get the character ID position.</summary>*/
        public int CharacterID { get => CharacterButtonsList.Pos; set => CharacterButtonsList.Pos = value;}
        
        /**<summary>Get the character selected.</summary>*/
        public Character Character => SavesFiles.GetSave().GetCharacter(_characterList.Selected.prefab.character.ID);

        /**<summary>Select the character to prepare to change position if it can move.</summary>*/
        public static bool ToChange(int id) => RestButtonsList.Option.Equals("Party") && !_cannotMove.Contains(id);

    }
}