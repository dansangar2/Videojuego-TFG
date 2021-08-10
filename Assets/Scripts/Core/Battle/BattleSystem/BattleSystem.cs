using System.Linq;
using Core.Battle.DamageText;
using Core.ButtonsSystem.ButtonList;
using Core.ButtonsSystem.ButtonType;
using Core.Messages;
using Core.Saves;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Battle.BattleSystem
{
    public partial class BattleSystem : MonoBehaviour
    {

        #region ATTRIBUTES

        /**<summary>The array with the enemies.</summary>*/
        public int[] enemiesId;
        /**<summary>The max member party.</summary>*/
        public int maxMembers = 3;
        /**<summary>The prefab for character HUBs.</summary>*/
        public MemberHUDButton memberBase;
        /**<summary>The ability list for the player party.</summary>*/
        public AbilityButtonsList abilitiesOf;
        /**<summary>The message text.</summary>*/
        public Message message;
        /**<summary>The velocity of the character blink.</summary>*/
        public float velocity = 0.003f;
        
        /**<summary>The array with the fighter of the battle.</summary>*/
        private Fighter[] _fighters = {};
        
        /**<summary>The array with the experience gained [charThatGained, enemyFrom].</summary>*/
        private int[,] _experienceGained;
        
        /**<summary>Current Character turn.</summary>*/
        private int _currentTurn;
        /**<summary>The current state of the battle.</summary>*/
        private BattleState _state;

        /**<summary>The current action of the character selected.</summary>*/
        private ActionType _actionType;

        /**<summary>The scene where you returned.</summary>*/
        private string _scene = "RestSystemTest";

        #endregion

        private void Start()
        {
            Message.SetExitsMessage();
            SavesFiles.CurrentSave = 0;
            TestSetUp();
            
            message.type = TextBoxType.Message;
            message.up = true;
            message.messages = GenEnemyMessage();
            Instantiate(message, transform);
            
            memberBase.gameObject.SetActive(false);
            memberBase.onlyHUD = true;
            
            _animatedText = animatedText.transform.GetComponentInChildren<AnimatedText>();

            InitFighters();
            OrderBySpeed();
            GetExperienceFor();
            
            _state = BattleState.Start;
        }
        
        private void Update()
        {
            if (Message.ThereAreMessage()) return;
            switch (_state)
            {
                case BattleState.Start:
                    InitFightersHud();
                    _state = BattleState.Turn;
                    break;
                case BattleState.Turn:
                    ChooseTurns();
                    _state = BattleState.TurnAction;
                    break;
                case BattleState.TurnAction:
                    if (GetFighterTurn().isEnemy) EnemyTurn();
                    else CharacterTurn();
                    break;
                case BattleState.Lose:
                    if (_fighters.Any(f => !f.member.Equals(null)))
                    {
                        TextData[] lostMessage = {new TextData("You lost...")};
                        foreach (Fighter fighter in _fighters)
                        { 
                            Destroy(fighter.member.gameObject);
                        }
                        message.messages = lostMessage;
                        Instantiate(message, transform);
                    }
                    else if(!Message.ThereAreMessage()) SceneManager.LoadScene("BattleSystemTest");
                    break;
                case BattleState.Win:
                    if (_fighters.Any(f => !f.member.Equals(null)))
                    {
                        TextData[] gainMessage = {new TextData("You won!!!")};
                        foreach (Fighter fighter in _fighters)
                        {
                            Destroy(fighter.member.gameObject);
                        }
                        message.messages = gainMessage;
                        Instantiate(message, transform);
                    }
                    else if(!Message.ThereAreMessage()) SceneManager.LoadScene(_scene);
                    break;
            }
        }
        
        #region GETTERS

        /**<summary>Get the group of fighters.
        <param name="enemy">If it's true, then it return the enemies, else, it return the party.
        </param></summary>*/
        public Fighter[] GetGroup(bool enemy = false)
        {
            return enemy ? _fighters.Where(f => f.isEnemy).ToArray() : 
                _fighters.Where(f => !(f.isEnemy || f.character.IsKo())).ToArray();
        }
        /**<summary>Current Fighter turn.</summary>*/
        private Fighter GetFighterTurn() { return _fighters[_currentTurn]; }
        //private Fighter GetFighterTarget() { return _fighters[_posOfTarget]; }

        #endregion
        
    }
}