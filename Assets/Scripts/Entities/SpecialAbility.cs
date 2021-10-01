using System;
using Data;
using UnityEngine;

namespace Entities
{
    [Serializable]
    public class SpecialAbility
    {
        
        #region ATTRIBUTES

        [SerializeField] private int abilityID; 
        [SerializeField] private int needLevel; 
        [SerializeField] private int level;
        [SerializeField] private int maxLevel;

        #endregion

        #region CONSTRUCTOR

        /**<summary>Set the special ability with the current level and need level (1 by default).</summary>*/ 
        public SpecialAbility(int abilityID, int level = 1, int needLevel = 1, int maxLevel = 10)
        {
            this.abilityID = abilityID;
            this.needLevel = needLevel;
            this.level = level;
            this.maxLevel = maxLevel;
        }
        
        /**<summary>Clone constructor.</summary>*/ 
        public SpecialAbility(SpecialAbility spAbility)
        {
            abilityID = spAbility.abilityID; 
            needLevel = spAbility.needLevel;
            level = spAbility.level;
            maxLevel = spAbility.maxLevel;
        }

        #endregion

        #region ATTRIBUTES

        /**<summary>The ability of the character with the level of the "Special Ability".</summary>*/ 
        public Ability Ability => new Ability(GameData.AbilityDB.FindByID(abilityID), Level, MaxLevel);
        
        /**<summary>The need level to learn the ability.</summary>*/ 
        public int NeedLevel { get => needLevel; set => needLevel = value; }
        
        /**<summary>The ability level.</summary>*/ 
        public int Level { get => level; set => level = value < maxLevel ? value : maxLevel; }
        
        /**<summary>Max Level of the ability.</summary>*/
        public int MaxLevel { get => maxLevel; set => maxLevel = value > 0 ? value > 15 ? 15 : value : 1; }

        #endregion
        
        #region METHODS

        /**<summary>Update All parameters of ability, using the current level.</summary>*/ 
        public void UpdateLv(Character character, int nLevel = -1)
        {
            if (nLevel==-1) nLevel = level;
            if(!Ability.CanLevelUp(character) || nLevel>=maxLevel) return;
            character.AbilityPoints -= Ability.NeedPointsToLevelUp;
            level = nLevel + 1;
            Ability.UpdateExperience(level, MaxLevel);
        }

        /**<summary>
        Update the experience from user, If actExp >= nedExp, nedExp is updated, using the current level.
        </summary>*/
        public void UpdateExperience()
        {
            Ability.UpdateExperience(Level, MaxLevel);
        }

        #endregion

    }
}