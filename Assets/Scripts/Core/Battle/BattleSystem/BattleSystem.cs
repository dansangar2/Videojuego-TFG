using System;
using System.Linq;
using Core.Battle.DamageText;
using Core.Battle.StatusesUI;
using Core.Battle.TurnsUI;
using Core.Controls;
using Core.Messages;
using Core.Saves;
using Entities;
using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Core.Battle.BattleSystem
{
    /**<summary>The battle system.</summary>*/
    public partial class BattleSystem : MonoBehaviour
    {

        #region ATTRIBUTES

        /**<summary>The array with the enemies ID.</summary>*/
        public static int[] EnemiesId = {0,1,2};
        /**<summary>The max member party.</summary>*/
        public static int MaxMembers = 3;
        
        /**<summary>The prefab message text.</summary>*/
        public Message message;
        /**<summary>The statusUI prefab for fighters.</summary>*/
        public StatusUI statusUI;
        /**<summary>The turn interface that check the next turns.</summary>*/
        public TurnUI turnUI;
        /**<summary>The panel where the main characters status will show.</summary>*/
        public Transform playerPanel;
        /**<summary>The panel where the enemies status will show.</summary>*/
        public Transform enemyPanel;
        
        /**<summary>The array with the fighter of the battle.</summary>*/
        private Fighter[] _fighters = {};
        /**<summary>Current Character turn.</summary>*/
        private int _currentTurn;
        /**<summary>Next 5 turns.</summary>*/
        private int[] _nextTurns = {};
        
        /**<summary>The current state of the battle.</summary>*/
        private BattleState _state;
        /**<summary>The scene where you go when the battle finish.</summary>*/
        private string _scene = "RestSystemTest";

        /**<summary>Seconds to wait between turns (finish one and start next).</summary>*/
        private float _secondsToWait;

        #region ClickSystem

        /**<summary>Get the buttons that can click.</summary>*/
        private ClickButton[] _buttonsCanClick;
        
        /**<summary>Camera that check where are the colliders.</summary>*/
        private Camera _camera;

        /**<summary>Get the ID of the button that have been clicked of the buttons that can click.</summary>*/
        private int _pressed = -1;
        /**<summary>Get the ID of the button that have been selected, after pressed.</summary>*/
        private int _selected = -1;

        #endregion

        #endregion

        #region SYSTEM

        private void Awake()
        {
            //While the message exits, the battle cannot continue.
            Message.SetExitsMessage();

            abilitiesOf.velocity = 0.5f;
            memberBase.transform.GetComponent<BoxCollider2D>().enabled = true;
            _camera = Camera.main;
            message.type = TextBoxType.Message;
            message.up = true;
            message.sceneRedirect = "";
            message.messages = GenEnemyMessage();
            
            //There are a text with the rules of the battle..
            if(SavesFiles.GetParty().Length == 0) TestInit1();
            else if (SavesFiles.GetSave().Level == 1)
            {
                TextData[] tx = new TextData[5];
                tx[0] = new TextData("Well, I'm going to explain you what we must do!");
                tx[1] = new TextData("We must attack the enemies with attack or specials abilities.");
                tx[2] = new TextData("The controls are on the left of the window. You can click too!");
                tx[3] = new TextData("Oh, and each character has a Element. The Element indicates the weakness " +
                                     "and the strengths of the characters. I mean, X to do 150% " +
                                     "of the damage to Y, for example.");
                tx[4] = new TextData("If some character hasn't abilities, then the ability panel won't appear.");

                foreach (TextData t in tx)
                {
                    t.speed = message.messages[0].speed;
                }
                message.messages = message.messages.Concat(tx).ToArray();
            }
            
            Instantiate(message, transform);
            
            memberBase.gameObject.SetActive(false);
            memberBase.onlyHUD = true;
            _animatedText = animatedText.transform.GetComponentInChildren<AnimatedText>();
            
            //Characters + Enemies + Controls
            _buttonsCanClick = new ClickButton[MaxMembers + 3 + 4];
            
            InitFighters();
            OrderBySpeed();
            GetExperienceFor();

            Image[] im = transform.Find("TextUI").Find("Controls").GetComponentsInChildren<Image>();
            BoxCollider2D[] bx = transform.Find("TextUI").Find("Controls").GetComponentsInChildren<BoxCollider2D>();
            for (int i = 0; i < 4; i++)
            {
                _buttonsCanClick[i+MaxMembers+3] = new ClickButton(i+MaxMembers+3, im[i], bx[i], "act" + i);
            }
            
            _state = BattleState.Start;
        }

        private void Update()
        {
            //Wait the time.
            _secondsToWait += Time.deltaTime;
            if (Message.ThereAreMessage() || _secondsToWait<1) return;

            #region ClickSystem

            //Click button system.
            if (Input.GetMouseButton(0))
            {
                
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
                
                //Check if it's selected after it had been pressed and released upper the same button.
                if (!hit && _selected > -1)
                {
                    _selected = -1;
                }
                else if (hit && _pressed == -1)
                {
                    _selected =
                        _buttonsCanClick.FirstOrDefault(b => b.collider == hit.collider && b.can)?
                            .id ?? -1;
                    _pressed = _selected;
                }
                else if (!hit && _pressed == -1)
                {
                    _pressed = -2;
                }
                else if (_pressed>-1) 
                    if(hit && hit.collider == _buttonsCanClick[_pressed].collider) 
                        _selected = _pressed;
            }
            _buttonsCanClick = _buttonsCanClick.Select(s => {s.image.color 
                = s.id==_selected? Color.green : s.id == _pressed ? Color.yellow 
                : Color.white; return s; }).ToArray();
            
            if (Input.GetMouseButtonUp(0) && _selected>-1)
            {
                ClickButton.KeyUsed = _buttonsCanClick[_selected].key;
                _pressed = -1;
                _selected = -1;
            }
            else if(Input.GetMouseButtonUp(0) || ClickButton.KeyUsed!="")
            {
                ClickButton.KeyUsed = "";
                _pressed = -1;
                _selected = -1;
            }

            #endregion
            
            switch (_state)
            {
                case BattleState.Start:
                    InitFightersHud();
                    _state = BattleState.Turn;
                    break;
                case BattleState.Turn:
                    UpdateBattlefield();
                    if (_nextTurns.Length <= 1) ChooseTurns();
                    else
                    {
                        turnUI.QuitOld();
                        _nextTurns = _nextTurns.Skip(1).Where(c => !_fighters[c].character.IsKo()).ToArray();
                    }
                    
                    if(_nextTurns.Length > 0) _currentTurn = _nextTurns[0];

                    #region CheckStatus

                    //Check the statuses and update the duration.
                    foreach (StatusOf of in CurrentTurn.Statuses)
                    {
                        of.Duration--;
                        switch (of.Duration)
                        {
                            //If it has a Dead effect, then set KO.
                            case 0 when of.Status.Effect == EffectType.Dead:
                                FighterTurn.SetKo();
                                UpdateBattlefield();
                                return;
                            case 0:
                                CurrentTurn.RemoveStatus(of.Status.ID);
                                break;
                        }
                    }
                    
                    //The damage that the character received by turn.
                    //Debug.Log(CurrentTurn.MaxBloodPoints * CurrentTurn.Regeneration);
                    int damageXTurn = -Convert.ToInt32(CurrentTurn.MaxBloodPoints * CurrentTurn.Regeneration);
                    
                    if(damageXTurn != 0)
                    {
                        CurrentTurn.ReduceCurrentBlood(damageXTurn);
                        DamageAnimation(FighterTurn, damageXTurn, AttackType.Blood);
                        if (CurrentTurn.IsKo())
                        {
                            UpdateBattlefield();
                            return;
                        }
                    }
                    
                    //The karma damage that the character received by turn.
                    damageXTurn = -Convert.ToInt32(CurrentTurn.MaxKarmaPoints * CurrentTurn.KarmaRegeneration);
                    
                    if(damageXTurn != 0)
                    {
                        CurrentTurn.ReduceCurrentKarma(damageXTurn);
                        DamageAnimation(FighterTurn, damageXTurn, AttackType.Karma);
                    }
                    
                    //Don't move.
                    if(CurrentTurn.Statuses.Any(s => s.Status.Effect == EffectType.DontMove)) return;

                    //Confused.
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
                    turnUI.gameObject.SetActive(false);
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
                    turnUI.gameObject.SetActive(false);
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
                    else if(!Message.ThereAreMessage())
                    {
                        for(int i = 0; i < Party.Length; i++)
                        {
                            foreach (StatusOf o in Party[i].Statuses)
                            {
                                if(o.Status.QuitWhenFinish) Party[i].RemoveStatus(o.Status.ID);
                            }
                        }
                        foreach (Character character in SavesFiles.GetParty())
                        {
                            character.RestPoints = 5;
                            character.AbilityPoints += 500;
                        }
                        SavesFiles.GetSave().Level++;
                        SceneManager.LoadScene(_scene);
                    }
                    break;
            }
        }

        #endregion
        
        #region GETTERS

        /**<summary>Get the group of fighters.
        <param name="enemy">If it's true, then it return the enemies, else, it return the party.
        </param></summary>*/
        private Fighter[] GetGroup(bool enemy = false)
        {
            return enemy ? _fighters.Where(f => f.isEnemy && !f.character.IsKo()).ToArray() : 
                _fighters.Where(f => !(f.isEnemy || f.character.IsKo())).ToArray();
        }
        /**<summary>Current Fighter turn.</summary>*/
        public Fighter FighterTurn => _fighters[_currentTurn];
        //private Fighter GetFighterTarget() { return _fighters[_posOfTarget]; }

        /**<summary>Get all Fighters that aren't KO.</summary>*/
        public Fighter[] FighterFighting => _fighters.Where(f => !f.character.IsKo()).ToArray();
        
        /**<summary>Get the character of current turn.</summary>*/
        public Character CurrentTurn => _fighters[_currentTurn].character;
        
        /**<summary>Get all party members fighting (no dead).</summary>*/
        public Fighter[] PartyFighting => GetGroup();

        /**<summary>Get all enemies fighting (no dead).</summary>*/
        public Fighter[] EnemiesFighting => GetGroup(true);

        /**<summary>Get all party members fighting.</summary>*/
        public Character[] Party => _fighters.Where(f => !f.isEnemy).Select(f => f.character).ToArray();

        /**<summary>Get all enemies fighting.</summary>*/
        public Character[] Enemies => _fighters.Where(f => f.isEnemy).Select(f => f.character).ToArray();

        /**<summary>Get all party members.</summary>*/
        public Fighter[] AllParty => _fighters.Where(f => !f.isEnemy).ToArray();

        /**<summary>Get all enemies members.</summary>*/
        public Fighter[] AllEnemies => _fighters.Where(f => f.isEnemy).ToArray();
        
        /**<summary>Get all characters.</summary>*/
        public Character[] AllFighters => _fighters.Select(f => f.character).ToArray();
        
        #endregion

    }
}