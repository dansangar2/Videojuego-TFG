using System;
using System.Collections.Generic;
using System.Linq;
using Core.Battle.DamageText;
using Core.ButtonsSystem.ButtonType;
using Core.Controls;
using Core.Messages;
using Core.Saves;
using Entities;
using Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Battle.BattleSystem
{
    public partial class BattleSystem
    {
        
        //Note: Some elements haven't sense without the models, but could break the code if deleted it. 
        
        #region ATTRIBUTES

        /**<summary>The prefab of the text that appear when the character receive damage or experience.</summary>*/
        public GameObject animatedText;
        
        /**<summary>The current action status of the current character.</summary>*/
        private ActionType _actionType;
        /**<summary>The array with the characters IDs order by speed.</summary>*/
        private int[] _orderBySpeed;
        /**<summary>The current animated text.</summary>*/
        private AnimatedText _animatedText;
        /**<summary>The array with the experience that the characters will gain [charThatGained, enemyFrom].</summary>*/
        private int[,] _experienceGained;
        
        #endregion

        #region MESSAGE

        /**<summary>It generate a enemy appear message.</summary>*/
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

        /**<summary>Order characters IDs by fighter speed.</summary>*/
        private void OrderBySpeed()
        {
            _orderBySpeed = _fighters.OrderByDescending(m => m.character.Agility)
                .Select(f => f.id).ToArray();
        }

        #endregion
        
        #region INIT

        /**<summary>Init the Fighter: character and enemies model, HUDs, renders, etc...</summary>*/
        private void InitFighters()
        {

            _fighters = new Fighter[MaxMembers+EnemiesId.Length];
            
            //MeshRenderer[] fighterRenderers = GetComponentsInChildren<MeshRenderer>();
            //MeshCollider[] fightersColliders = GetComponentsInChildren<MeshCollider>();
            //MeshFilter[] fightersFilters = GetComponentsInChildren<MeshFilter>();

            for (int i = 0; i<3 ;i++)
            {
                if(i< MaxMembers)
                {
                    _fighters[i] = gameObject.AddComponent<Fighter>();
                    _fighters[i].SetData(i, statusUI, memberBase, playerPanel
                        //, fightersFilters, fighterRenderers, fightersColliders
                        );
                    _fighters[i].character.ResetCharge();
                    _fighters[i].CharacterMark();
                    _buttonsCanClick[i] = new ClickButton(i, 
                        _fighters[i].member.ImageOfButton, 
                        _fighters[i].collider, "char" + i, false);
                }/*else
                {
                    Destroy(fightersFilters[i].gameObject);
                }*/

            }
            
            /*for (int i = 0; i < 3; i++)
            {
                fighterRenderers[MaxMembers + i] = fighterRenderers[3 + i];
                fightersColliders[MaxMembers + i] = fightersColliders[3 + i];
                fightersFilters[MaxMembers + i] = fightersFilters[3 + i];
            }*/

            for (int i = MaxMembers; i<_fighters.Length ;i++)
            {
                _fighters[i] = gameObject.AddComponent<Fighter>();
                _fighters[i].SetData(i, EnemiesId, statusUI, memberBase, enemyPanel
                    //, fightersFilters, fighterRenderers, fightersColliders
                    );
                _fighters[i].character.ResetCharge();
                _fighters[i].CharacterMark();
                _buttonsCanClick[i] = new ClickButton(i, 
                    _fighters[i].member.ImageOfButton, 
                    _fighters[i].collider, "enem" + i, false);
            }
        }

        /**<summary>Set active the HUDs of the characters when the enemies appeared message dead.</summary>*/
        private void InitFightersHud()
        {
            foreach (Fighter fighter in _fighters)
            {
                fighter.member.gameObject.SetActive(true);
            }
        }

        
        /**<summary>It get the next 5 characters turns.</summary>*/
        private void ChooseTurns()
        {
            
            int total = _fighters.Select(i => i.character.Agility).Sum();
            _nextTurns = new int[5];
            int j = 0;
            while (true)
            {
                
                foreach (int i in _orderBySpeed)
                {
                    if (_fighters[i].character.IsKo()) continue;
                    if (!_fighters[i].character.AddCharge(total)) continue;
                    _nextTurns[j] = i;
                    j++;
                    if(j >= 5)
                    {
                        turnUI.SetUp(_fighters, _nextTurns);
                        return;
                    }
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

        /**<summary>Set the damage by speed and it makes the float text object.</summary>*/
        public void DoAttack(Fighter attacker, params Fighter[] targets)
        {
            _buttonsCanClick = _buttonsCanClick.Select(s =>
            {
                s.can = s.id >= MaxMembers + 3;
                return s;
            }).ToArray();
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
            _abilityInUse = null;
            UpdateBattlefield();
            _secondsToWait = 0;
            GenericButton.Message = "";
        }

        /**<summary>It make an animation text.</summary>*/
        public void DamageAnimation(Fighter fighter, int damage)
        {
            DamageAnimation(fighter, damage, _abilityInUse.Type);
        }
        
        /**<summary>It make an animation text, depending of the type.</summary>*/
        public void DamageAnimation(Fighter fighter, int damage, AttackType type)
        {
            if(damage!=0 || type!=AttackType.Blood) _animatedText.SetDamage(damage, fighter, type);
        }
        
        /**<summary>Update the stats/status of the members of the battle.</summary>*/
        public void UpdateBattlefield()
        {
            int[] ids = { };
            foreach (Fighter f in _fighters)
            {
                if (f.character.IsKo())
                {
                    f.SetKo();
                    if (!f.isEnemy) continue;
                    Array.Resize(ref ids, ids.Length + 1);
                    ids[ids.Length - 1] = f.id;
                }
                f.CharacterMark();
            }
            AddExperience(ids);
            if (PartyFighting.Length == 0) _state = BattleState.Lose;
            else if (EnemiesFighting.Length==0) _state = BattleState.Win;
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
                        Mathf.Round(EnemiesFighting[j].character.NedExp *
                                    //The max increment of experience is x2 if the enemy it's 20 level upper.
                                    (1 + Convert.ToSingle(
                                        EnemiesFighting[j].character.Level 
                                        - SavesFiles.GetParty()[i].Level)/20)));
                    // The min is 1.
                    exp = Mathf.Min(exp, EnemiesFighting[j].character.NedExp*2);
                    exp = Mathf.Max(1, exp);

                    experienceGained[Array.IndexOf(
                        SavesFiles.GetParty().Select(c => c.ID).ToArray(),
                        SavesFiles.GetParty()[i].ID),j] = exp;
                    
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
                        _experienceGained[Array.IndexOf(
                            SavesFiles.GetParty().Select(c => c.ID).ToArray(),
                            character.ID), id - MaxMembers]*played)));
                    _experienceGained[Array.IndexOf(
                        SavesFiles.GetParty().Select(c => c.ID).ToArray(),
                        character.ID), id - MaxMembers] = 0;
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