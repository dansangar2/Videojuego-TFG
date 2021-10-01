using System;
using System.Collections.Generic;
using System.Linq;
using Core.Battle.DamageText;
using Core.ButtonsSystem.ButtonType;
using Core.Messages;
using Core.Saves;
using Entities;
using Enums;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Core.Battle.BattleSystem
{
    public partial class BattleSystem
    {
        
        #region ATTRIBUTES

        /**<summary>The text that appear when the character receive damage or experience.</summary>*/
        public GameObject animatedText;
        
        /**<summary>Current Character turn.</summary>*/
        private int _currentTurn;
        /**<summary>The current action of the character selected.</summary>*/
        private ActionType _actionType;
        /**<summary>The array with the characters ids order by speed.</summary>*/
        private int[] _orderBySpeed;
        /**<summary>The current animated text.</summary>*/
        private AnimatedText _animatedText;
        /**<summary>The array with the experience gained [charThatGained, enemyFrom].</summary>*/
        private int[,] _experienceGained;
        
        #endregion

        #region MESSAGE

        /**<summary>It generate a enemy appeared message.</summary>*/
        private TextData[] GenEnemyMessage(float speed = 0.02f)
        {
            
            Dictionary<int, int> results = new Dictionary<int, int>();
            foreach (int id in EnemiesId)
            {
                if(results.ContainsKey(id)) continue;
                int i = EnemiesId.Count(n => n==id);
                results.Add(id, i);
            }

            string messages = results.Select(i 
                    => (i.Value.ToString().Equals("1") ? "" : i.Value.ToString()) + " " +
                       SavesFiles.GetSave().GetEnemy(i.Key).Name)
                .Aggregate((i, j) => i + ", " + j) + " was detected!";
            
            if(messages.Contains(',')) messages = messages.Substring(0, messages.LastIndexOf(','))
                + " and" + messages.Substring(messages.LastIndexOf(',')+1);

            TextData data = new TextData(messages) {speed = speed};
            return new []{data};
        }

        #endregion

        #region ORDER

        /**<summary>Order by speed array with the ids of the characters.</summary>*/
        private void OrderBySpeed()
        {
            _orderBySpeed = _fighters.OrderByDescending(m => m.character.Agility)
                .Select(f => f.id).ToArray();
            
        }

        #endregion
        
        #region INIT

        /**<summary>Init the character and enemies model, HUDs, renders, etc...</summary>*/
        private void InitFighters()
        {

            _fighters = new Fighter[MaxMembers+EnemiesId.Length];
            
            MeshRenderer[] fighterRenderers = GetComponentsInChildren<MeshRenderer>();
            MeshCollider[] fightersColliders = GetComponentsInChildren<MeshCollider>();
            MeshFilter[] fightersFilters = GetComponentsInChildren<MeshFilter>();

            for (int i = 0; i<3 ;i++)
            {
                if(i< MaxMembers)
                {
                    _fighters[i] = gameObject.AddComponent<Fighter>();
                    _fighters[i].SetData(i, statusUI, memberBase, GetComponentInChildren<GridLayoutGroup>(),
                        fightersFilters, fighterRenderers, fightersColliders);
                    _fighters[i].character.ResetCharge();
                    _fighters[i].CharacterMark();
                }else
                {
                    Destroy(fightersFilters[i].gameObject);
                }

            }
            
            for (int i = 0; i < 3; i++)
            {
                fighterRenderers[MaxMembers + i] = fighterRenderers[3 + i];
                fightersColliders[MaxMembers + i] = fightersColliders[3 + i];
                fightersFilters[MaxMembers + i] = fightersFilters[3 + i];
            }

            for (int i = MaxMembers; i<_fighters.Length ;i++)
            {
                _fighters[i] = gameObject.AddComponent<Fighter>();
                _fighters[i].SetData(i, EnemiesId, statusUI, fightersFilters, fighterRenderers, fightersColliders);
                _fighters[i].character.ResetCharge();
                _fighters[i].CharacterMark();
            }
        }

        /**<summary>Set active the HUDs of the characters when the enemies appeared message dead.</summary>*/
        private void InitFightersHud()
        {
            foreach (Fighter fighter in _fighters.Where(m => !m.isEnemy).ToArray())
            {
                fighter.member.gameObject.SetActive(true);
            }
        }

        
        /**<summary>It get the next character turn.</summary>*/
        private void ChooseTurns()
        {
            
            int total = _fighters.Select(i => i.character.Agility).Sum();
            while (true)
            {
                
                foreach (int i in _orderBySpeed)
                {
                    if (_fighters[i].character.IsKo()) continue;
                    if (!_fighters[i].character.AddCharge(total)) continue;
                    _currentTurn = i;
                    return;
                }
            }
        }

        /**<summary>Select a random attack.</summary>*/
        public void RandomAttack(bool enemies)
        {
            int abi = Random.Range(-2, CurrentTurn.Abilities().Count(a => a.Cost<= CurrentTurn.CurrentKarmaPoints));
            _abilityInUse = abi switch
            {
                -2 => CurrentTurn.LongAttack,
                -1 => CurrentTurn.MeleeAttack,
                _ => CurrentTurn.Abilities().Where(a => a.Cost<= CurrentTurn.CurrentKarmaPoints).ToArray()[abi]
            };
            UseAbility(enemies, true);
        }
        
        #endregion

        #region DAMAGE

        /**<summary>The array with the characters ids order by speed.</summary>*/
        public void DoAttack(Fighter attacker, params Fighter[] targets)
        {
            _actionType = ActionType.Process;
            attacker.character.ReduceCurrentKarma(_abilityInUse.Cost);
            attacker.CharacterMark();
            int recovery = 0;
            foreach (Fighter fighter in targets)
            {
                for (int i = 0; i < _abilityInUse.Hits; i++)
                {
                    int damage = _abilityInUse.Damage(attacker.character, fighter.character);
                    recovery += damage;
                    DamageAnimation(fighter, damage);
                }
            }
            if(_abilityInUse.Type == AttackType.AbsorbBlood || _abilityInUse.Type == AttackType.AbsorbKarma) 
                DamageAnimation(attacker,Convert.ToInt32(-recovery*0.4f));
            _actionType = ActionType.None;
            UpdateBattlefield();
            _secondsToWait = 0;
            GenericButton.Message = "";
        }

        public void DamageAnimation(Fighter fighter, int damage)
        {
            DamageAnimation(fighter, damage, _abilityInUse.Type);
        }
        
        public void DamageAnimation(Fighter fighter, int damage, AttackType type)
        {
            _animatedText.SetDamage(damage, fighter, type);
        }
        
        public void UpdateBattlefield()
        {
            int[] ids = { };
            foreach (Fighter f in _fighters)
            {
                if (//f.character.Statuses
                   //.Any(s => s.Status.Effect == EffectType.Dead) || 
                    f.character.IsKo())
                {
                    f.SetKo();
                    if (!f.isEnemy) continue;
                    Array.Resize(ref ids, ids.Length + 1);
                    ids[ids.Length - 1] = f.id;
                }
                f.CharacterMark();
            }
            AddExperience(ids);
            if (PartyFighter.Length == 0) _state = BattleState.Lose;
            else if (EnemiesFighter.Length==0) _state = BattleState.Win;
            else _state = BattleState.Turn;
        }

        #endregion

        #region EXPERIENCE

        /**<summary>Get the experience that the character will gain after defeat a enemy.</summary>*/
        private void GetExperienceFor()
        {
            int[,] experienceGained = new int[SavesFiles.GetParty().Length, GetGroup(true).Length];

            for (int i = 0; i < SavesFiles.GetParty().Length; i++)
            {
                for (int j = 0; j < GetGroup(true).Length; j++)
                {
                    //The experience gained is reduced or increment if the
                    //character level is lower o upper than the enemy.
                    int exp = Convert.ToInt32(
                        //The experience gained is reduced if the character isn't in the battlefield.
                        Mathf.Round(EnemiesFighter[j].character.NedExp *
                                    //The max increment of experience is x2 if the enemy it's 20 level upper.
                                    (1 + Convert.ToSingle(
                                        EnemiesFighter[j].character.Level 
                                        - SavesFiles.GetParty()[i].Level)/20)));
                    Debug.Log("Percentage LvC " + SavesFiles.GetParty()[i].Level + " LvE " + 
                              EnemiesFighter[j].character.Level + " perc " + Convert.ToSingle(
                        EnemiesFighter[j].character.Level 
                        - SavesFiles.GetParty()[i].Level)/20);
                    Debug.Log("Init "+ EnemiesFighter[j].character.NedExp);
                    Debug.Log("Final " + exp);
                    // The min is 1.
                    exp = Mathf.Min(exp, EnemiesFighter[j].character.NedExp*2);
                    exp = Mathf.Max(1, exp);
                    
                    experienceGained[SavesFiles.GetParty()[i].ID,j] = exp;
                }
            }
            
            _experienceGained = experienceGained;
        }

        /**<summary>Add the experience to the characters when the enemy is defeated.</summary>*/
        private void AddExperience(params int[] ids)
        {
            if(ids.Length == 0) return;
            int i = 0;
            
            foreach (Character character in SavesFiles.GetParty())
            {
                int exp = 0;
                //Check if the character is in the battlefield. If not his experience is 80%.
                float played = i < MaxMembers ? 1 : 0.8f;

                foreach (int id in ids)
                {
                    exp += Convert.ToInt32(Mathf.Max(0, Mathf.Floor(
                        _experienceGained[character.ID, id - MaxMembers]*played)));
                    _experienceGained[character.ID, id - MaxMembers] = 0;
                }
                
                character.GainExperience(exp);
                
                //Print the exp. gained on left of the character if not is 0.
                if (i<MaxMembers && exp != 0) _animatedText.GainExp(exp, _fighters[i]);
                i++;
            }
        }

        #endregion

        #region RESSET COLOR

        /**<summary>Set the character color to default.</summary>*/
        public void InitMark()
        {
            foreach (Fighter f in _fighters.Where(f => !f.character.IsKo()))
            {
                f.CharacterMark();
            }
        }

        #endregion

    }
}