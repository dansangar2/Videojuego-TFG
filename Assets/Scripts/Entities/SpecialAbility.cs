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
        
        #endregion
        
        #region CONSTRUCTORS
        
        /**<summary>
        Set the ability, the need level to Unlock the ability and the initial level constructor.
        </summary>*/ 
        public SpecialAbility(int abilityID, int needLevel, int initialLevel = 1) 
        { 
            this.abilityID = abilityID; 
            this.needLevel = needLevel; 
            level = initialLevel;
        }
        
        #endregion
        
        #region GETTERS & SETTERS
        
        /**<summary>
        The ability of the character.
        </summary>*/ 
        public Ability SpAbility => new Ability(GameData.AbilityDB.FindByID(abilityID));
        
        /**<summary>
        The need level to learn it.
        </summary>*/ 
        public int NeedLevel { get => needLevel; set => needLevel = value; }
        
        /**<summary>
        The ability level.
        </summary>*/ 
        public int Level { get => level; set => level = value; }
        
        /**<summary>
        Set new ability.
        </summary>*/ 
        public int AbilityID { set => abilityID = value; } 
        //public void SetAbility(int abilityID) { ability = new Ability(GameData.AbilityDB.FindByID(abilityID)); }
        
        #endregion
        
    }
}