using System;
using System.Linq;
using Core.Battle.DamageText;
using Core.Battle.StatusesUI;
using Core.Messages;
using Core.Saves;
using Entities;
using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Core.Battle.BattleSystem
{
    public partial class BattleSystem : MonoBehaviour//Agent
    {

        #region ATTRIBUTES

        /**<summary>The array with the enemies.</summary>*/
        public static int[] EnemiesId = {0,1,2};
        /**<summary>The max member party.</summary>*/
        public static int MaxMembers = 3;
        /**<summary>The prefab message text.</summary>*/
        public Message message;
        //**<summary>The velocity of the character blink.</summary>*/
        //public float velocity = 0.003f;
        /**<summary>The statusUI prefab for fighters.</summary>*/
        public StatusUI statusUI;
        
        
        /**<summary>The array with the fighter of the battle.</summary>*/
        private Fighter[] _fighters = {};
        /**<summary>The current state of the battle.</summary>*/
        private BattleState _state;
        /**<summary>The scene where you returned.</summary>*/
        private string _scene = "RestSystemTest";

        /**<summary>Seconds to wait for next turn.</summary>*/
        private float _secondsToWait;

        #endregion

        #region SYSTEM

        private void Awake()
        {
            abilitiesOf.velocity = 0.5f;
            
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
            _secondsToWait += Time.deltaTime;
            if (Message.ThereAreMessage() || _secondsToWait<1) return;
            switch (_state)
            {
                case BattleState.Start:
                    InitFightersHud();
                    _state = BattleState.Turn;
                    break;
                case BattleState.Turn:
                    UpdateBattlefield();
                    ChooseTurns();
                    
                    #region CheckStatus

                    
                    foreach (StatusOf of in CurrentTurn.Statuses)
                    {
                        of.Duration--;
                        switch (of.Duration)
                        {
                            case 0 when of.Status.Effect == EffectType.Dead:
                                FighterTurn.SetKo();
                                UpdateBattlefield();
                                return;
                            case 0:
                                CurrentTurn.RemoveStatus(of.Status.ID);
                                break;
                        }
                    }
                    
                    int damageXTurn = -Convert.ToInt32(CurrentTurn.MaxBloodPoints * CurrentTurn.Regeneration);
                    
                    if(damageXTurn != 0)
                    {
                        CurrentTurn.ReduceCurrentBlood(-damageXTurn);
                        DamageAnimation(FighterTurn, -damageXTurn, AttackType.Blood);
                        if (CurrentTurn.IsKo())
                        {
                            UpdateBattlefield();
                            return;
                        }
                    }
                    damageXTurn = -Convert.ToInt32(CurrentTurn.MaxKarmaPoints * CurrentTurn.KarmaRegeneration);
                    if(damageXTurn != 0)
                    {
                        CurrentTurn.ReduceCurrentKarma(damageXTurn);
                        DamageAnimation(FighterTurn, damageXTurn, AttackType.Karma);
                    }
                    
                    if(CurrentTurn.Statuses.Any(s => s.Status.Effect == EffectType.DontMove)) return;

                    if (CurrentTurn.Statuses.Any(s => s.Status.Effect == EffectType.AttackRandom))
                    {
                        int i = Random.Range(0, 2);
                        RandomAttack(Convert.ToBoolean(i));
                        return;
                    }
                    if (CurrentTurn.Statuses.Any(s => s.Status.Effect == EffectType.AttackRandomEnemy))
                    {
                        RandomAttack(FighterTurn.isEnemy);
                        return;
                    } 
                    if (CurrentTurn.Statuses.Any(s => s.Status.Effect == EffectType.AttackRandomPartner))
                    {
                        RandomAttack(!FighterTurn.isEnemy);
                        return;
                    }
                    
                    #endregion

                    _state = BattleState.TurnAction;
                    break;
                case BattleState.TurnAction:
                    if (FighterTurn.isEnemy) EnemyTurn();
                    else CharacterTurn();
                    break;
                case BattleState.Lose:
                    if (AllParty.Any(f => !f.member.Equals(null)))
                    {
                        TextData[] lostMessage = {new TextData("You lost...")};
                        foreach (Fighter fighter in AllParty)
                        { 
                            Destroy(fighter.member.gameObject);
                        }
                        message.messages = lostMessage;
                        Instantiate(message, transform);
                    }
                    else if(!Message.ThereAreMessage()) SceneManager.LoadScene("MainMenu");

                    MaxMembers = 3;
                    break;
                case BattleState.Win:
                    if (AllParty.Any(f => !f.member.Equals(null)))
                    {
                        TextData[] gainMessage = {new TextData("You won!!!")};
                        foreach (Fighter fighter in AllParty)
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

        #endregion
        
        #region GETTERS

        /**<summary>Get the group of fighters.
        <param name="enemy">If it's true, then it return the enemies, else, it return the party.
        </param></summary>*/
        public Fighter[] GetGroup(bool enemy = false)
        {
            return enemy ? _fighters.Where(f => f.isEnemy && !f.character.IsKo()).ToArray() : 
                _fighters.Where(f => !(f.isEnemy || f.character.IsKo())).ToArray();
        }
        /**<summary>Current Fighter turn.</summary>*/
        private Fighter FighterTurn => _fighters[_currentTurn];
        //private Fighter GetFighterTarget() { return _fighters[_posOfTarget]; }

        /**<summary>Get all enemies members.</summary>*/
        public Fighter[] FighterFighting => _fighters.Where(f => !f.character.IsKo()).ToArray();
        
        /**<summary>Get the character of current turn.</summary>*/
        public Character CurrentTurn => _fighters[_currentTurn].character;
        
        /**<summary>Get all party members fighting (no dead).</summary>*/
        public Fighter[] PartyFighter => GetGroup();

        /**<summary>Get all enemies fighting (no dead).</summary>*/
        public Fighter[] EnemiesFighter => GetGroup(true);

        /**<summary>Get all party members fighting.</summary>*/
        public Character[] Party => _fighters.Where(f => !f.isEnemy).Select(f => f.character).ToArray();

        /**<summary>Get all enemies fighting.</summary>*/
        public Character[] Enemies => _fighters.Where(f => f.isEnemy).Select(f => f.character).ToArray();

        /**<summary>Get all party members.</summary>*/
        public Fighter[] AllParty => _fighters.Where(f => !f.isEnemy).ToArray();

        /**<summary>Get all enemies members.</summary>*/
        public Fighter[] AllEnemies => _fighters.Where(f => f.isEnemy).ToArray();
        
        #endregion

    }
}