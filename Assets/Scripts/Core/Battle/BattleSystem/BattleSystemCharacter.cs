using System;
using System.Linq;
using Core.Buttons;
using Core.Controls;
using Data;
using Entities;
using Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Battle.BattleSystem
{
    public partial class BattleSystem
    {

        private Ability _abilityInUse;

        private int _target;
        private int _posOfTarget;
        private bool _lastActionSelectAbility;
        
        #region CHOOSETURN
        
        private void UseAbility(bool enemies)
        {
            
            switch(_abilityInUse.Target)
            {
                case TargetType.Enemy:
                    AbilityRange(GetGroup(!enemies));
                    break;
                case TargetType.Group:
                    AbilityRange(GetGroup(enemies));
                    break;
                case TargetType.Both:
                    AbilityRange(_fighters);
                    break;
            }
        }

        private void AbilityRange(Fighter[] fighters)
        {
            
            switch (_abilityInUse.Range)
            {
                case TargetRange.One:
                    SelectTarget(fighters);
                    break;
                case TargetRange.All:
                    AttackAll(fighters);
                    break;
                case TargetRange.MoreThanOne:
                    AttackAll(fighters, false, _abilityInUse.NumberOfTarget);
                    break;
                case TargetRange.Random:
                    AttackAll(fighters, true, _abilityInUse.NumberOfTarget);
                    break;
                case TargetRange.Himself:
                    SelectTarget(new [] {GetFighterTurn()});
                    break;
            }
        }
        
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
            fighters[_target].CharacterBlink();
            
            if(Input.GetKeyDown(ControlsKeys.Ok)) DoAttack(GetFighterTurn(), _fighters[fighters[_target].id]);
        }

        private void AttackAll(Fighter[] fighters, bool random = false, int numberOfRandom = 1)
        {
            foreach (Fighter fighter in fighters)
            {
                fighter.CharacterBlink(velocity);
            }

            if (random)
            {
                int[] targets = new int[numberOfRandom];
                int pos = 0;

                while (pos<numberOfRandom)
                {
                    int numberToChoose = fighters[Random.Range(0, fighters.Length)].id;
                    if (!targets.Contains(numberToChoose) || _abilityInUse.CanRepeatRandomTarget)
                    {
                        targets[pos] = numberToChoose;
                        pos++;
                        
                    }else if (_abilityInUse.CanRepeatRandomTarget) pos++;
                }

                fighters = targets.Select(t => _fighters[t]).ToArray();

            }

            if (!Input.GetKeyDown(ControlsKeys.Ok)) return;
            
            foreach (Fighter fighter in _fighters)
            { 
                fighter.CharacterMark();
            }
            DoAttack(GetFighterTurn(), fighters);
        }
        
        #endregion
        
        #region TURN

        private void CharacterTurn()
        {
            switch (_actionType)
            {
                //Choose action
                case ActionType.None when Input.GetKeyDown(ControlsKeys.Ok):
                    _actionType = ActionType.Melee;
                    GetFighterTurn().CharacterMark();
                    break;
                case ActionType.None when Input.GetKeyDown(ControlsKeys.ActionButton1) || _lastActionSelectAbility:
                    _lastActionSelectAbility = false;
                    abilitiesOf.character = GetFighterTurn().character;
                    _abilityList = Instantiate(abilitiesOf, transform.GetChild(0));
                    //abilitiesOf.SetUp(_fighters[_currentTurn].character.ID,
                    //    transform.GetChild(0).transform);
                    _actionType = ActionType.Ability;
                    GetFighterTurn().CharacterMark();
                    break;
                case ActionType.None when Input.GetKeyDown(ControlsKeys.ActionButton2):
                    //_actionType = ActionType.Item;
                    break;
                case ActionType.None when Input.GetKeyDown(ControlsKeys.Back):
                    _actionType = ActionType.Long;
                    GetFighterTurn().CharacterMark();
                    break;
                //Melee attack
                case ActionType.Melee:
                    _abilityInUse = GetFighterTurn().character.MeleeAttack;
                    _actionType = ActionType.Process;
                    //UseAbility(false);
                    break;
                //Long attack
                case ActionType.Long:
                    _abilityInUse = GetFighterTurn().character.LongAttack;
                    _actionType = ActionType.Process;
                    //UseAbility(false);
                    break;
                //Use item
                case ActionType.Item:
                    break;
                //Use Ability
                case ActionType.Ability:
                    if (_abilityList.Equals(null))
                    {
                        _actionType = ActionType.None;
                    }
                    if(Button.Message.Equals("")) return;
                    _abilityInUse = GameData.AbilityDB.FindByID(Convert.ToInt32(Button.Message));
                    _lastActionSelectAbility = true;
                    _actionType = ActionType.Process;
                    //Destroy(transform.parent.GetChild(transform.GetChild(0).childCount-1).gameObject);
                    //Button.message = "";
                    //_actionType = ActionType.Process;
                    break;
                case ActionType.Process:
                    
                    UseAbility(false);
                    break;
                default:
                    GetFighterTurn().CharacterBlink(velocity);
                    break;
                
            }

            if (_actionType!=ActionType.None && Input.GetKeyDown(ControlsKeys.Back))
            {
                foreach (Fighter f in _fighters) { f.CharacterMark(); } 
                //Destroy(_abilityList.transform.gameObject);
                //if(lastActionSelectAbility)
                //else
                _actionType = ActionType.None;
            }
            
        }

        #endregion
        
    }
}