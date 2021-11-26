using System;
using System.Linq;
using Core.ButtonsSystem.ButtonList;
using Core.ButtonsSystem.ButtonType;
using Core.Controls;
using Entities;
using Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Battle.BattleSystem
{
    /**<summary>The battle system.</summary>*/
    public partial class BattleSystem
    {

        #region ATTRIBUTES

        /**<summary>The ability list for the player party.</summary>*/
        public AbilityButtonsList abilitiesOf;
        /**<summary>The prefab for character HUDs.</summary>*/
        public MemberHUDButton memberBase;
        /**<summary>The ability that the character has selected.</summary>*/
        private Ability _abilityInUse;

        /**<summary>The target/position where that the character is.</summary>*/
        private int _target;
        /**<summary>Check if the last action is choose ability.</summary>*/
        private bool _lastIsAbility;

        #endregion
        
        #region CHOOSETARGET
        
        /**<summary>Use the ability and check the possible targets.</summary>*/
        private void UseAbility(bool enemies, bool auto = false)
        {
            //Filter group by the enum.
            switch(_abilityInUse.Target)
            {
                case TargetType.Enemy:
                    AbilityRange(GetGroup(!enemies), auto);
                    break;
                case TargetType.Group:
                    AbilityRange(GetGroup(enemies), auto);
                    break;
                case TargetType.Both:
                    AbilityRange(FighterFighting, auto);
                    break;
            }
        }

        /**<summary>Choose the range of the ability.
        <param name="fighters">The fighters that the character can attack.</param>
        </summary>*/
        private void AbilityRange(Fighter[] fighters, bool auto = false)
        {
            if(!auto)
            {
                _buttonsCanClick = _buttonsCanClick.Select(s =>
                {
                    s.can = fighters.Select(i => i.id).ToArray().Contains(s.id) || s.id >= MaxMembers + 3;
                    return s;
                }).ToArray();
            }
            switch (_abilityInUse.Range)
            {
                case TargetRange.One when auto:
                    AttackAll(fighters, true, auto: true);
                    break;
                case TargetRange.One:
                    SelectTarget(fighters);
                    break;
                case TargetRange.All:
                    AttackAll(fighters, auto:auto);
                    break;
                case TargetRange.Random:
                    AttackAll(fighters, true, _abilityInUse.NumberOfTarget, auto);
                    break;
                case TargetRange.Himself when auto:
                    AttackAll(new [] {FighterTurn}, auto:true);
                    break;
                case TargetRange.Himself:
                    SelectTarget(new [] {FighterTurn});
                    break;
                case TargetRange.AllExceptHimself:
                    AttackAll(fighters.Where(c => c.character.ID != CurrentTurn.ID).ToArray());
                    break;
            }
        }
        
        /**<summary>Select the character that can select.
        <param name="fighters">The possible targets.</param></summary>*/
        private void SelectTarget(Fighter[] fighters)
        {
            if (Input.GetKeyDown(ControlsKeys.MoveRight) 
                //|| Input.GetKeyDown(ControlsKeys.MoveUp)
                )
            {
                fighters[_target].CharacterMark();
                _target++;
                _target %= fighters.Length;
                fighters[_target].CharacterMark(true);
            }
            else if (Input.GetKeyDown(ControlsKeys.MoveLeft) 
                     //|| Input.GetKeyDown(ControlsKeys.MoveDown)
                     )
            {
                fighters[_target].CharacterMark();
                _target--;
                _target = _target < 0 ? fighters.Length-1 : _target;
                fighters[_target].CharacterMark(true);
            }

            if (_target < 0 || _target >= fighters.Length) _target = 0;
            fighters[_target].CharacterBlink();

            if(Input.GetKeyDown(ControlsKeys.Ok))
            {
                DoAttack(FighterTurn, _fighters[fighters[_target].id]);
            }else if (!ClickButton.KeyUsed.Equals(""))
            {
                int? pos = _buttonsCanClick.Take(MaxMembers + 3).FirstOrDefault(b =>
                    b.key.Equals(ClickButton.KeyUsed))?.id;
                if(pos != null) DoAttack(FighterTurn, _fighters[(int)pos]);
            }
        }

        /**<summary>Attack all enemies.
        <param name="fighters">The enemies to attack.</param></summary>*/
        private void AttackAll(Fighter[] fighters, bool random = false, int numberOfRandom = 1, bool auto = false)
        {
            foreach (Fighter fighter in fighters)
            {
                fighter.CharacterBlink();
            }

            if (random)
            {
                int pos = 0;

                numberOfRandom = numberOfRandom > fighters.Length ? fighters.Length : numberOfRandom;
                int[] targets = new int[numberOfRandom];
                
                while (pos<numberOfRandom)
                {
                    int numberToChoose = Random.Range(0, fighters.Length);
                        
                    targets[pos] = fighters[numberToChoose].id;
                    if (!_abilityInUse.CanRepeatRandomTarget)
                    {
                        for (int i = numberToChoose; i < fighters.Length-1; i++)
                        {
                            fighters[i] = fighters[i + 1];
                            Array.Resize(ref fighters, fighters.Length-1);
                        }
                    }
                    pos++;
                }
                fighters = targets.Select(t => _fighters[t]).ToArray();

            }
            
            //Press OK, Click in any button action or is auto.
            if (!(Input.GetKeyDown(ControlsKeys.Ok) || 
                 _buttonsCanClick.Take(MaxMembers+3).Any(s => ClickButton.KeyUsed.Equals(s.key))) 
                 && !auto) return;
            
            InitMark();
            DoAttack(FighterTurn, fighters);
        }
        
        #endregion
        
        #region TURN

        /**<summary>It manage the character action.</summary>*/
        private void CharacterTurn(bool enemy = false)
        {
            switch (_actionType)
            {
                //Choose action
                case ActionType.None when Input.GetKeyDown(ControlsKeys.Ok) 
                                          || ClickButton.KeyUsed == _buttonsCanClick[MaxMembers+3].key:
                    _actionType = ActionType.Atk1;
                    FighterTurn.CharacterMark();
                    break;
                case ActionType.None when (Input.GetKeyDown(ControlsKeys.ActionButton1)
                                           || ClickButton.KeyUsed == _buttonsCanClick[MaxMembers+5].key) 
                                          && CurrentTurn.Abilities().Length!=0:
                    abilitiesOf.character = FighterTurn.character;
                    abilitiesOf.transform.gameObject.SetActive(true);
                    _actionType = ActionType.Ability;
                    FighterTurn.CharacterMark();
                    break;
                case ActionType.None when Input.GetKeyDown(ControlsKeys.ActionButton2)
                                          || ClickButton.KeyUsed == _buttonsCanClick[MaxMembers+4].key:
                    _actionType = ActionType.Atk2;
                    FighterTurn.CharacterMark();
                    break;
                //Melee attack
                case ActionType.Atk1:
                    _abilityInUse = FighterTurn.character.MeleeAttack;
                    _actionType = ActionType.Process;
                    break;
                //Long attack
                case ActionType.Atk2:
                    _abilityInUse = FighterTurn.character.LongAttack;
                    _actionType = ActionType.Process;
                    break;
                //Ability
                case ActionType.Ability:
                    if(GenericButton.Message.Equals("")) break;
                    _lastIsAbility = true;
                    _abilityInUse = CurrentTurn.GetAbility(Convert.ToInt32(GenericButton.Message));
                    _actionType = ActionType.Process;
                    break;
                case ActionType.Process:
                    UseAbility(enemy);
                    break;
                default:
                    FighterTurn.CharacterBlink();
                    break;
                
            }

            //Return back or open the ability list if the last action was it.
            switch (_actionType)
            {
                case ActionType.Process when Input.GetKeyDown(ControlsKeys.Back) 
                                             || ClickButton.KeyUsed.Equals(_buttonsCanClick[MaxMembers+6].key):
                {
                    InitMark();
                    if (_lastIsAbility)
                    {
                        abilitiesOf.gameObject.SetActive(true);
                        _actionType = ActionType.Ability;
                        _abilityInUse = null;
                        _lastIsAbility = false;
                    }
                    else _actionType = ActionType.None;

                    break;
                }
                case ActionType.Ability when Input.GetKeyDown(ControlsKeys.Back)
                                             || ClickButton.KeyUsed.Equals(_buttonsCanClick[MaxMembers+6].key):
                    _actionType = ActionType.None;
                    break;
            }

        }

        #endregion
        
    }
}