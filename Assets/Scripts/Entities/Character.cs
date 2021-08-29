using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Enums;
using UnityEngine;

namespace Entities
{
    /**<summary>The character of the game.</summary>*/ 
    [Serializable]
    public class Character : StatsGenerator
    {
        
        #region ATTRIBUTES
        
        [SerializeField] private Sprite face; 
        [SerializeField] private Mesh model; 
        [SerializeField] private int meleeAttackID; 
        [SerializeField] private int longAttackID; 
        [SerializeField] private int abilityPoints;
        [SerializeField] private StatusOf[] statuses = { };
        //<Ability, int[abilityLevel, neededLevel]>
        [SerializeField] private SpecialAbility[] abilities = {};

        public int RestPoints { get; set; } = 5;

        #endregion
        
        #region CONSTRUCTORS
        
        /**<summary>Empty constructor for character.</summary>*/ 
        public Character(int id): base(id){ }
        
        /**<summary>Clone character constructor.</summary>*/ 
        public Character(Character chara): base(chara) 
        { 
            face = chara.face; 
            model = chara.model;
            meleeAttackID = chara.meleeAttackID; 
            longAttackID = chara.longAttackID;
            
            abilities = new SpecialAbility[chara.abilities.Length];
            for (int i = 0; i < chara.abilities.Length; i++)
            {
                abilities[i] = chara.abilities[i];
            }
            
            statuses = chara.statuses;
            for (int i = 0; i < chara.statuses.Length; i++)
            {
                statuses[i] = chara.statuses[i];
            }
        }
        
        #endregion
        
        #region GETTERS & SETTERS
        
        /**<summary>
        The face that is seeing in the menus.
        </summary>*/ 
        public Sprite Face { get => face; set => face = value; }
        
        /**<summary>
        Characters models.
        </summary>*/ 
        public Mesh Model { get => model; set => model = value; }
            
        /**<summary>
        Melee Attack of the character. 
        </summary>*/ 
        public Ability MeleeAttack => GameData.AbilityDB.FindByID(meleeAttackID);
        
        /**<summary>
        Set the Melee Attack of the character. 
        </summary>*/ 
        public int MeleeAttackID { set => meleeAttackID = value; }
        
        /**<summary>
        Long Attack of the character. 
        </summary>*/ 
        public Ability LongAttack => GameData.AbilityDB.FindByID(longAttackID);
        
        /**<summary>
        Set the Long Attack of the character. 
        </summary>*/ 
        public int LongAttackID { set => longAttackID = value; }
        
        /**<summary>Ability points of the character.</summary>*/ 
        public int AbilityPoints { get => abilityPoints; set => abilityPoints = value; }

        //**<summary>The status that the character has.</summary>*/ 
        //public Status[] Status => status.Select(s => GameData.StatusDB.FindByID(s)).ToArray();
        
        /**<summary>The status that the character has.</summary>*/ 
        public StatusOf[] Statuses => statuses;
        
        /**<summary>
        All abilities with current level and need to unlock. 
        </summary>*/ 
        public SpecialAbility[] AllDataAbilities  => abilities;
        
        /**<summary>
        Get all secondary abilities of the character.
        <param name="all">Get all, inclusive the locked abilities. Default false.</param> 
        </summary>*/ 
        public Ability[] Abilities(bool all = true) 
        { 
            if (all) return abilities.Select(a => a.Ability).ToArray(); 
            return abilities.Where(a => level >= a.NeedLevel)
                .Select(a => a.Ability).ToArray();
        }
        
        /**<summary>
        Get the special ability by ID.
        <param name="abilityID">The id of the ability.</param> 
        </summary>*/ 
        public SpecialAbility GetSpAbility(int abilityID)
        {
            return abilities.First(a => a.Ability.ID == abilityID);
        }
        
        /**<summary>
        Get the ability of special ability by ID.
        <param name="abilityID">The id of the ability.</param> 
        </summary>*/ 
        public Ability GetAbility(int abilityID)
        {
            return abilities.Select(s => s.Ability).First(a => a.ID == abilityID);
        }
        
        /**<summary>
        Get the base ability by ID without level.
        <param name="abilityID">The id of the ability.</param> 
        </summary>*/ 
        public Ability GetBaseAbility(int abilityID) 
        { 
            return abilities.Select(a => a.Ability).First(a => a.ID == abilityID);
        }
        
        /**<summary>
        Get all secondary abilities of the character with the level of the ability.
        <param name="all">Get all, inclusive the locked abilities. Default false.</param> 
        </summary>*/ 
        public Dictionary<Ability, int> AbilitiesAndLevel(bool all = true) 
        { 
            if (all) return abilities.ToDictionary(a => a.Ability, b => b.NeedLevel); 
            return abilities.Where(a => level >= a.NeedLevel)
                .ToDictionary(a => a.Ability, 
                    b => b.Level); 
        }
    
        /*/**<summary>The points that the character has to use in the rest zone.</summary>*/ 
        //public int RestPoints { get => restPoints; set => restPoints = value; }
        
        /**<summary>Add a status to the character.</summary>*/ 
        public void AddStatus(StatusOf status)
        {
            if(statuses.Select(s => s.Status.ID).Contains(status.Status.ID)) return;
            if (statuses.FirstOrDefault(s => s.Status.ID == status.Status.ID)?.Level < status.Level) 
            { 
                statuses[Array.IndexOf(statuses, 
                    statuses.First(s => s.Status.ID == status.Status.ID))] = new StatusOf(status);
                return;
            }
            Array.Resize(ref statuses, statuses.Length + 1); 
            statuses[statuses.Length - 1] = new StatusOf(status);
        }
        
        /**<summary>Remove a status of the Character.</summary>*/ 
        public void RemoveStatus(int statusId)
        {
            if (statuses.All(s => s.Status.ID != statusId)) return;
            for (int i = Array.IndexOf(statuses, 
                statuses.First(s => s.Status.ID == statusId)); i < statuses.Length-1; i++) 
            { 
                statuses[i] = statuses[i+1]; 
            } 
            Array.Resize(ref statuses, statuses.Length - 1); 
        }

        #region Stats with status

        /**<summary>Get the Max Blood Points with the status increments.</summary>*/ 
        public new int MaxBloodPoints => Convert.ToInt32(Mathf.Round(base.MaxBloodPoints * Calculate(0)));

        /**<summary>Get the Max Karma Points with the status increments.</summary>*/ 
        public new int MaxKarmaPoints => Convert.ToInt32(Mathf.Round(base.MaxKarmaPoints * Calculate(1)));
        
        /**<summary>Get the Attack with the status increments.</summary>*/ 
        public new int Attack => Convert.ToInt32(Mathf.Round(base.Attack*Calculate(2)));

        /**<summary>Get the Defense with the status increments.</summary>*/ 
        public new int Defense => Convert.ToInt32(Mathf.Round(base.Defense * Calculate(3)));
        
        /**<summary>Get the Spirit with the status increments.</summary>*/ 
        public new int Spirit => Convert.ToInt32(Mathf.Round(base.Spirit * Calculate(4)));
        
        /**<summary>Get the Mentality with the status increments.</summary>*/ 
        public new int Mentality => Convert.ToInt32(Mathf.Round(base.Mentality*Calculate(5)));
        
        /**<summary>Get the Agility with the status increments.</summary>*/ 
        public new int Agility =>  Convert.ToInt32(Mathf.Round( base.Agility* Calculate(6)));

        /**<summary>Get the Blood Recovery Plus Rate with the status increments.</summary>*/ 
        public new float BloodRecoveryPlus => base.BloodRecoveryPlus + 
                                              Statuses.Sum(s => s.Status.IncrementPower[7]);
        
        /**<summary>Get the Karma Recovery Plus Rate with the status increments.</summary>*/ 
        public new float KarmaRecoveryPlus => base.KarmaRegeneration + 
                                              Statuses.Sum(s => s.Status.IncrementPower[8]);

        /**<summary>Get the Regeneration Rate with the status increments.</summary>*/ 
        public new float Regeneration => base.Regeneration + 
                                         Statuses.Sum(s => s.Status.IncrementPower[9]);
        
        /**<summary>Get the Karma Regeneration Rate with the status increments.</summary>*/
        public new float KarmaRegeneration => base.KarmaRegeneration +
                                              Statuses.Sum(s => s.Status.IncrementPower[10]);


        #endregion
        
        #endregion
        
        #region METHODS

        public float Calculate(int index)
        {
            if (Statuses.Length == 0) return 1;
            return Statuses.Select(s => s.Status.IncrementPower[index])
                .Aggregate((x, y) => x * y);
        }
        
        /**<summary>
        Add a Ability in the special ability section of the character.
        <para>If the ability exits, it only change the max level and ability level.</para>
        <param name="needLevel">The level necessary to learn the ability.</param>
        <param name="abilityLevel">The initial ability level.</param>
        </summary>*/ 
        public void AddAbility(int abilityID, int needLevel = -1, int abilityLevel = -1,
            int aMaxLevel = -1) 
        { 
            if (abilities.Any(a => a.Ability.ID == abilityID)) 
            { 
                foreach (SpecialAbility a in abilities.Where(a => a.Ability.ID == abilityID)) 
                { 
                    a.Level = abilityLevel <= 0 ? a.Level : abilityLevel; 
                    a.NeedLevel = needLevel <= 0 ? a.NeedLevel : needLevel;
                    a.MaxLevel = aMaxLevel <= 0 ? a.MaxLevel : aMaxLevel;
                }
            }
            else
            {
                AddSpAbility(abilityID, abilityLevel <= 0 ? 1 : abilityLevel, 
                    needLevel <= 0 ? 1 : needLevel, aMaxLevel <= 0 ? 1 : aMaxLevel);
            }
        }
        
        /**<summary>
        Add a Special ability with the need level to unlock the ability and the current level of it.
        </summary>*/ 
        private void AddSpAbility(int abilityID, int needLevel, int abilityLevel, int aMaxLevel) 
        { 
            Array.Resize(ref abilities, abilities.Length + 1); 
            abilities[abilities.Length - 1] = new SpecialAbility(abilityID, needLevel, 
                abilityLevel, aMaxLevel);
        }
        
        /**<summary>
        Remove a Ability from the abilities list.
        <param name="abilityID">Ability to remove.</param>
        </summary>*/ 
        public void RemoveAbility(int abilityID) 
        { 
            abilities = abilities.Where(a => a.Ability.ID != abilityID).ToArray(); 
        }
        
        /**<summary>Remove a Ability from the abilities list.</summary>*/ 
        public void RestTurnsToStatuses() 
        {
            foreach (StatusOf status in Statuses)
            {
                if(status.Duration<0) continue;
                status.Duration--;
                if (status.Duration == 0)
                {
                    RemoveStatus(status.Status.ID);
                }
            } 
        }

        /**<summary>Return the rentability of the effects of the status.</summary>*/ 
        public int StatusRentability()
        {
            int result = 0;
            int statusResult = 0;

            foreach (StatusOf status in Statuses)
            {
                statusResult += main.Select((m, i) => m * status.IncrementPowerPlus[i] < m ? -1 : 1).Sum();
                statusResult += special.Select((s, i) => 
                    s + status.IncrementPowerPlus[i + main.Length] < s ? -1 : 1).Sum();
                statusResult += status.Status.Effect == EffectType.None ? 1 : -1;
                statusResult += status.Status.TemporalLevelUp;
                result += statusResult * status.Duration;
            }

            return result;
        }
        
        /**<summary>Set the Character with BP = 0 and quit all status.</summary>*/
        public void SetKo()
        {
            main[7] = 0;
            statuses = new StatusOf[]{};
        }
        
        #endregion
        
    }

}