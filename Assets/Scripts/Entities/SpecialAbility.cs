using System;
using System.Linq;
using Data;
using UnityEngine;

namespace Entities
{
    /**<summary>It's the ability of the character, with the
    need level to unlock and current level of the ability.</summary>*/ 
    [Serializable]
    public class SpecialAbility
    {
        
        #region ATTRIBUTES
        
        [SerializeField] private int abilityID; 
        [SerializeField] private int needLevel; 
        [SerializeField] private int level;
        [SerializeField] private int maxLevel = 10;
        [SerializeField] private int needPointsToLevelUp = 10; 
        
        /**<summary>rate for = {pic, div, uiv}</summary>*/ 
        [SerializeField] protected float[] rate = {1, 1, 1};
        /**<summary>learning for = {pic, div, uiv}</summary>*/ 
        [SerializeField] protected float[] learning = {1, 1, 1};
        [SerializeField] private int[] expData = {10, 5, 15, 10};
        
        #endregion
        
        #region CONSTRUCTORS

        /**<summary>
        Empty constructor.
        </summary>*/ 
        public SpecialAbility(){}
        
        /**<summary>
        Set the ability with the evolution parameters.
        </summary>*/ 
        public SpecialAbility(int abilityID, int needLevel, int level, 
            int maxLevel, float[] rate, float[] learning, int[] expData)
        {
            this.abilityID = abilityID;
            this.needLevel = needLevel;
            this.level = level;
            this.maxLevel = maxLevel;
            for (int i = 0; i <rate.Length; i++) 
            {
                this.rate[i] = rate[i]; 
                this.learning[i] = learning[i];
            }
            for (int i = 0; i < expData.Length; i++) { this.expData[i] = expData[i]; }
            SetExperienceCurveParameters(expData);
            UpdateExperience();
        }
        
        /**<summary>
        Clone constructor.
        </summary>*/ 
        public SpecialAbility(SpecialAbility ability)
        {
            abilityID = ability.abilityID;
            needLevel = ability.needLevel;
            level = ability.level;
            MaxLevel = ability.MaxLevel;
            for (int i = 0; i < rate.Length; i++) 
            {
                rate[i] = ability.rate[i]; 
                learning[i] = ability.learning[i];
            }
            SetExperienceCurveParameters(ability.expData);
            UpdateExperience();
        }

        #endregion

        #region GETTERS & SETTERS
        
        /**<summary>Values for Rates.</summary>*/
        public float[] Rate { get => rate; set => rate = value; }
        /**<summary>Learning values.</summary>*/
        public float[] Learning { get => learning; set => learning = value; }
        /**<summary>Max Level of the ability.</summary>*/
        public int MaxLevel { get => maxLevel; set => maxLevel = value > 0 ? value > 15 ? 15 : value : 1; }
        /**<summary>Need points for to level up the ability.</summary>*/
        public int NeedPointsToLevelUp => needPointsToLevelUp;
        /**<summary>Values for calculate exp.</summary>*/
        public int[] ExpData { get => expData; set => expData = value; }

        /**<summary>
        The ability of the character.
        </summary>*/ 
        public Ability Ability => new Ability(GameData.AbilityDB.FindByID(abilityID));
        
        /**<summary>
        The need level to learn it.
        </summary>*/ 
        public int NeedLevel { get => needLevel; set => needLevel = value; }
        
        /**<summary>
        The ability level.
        </summary>*/ 
        public int Level { get => level; set => level = value < maxLevel ? value : maxLevel; }
        
        //**<summary>
        //Set new ability.
        //</summary>*/ 
        //public int AbilityID { set => abilityID = value; } 
        //public void SetAbility(int abilityID) { ability = new Ability(GameData.AbilityDB.FindByID(abilityID)); }
        
        /**<summary>The max interval of the attack.</summary>*/
        public float UpperInterval => Calculate(2);

        /**<summary>The min interval of the attack.</summary>*/
        public float DownInterval => Calculate(1);

        /**<summary>The power up of the attack (extra power).</summary>*/
        public float PowerIncrement => Calculate(0);
        
        #endregion
        
        #region METHODS
             
        /**<summary>Set all parameters of the item.</summary>*/ 
        public void SetAll(float[] nRate, float[] nLearning, int[] nExp)
        { 
            for (int i = 0; i < 3; i++) 
            { 
                MainSet(i, nRate[i], nLearning[i]);
            } 
            SetExperienceCurveParameters(nExp);
        }

        /**<summary>Obtain the damage value and to do the damage.
        <param name="user">Character that to do the attack.</param>
        <param name="destiny">Character that received the damage.</param></summary>*/ 
        public int Damage(Character user, Character destiny)
        {
            ApplyAllStatusTo(destiny);
            return Ability.Damage(user, destiny, Calculate(0), Calculate(1), Calculate(2));
        }

        /**<summary>Apply or not the Statuses of tha ability by level to the character "destiny".
        <param name="destiny">Character that received the statuses.</param></summary>*/ 
        public void ApplyAllStatusTo(Character destiny)
        {
            Ability.Statuses.Select(s =>
            {
                s.Key.ApplyStatusToCharacter(destiny, s.Value, level);
                return s;
            });
        }
        
        /**<summary>Update All parameters of ability, using the current level.</summary>*/ 
        public void UpdateLv(Character character) 
        {
            if(!CanLevelUp(character)) return;
            level++;
            UpdateExperience();
        }
            
        /**<summary>Get the final value of parameter of "index". </summary>*/ 
        private float Calculate(int index) 
        {
            return Ability.Stats[index]*Mathf.Pow(rate[index],level)*LearningRate(index);
        }

        /**<summary>Get the learning rate.</summary>*/
        private float LearningRate(int index)
        { 
            if (level <= 2) return 1; 
            return learning[index] + (1 - learning[index]) * 
                Mathf.Pow(level-1 - Convert.ToSingle((MaxLevel-1) / 2), 2) / 
                Mathf.Pow(Convert.ToSingle((MaxLevel-1) / 2), 2);
        }
        
        /**<summary>
        Set the values of normal Main
        <param name="index">The index of stat. Example 0 = mbp</param>
        <param name="rateValue">The rate Value of the stat</param>
        <param name="learningValue">The exponent Value of the stat</param>
        </summary>*/ 
        private void MainSet(int index, float rateValue, float learningValue) 
        { 
            rate[index] = rateValue; 
            learning[index] = learningValue;
        }
        
        /**<summary>Set the values of experience curve</summary>*/ 
        private void MainSet(int value1, int value2, int value3, int value4) 
        { 
            ExpData[0] = value1; 
            ExpData[1] = value2; 
            ExpData[2] = value3; 
            ExpData[3] = value4;
        }

        /**<summary>
        <para>ROUND(e[0]*(level - 1)^(0.9+(e[2]/250))*l*(level+1)/(6+l^2)/50/e[3])+(l-1)*e[1])</para>
        <para>Where "l" is the current level</para>
        Where "e" it´s the expData
        </summary>*/ 
        private int MainFormulaExperience() 
        { 
            int nextLevel = level + 1; 
            return Convert.ToInt32(Mathf.Round(ExpData[0] 
                *Mathf.Pow(nextLevel - 1, 0.9f+Convert.ToSingle(ExpData[2])/250) 
                *nextLevel 
                *(nextLevel+1)/(6+Mathf.Pow(nextLevel, 2)/50/ExpData[3])+(nextLevel-1) 
                *ExpData[1])); 
        }
        
        /**<summary>
        Update the experience of ability, If actExp >= nedExp, nedExp is updated, using the current level.
        </summary>*/ 
        public bool CanLevelUp(Character character) 
        { 
            return (needPointsToLevelUp <= character.AbilityPoints) && (needPointsToLevelUp != 0);
        }
        
        /**<summary>
        Update the experience from user, If actExp >= nedExp, nedExp is updated, using the current level.
        </summary>*/ 
        public void UpdateExperience() { needPointsToLevelUp = level < MaxLevel ? MainFormulaExperience() : 0; }
        
        /**<summary>Set the experience value curve.</summary>*/ 
        public void SetExperienceCurveParameters(int[] expValues) 
        { 
            for (int i = 0; i < expValues.Length; i++) { expData[i] = expValues[i]; } 
        }

        #endregion
        
    }
}