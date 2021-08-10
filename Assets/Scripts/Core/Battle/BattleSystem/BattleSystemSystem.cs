using System;
using System.Collections.Generic;
using System.Linq;
using Core.Battle.DamageText;
using Core.ButtonsSystem.ButtonList;
using Core.ButtonsSystem.ButtonType;
using Core.Messages;
using Core.Saves;
using Data;
using Entities;
using Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Battle.BattleSystem
{
    public partial class BattleSystem
    {
        
        #region ATTRIBUTES

        /**<summary>The text that appear when the character receive damage or experience.</summary>*/
        public GameObject animatedText;
        
        /**<summary>The array with the characters ids order by speed.</summary>*/
        private int[] _orderBySpeed;
        
        /**<summary>The current animated text.</summary>*/
        private AnimatedText _animatedText;

        /**<summary>List of abilities of the characters.</summary>*/
        private AbilityButtonsList _abilityList;

        #endregion

        #region MESSAGE

        /**<summary>It generate a enemy appeared message.</summary>*/
        private TextData[] GenEnemyMessage(float speed = 0.02f)
        {
            
            Dictionary<int, int> results = new Dictionary<int, int>();
            foreach (int id in enemiesId)
            {
                if(results.ContainsKey(id)) continue;
                int i = enemiesId.Count(n => n==id);
                results.Add(id, i);
            }

            string messages = results.Select(i 
                    => (i.Value.ToString().Equals("1") ? "" : i.Value.ToString()) + " " +
                       GameData.EnemyDB.FindByID(i.Key).Name)
                .Aggregate((i, j) => i + ", " + j) + " was detected!";
            
            messages = messages.Substring(0, messages.LastIndexOf(','))
                + " and" + messages.Substring(messages.LastIndexOf(',')+1);

            TextData data = new TextData(messages) {speed = speed};
            return new []{data};
        }

        #endregion

        #region ORDER

        /**<summary>Set the order by speed array with the ids of the characters.</summary>*/
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
            int members = maxMembers > SavesFiles.GetSave().Party.Length
                ? SavesFiles.GetSave().Party.Length : maxMembers;

            _fighters = new Fighter[members+enemiesId.Length];
            
            MeshRenderer[] fighterRenderers = GetComponentsInChildren<MeshRenderer>();
            MeshCollider[] fightersColliders = GetComponentsInChildren<MeshCollider>();
            MeshFilter[] fightersFilters = GetComponentsInChildren<MeshFilter>();

            for (int i = 0; i<maxMembers ;i++)
            {
                try
                {
                    _fighters[i] = gameObject.AddComponent<Fighter>();
                    _fighters[i].SetData(i, memberBase, GetComponentInChildren<GridLayoutGroup>(),
                        fightersFilters, fighterRenderers, fightersColliders);
                    _fighters[i].character.ResetCharge();
                    _fighters[i].CharacterMark();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                    Destroy(fightersFilters[i].gameObject);
                }

            }
            
            for (int i = members; i<_fighters.Length ;i++)
            {
                _fighters[i] = gameObject.AddComponent<Fighter>();
                _fighters[i].SetData(i, enemiesId, fightersFilters, fighterRenderers, fightersColliders);
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
                    if (!_fighters[i].character.AddCharge(total)) continue;
                    _currentTurn = i;
                    return;
                }
            }
        }
        
        #endregion

        #region DAMAGE

        /**<summary>The array with the characters ids order by speed.</summary>*/
        public void DoAttack(Fighter attacker, params Fighter[] targets)
        {
            List<int> idsToDestroy = new List<int>();
            _actionType = ActionType.Process;
            attacker.character.ReduceCurrentKarma(_abilityInUse.Cost);
            attacker.CharacterMark();
            foreach (Fighter fighter in targets)
            {
                for (int i = 0; i < _abilityInUse.Hits; i++)
                {
                    Damage(fighter, _abilityInUse.Damage(attacker.character, fighter.character));
                }

                if(fighter.character.IsKo() && fighter.isEnemy) idsToDestroy.Add(fighter.id);
                if(fighter.character.IsKo() && !fighter.isEnemy) fighter.meshRenderer.material.color = Color.red;
            }
            foreach (Fighter f in GetGroup()) { f.member.UpdateUI(); }
            UpdateBattlefield(idsToDestroy.OrderBy(i => i).ToArray());
            _actionType = ActionType.None;
            _lastActionSelectAbility = false;
            GenericButton.Message = "";
        }

        public void UpdateBattlefield(params int[] ids)
        {
            for (int id = ids.Length-1; id >= 0; id--)
            {
                AddExperience(ids);
                Destroy(_fighters[ids[id]].meshFilter.gameObject);
                Destroy(_fighters[ids[id]]);
                for (int i = ids[id]; i < _fighters.Length-1; i++) 
                {
                    _fighters[i] = _fighters[i+1]; 
                    _fighters[i].id--;
                } 
                Array.Resize(ref _fighters, _fighters.Length - 1);
                _orderBySpeed = _fighters.OrderByDescending(f => f.character.Agility).Select(f => f.character.ID)
                    .ToArray();
            }
            if (GetGroup().Length == 0) _state = BattleState.Lose;
            else if (GetGroup(true).Length==0) _state = BattleState.Win;
            else _state = BattleState.Turn;
        }
        
        public void Damage(Fighter fighter, int damage)
        {
            fighter.CharacterMark();
            _animatedText.SetDamage(damage, fighter, _abilityInUse.Type);
            if (_abilityInUse.Type == AttackType.Blood) fighter.character.ReduceCurrentBlood(damage);
            else fighter.character.ReduceCurrentKarma(damage);
        }
        
        #endregion

        #region EXPERIENCE

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
                        Mathf.Round(GetGroup(true)[j].character.NedExp * 
                                    //The max increment of experience is x2 if the enemy it's 20 level upper.
                                    (1 + Convert.ToSingle(
                                        GetGroup(true)[j].character.Level 
                                        - SavesFiles.GetParty()[i].Level)/20)));
                    // The min is 1.
                    exp = Mathf.Min(exp, GetGroup(true)[j].character.NedExp*2);
                    exp = Mathf.Max(1, exp);
                    
                    experienceGained[SavesFiles.GetParty()[i].ID,j] = exp;
                }
            }
            
            _experienceGained = experienceGained;
        }

        private void AddExperience(params int[] ids)
        {
            if(ids.Length == 0) return;
            int i = 0;
            
            foreach (Character character in SavesFiles.GetParty())
            {
                int exp = 0;
                //Check if the character is in the battlefield. If not his experience is 80%.
                float played = i < maxMembers ? 1 : 0.8f;

                foreach (var id in ids)
                {
                    exp += Convert.ToInt32(Mathf.Max(1, Mathf.Floor(
                        _experienceGained[character.ID, id - maxMembers]*played)));
                }

                character.GainExperience(exp);
                
                //Print the exp. gained upper the character.
                if (i<maxMembers) _animatedText.GainExp(exp, _fighters[i]);
                i++;
            }
        }

        #endregion

        #region TESTS

        [ContextMenu("Set Up")]
        public void TestSetUp()
        {
            TestDelete();
            TestInit1();
            TestLoad();
            TestParty();
        }
        
        [ContextMenu("Init Party1")]
        public void TestInit1()
        {
            SavesFiles.GetSave().AddCharacter(3, 0, 2, 1);
            SavesFiles.SaveData();
        }
        
        [ContextMenu("Init Party2")]
        public void TestInit2()
        {
            SavesFiles.GetSave().AddCharacter(1);
            SavesFiles.SaveData();
        }
        
        [ContextMenu("Load data")]
        public void TestLoad()
        {
            
            SavesFiles.LoadData();
        }
        
        [ContextMenu("Delete")]
        public void TestDelete()
        {
            SavesFiles.Init();
        }
        
        [ContextMenu("Print current party")]
        public void TestParty()
        {
            foreach (Character cha in SavesFiles.GetSave().Party)
            {
                Debug.Log(cha);
            }
        }
        
        #endregion
        
    }
}