using System.Linq;
using Core.Battle.DamageText;
using Core.HUDs;
using Core.Messages;
using Core.Saves;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Battle.BattleSystem
{
    public partial class BattleSystem : MonoBehaviour
    {

        public int[] enemiesId;
        public int maxMembers = 3;
        public Member memberBase;
        public AbilityList abilitiesOf;
        public Message message;
        public float velocity = 0.003f;
        
        private Fighter[] _fighters = {};
        
        private int[,] _experienceGained;
        
        private int _currentTurn;
        private BattleState _state;

        private ActionType _actionType;

        private string _scene;

        private void Start()
        {
            SavesFiles.CurrentSave = 0;
            TestSetUp();
            
            memberBase.gameObject.SetActive(false);
            memberBase.onlyHUD = true;
            _damageText = damageText.transform.GetComponentInChildren<AnimatedText>();
            //_damageText = damageText.transform.GetComponentInChildren<Text>();
            message.type = TextBoxType.Message;
            message.up = true;
            message.messages = GenEnemyMessage();

            Instantiate(message, transform);
            
            memberBase.gameObject.SetActive(false);

            _state = BattleState.Start;
            InitFighters();
            OrderBySpeed();
            GetExperienceFor();
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
                    else if(!Message.ThereAreMessage()) SceneManager.LoadScene("MainMenu");
                    break;
            }
        }
        
        #region GETTERS

        public Fighter[] GetGroup(bool enemy = false)
        {
            return enemy ? _fighters.Where(f => f.isEnemy).ToArray() : 
                _fighters.Where(f => !(f.isEnemy && f.character.IsKo())).ToArray();
        }
        private Fighter GetFighterTurn() { return _fighters[_currentTurn]; }
        private Fighter GetFighterTarget() { return _fighters[_posOfTarget]; }

        #endregion
        
    }
}