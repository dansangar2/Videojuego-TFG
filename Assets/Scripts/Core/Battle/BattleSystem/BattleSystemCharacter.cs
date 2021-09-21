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
    public partial class BattleSystem
    {

        #region ATTRIBUTES

        /**<summary>The ability list for the player party.</summary>*/
        public AbilityButtonsList abilitiesOf;
        /**<summary>The prefab for character HUBs.</summary>*/
        public MemberHUDButton memberBase;
        /**<summary>The ability that the character has selected.</summary>*/
        private Ability _abilityInUse;

        /**<summary>The target that the character has selected.</summary>*/
        private int _target;

        #endregion
        
        #region CHOOSETARGET
        
        /**<summary>Use the ability and check the possible targets.</summary>*/
        private void UseAbility(bool enemies, bool auto = false)
        {
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
            switch (_abilityInUse.Range)
            {
                case TargetRange.One when auto:
                    AttackAll(fighters, true, confused: true);
                    break;
                case TargetRange.One:
                    SelectTarget(fighters);
                    break;
                case TargetRange.All:
                    AttackAll(fighters, confused:auto);
                    break;
                /*case TargetRange.MoreThanOne:
                    AttackAll(fighters, false, _abilityInUse.NumberOfTarget);
                    break;*/
                case TargetRange.Random:
                    AttackAll(fighters, true, _abilityInUse.NumberOfTarget, auto);
                    break;
                case TargetRange.Himself when auto:
                    AttackAll(new [] {FighterTurn}, confused:true);
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
            if (Input.GetKeyDown(ControlsKeys.MoveLeft) 
                || Input.GetKeyDown(ControlsKeys.MoveUp))
            {
                fighters[_target].CharacterMark();
                _target++;
                _target %= fighters.Length;
                fighters[_target].CharacterMark(true);
            }
            else if (Input.GetKeyDown(ControlsKeys.MoveRight) 
                     || Input.GetKeyDown(ControlsKeys.MoveDown))
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
            }
        }

        /**<summary>Attack all enemies.
        <param name="fighters">The enemies to attack.</param></summary>*/
        private void AttackAll(Fighter[] fighters, bool random = false, int numberOfRandom = 1, bool confused = false)
        {
            foreach (Fighter fighter in fighters)
            {
                fighter.CharacterBlink();
            }

            if (random)
            {
                int[] targets = new int[numberOfRandom];
                int pos = 0;

                numberOfRandom = numberOfRandom > fighters.Length ? fighters.Length : numberOfRandom;

                while (pos<numberOfRandom)
                {
                    if (!_abilityInUse.CanRepeatRandomTarget)
                    {
                        int numberToChoose = Random.Range(0, fighters.Length);
                        
                        targets[pos] = fighters[numberToChoose].id;
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

            if (!(Input.GetKeyDown(ControlsKeys.Ok) || confused)) return;
            
            InitMark();
            DoAttack(FighterTurn, fighters);
        }
        
        #endregion
        
        #region TURN

        private void CharacterTurn(bool enemy = false)
        {

            switch (_actionType)
            {
                //Choose action
                case ActionType.None when Input.GetKeyDown(ControlsKeys.Ok):
                    _actionType = ActionType.Melee;
                    FighterTurn.CharacterMark();
                    break;
                case ActionType.None when Input.GetKeyDown(ControlsKeys.ActionButton1):
                    abilitiesOf.character = FighterTurn.character;
                    _abilityList = Instantiate(abilitiesOf, transform.GetChild(0));
                    _actionType = ActionType.Ability;
                    FighterTurn.CharacterMark();
                    break;
                case ActionType.None when Input.GetKeyDown(ControlsKeys.ActionButton2):
                    //_actionType = ActionType.Item;
                    break;
                case ActionType.None when Input.GetKeyDown(ControlsKeys.Back):
                    _actionType = ActionType.Long;
                    FighterTurn.CharacterMark();
                    break;
                //Melee attack
                case ActionType.Melee:
                    _abilityInUse = FighterTurn.character.MeleeAttack;
                    _actionType = ActionType.Process;
                    break;
                //Long attack
                case ActionType.Long:
                    _abilityInUse = FighterTurn.character.LongAttack;
                    _actionType = ActionType.Process;
                    break;
                //Use item
                case ActionType.Item:
                    break;
                //Use Ability
                case ActionType.Ability:
                    if(GenericButton.Message.Equals("")) break;
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

            switch (_actionType)
            {
                case ActionType.Process when Input.GetKeyDown(ControlsKeys.Back):
                {
                    InitMark();
                    if (!GenericButton.Message.Equals(""))
                    {
                        _abilityList = Instantiate(abilitiesOf, transform.GetChild(0));
                        _actionType = ActionType.Ability;
                        _abilityInUse = null;
                    }
                    else _actionType = ActionType.None;

                    break;
                }
                case ActionType.Ability when Input.GetKeyDown(ControlsKeys.Back):
                    _actionType = ActionType.None;
                    break;
            }

        }

        #endregion
        
    }
}