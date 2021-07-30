using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

namespace Entities
{
    [Serializable]
    public class Character : StatsGenerator
    {
        
        #region ATTRIBUTES
        
        [SerializeField] private Sprite face;
        [SerializeField] private Mesh model;
        [SerializeField] private int meleeAttackID;
        [SerializeField] private int longAttackID;
        //<Ability, int[abilityLevel, neededLevel]>
        //[SerializeField] private SerializableDictionary<int, int[]> abilities = new SerializableDictionary<int, int[]>();
        [SerializeField] private SpecialAbility[] abilities = {};
        
        #endregion

        #region CONSTRUCTORS

        /**<summary>
        Empty constructor for character.
        </summary>*/
        public Character(int id): base(id){ }
        
        /**<summary>
        Clone character constructor.
        </summary>*/
        public Character(Character chara): base(chara)
        {
            face = chara.face;
            model = chara.model;
            abilities = chara.abilities;
            meleeAttackID = chara.meleeAttackID;
            longAttackID = chara.longAttackID;

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
        public int MeleeAttackID { set => elementID = value; }
        
        /**<summary>
        Long Attack of the character. 
        </summary>*/
        public Ability LongAttack => GameData.AbilityDB.FindByID(longAttackID);
        
        /**<summary>
        Set the Long Attack of the character. 
        </summary>*/
        public int LongAttackID { set => longAttackID = value; }

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
            if (all) return abilities.Select(a => a.SpAbility).ToArray();
            return abilities.Where(a => level >= a.NeedLevel)
                .Select(a => a.SpAbility).ToArray();
        }

        /**<summary>
        Get all secondary abilities of the character with the level of the ability.
        <param name="all">Get all, inclusive the locked abilities. Default false.</param> 
        </summary>*/
        public Dictionary<Ability, int> AbilitiesAndLevel(bool all = true)
        {
            if (all) return abilities.ToDictionary(a => a.SpAbility, b => b.NeedLevel);
            return abilities.Where(a => level >= a.NeedLevel)
                .ToDictionary(a => a.SpAbility, 
                    b => b.Level);
        }

        #endregion

        #region METHODS

        /**<summary>
        Add a Ability in the special ability section of the character.
        <para>If the ability exits, it only change the max level and ability level.</para>
        <param name="needLevel">The level necessary to learn the ability.</param>
        <param name="abilityLevel">The initial ability level.</param>
        </summary>*/
        public void AddAbility(int abilityID, int needLevel = -1, int abilityLevel = -1)
        {
            if (abilities.Any(a => a.SpAbility.ID == abilityID))
            {
                foreach (SpecialAbility a in abilities.Where(a => a.SpAbility.ID == abilityID))
                {
                    a.Level = abilityLevel <= 0 ? a.Level : abilityLevel;
                    a.NeedLevel = needLevel <= 0 ? a.NeedLevel : needLevel;
                }
            }
            else AddSpAbility(abilityID, abilityLevel <= 0 ? 1 : abilityLevel, needLevel <= 0 ? 1 : needLevel);
        }

        private void AddSpAbility(int abilityID, int needLevel, int abilityLevel)
        {
            Array.Resize(ref abilities, abilities.Length + 1); 
            abilities[abilities.Length - 1] = new SpecialAbility(abilityID, needLevel, abilityLevel);
        }

        /**<summary>
        Remove a Ability from the abilities list.
        <param name="abilityID">Ability to remove.</param>
        </summary>*/
        public void RemoveAbility(int abilityID)
        {
            abilities = abilities.Where(a => a.SpAbility.ID != abilityID).ToArray();
        }

        #endregion
        
    }

}