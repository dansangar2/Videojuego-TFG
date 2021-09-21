using System;
using UnityEngine;

namespace Entities
{
    /**<summary>It's the ability of the character, with the
    need level to unlock and current level of the ability.</summary>*/ 
    [Serializable]
    public class AbilityStatsGenerator : Base
    {
        
        #region ATTRIBUTES
        
        [SerializeField] private int needPointsToLevelUp;
        /**<summary>rate for = {pic, div, uiv}</summary>*/ 
        [SerializeField] protected float[] rate = {1, 1, 1};
        /**<summary>learning for = {pic, div, uiv}</summary>*/ 
        [SerializeField] protected float[] learning = {1, 1, 1};
        [SerializeField] private int[] expData = {10, 5, 15, 10};
        
        #endregion
        
        #region CONSTRUCTORS

        /**<summary>Empty stats generator ability constructor.</summary>*/ 
        public AbilityStatsGenerator(int id) : base(id){}
        
        /**<summary>
        Set the ability with the evolution parameters.
        </summary>*/ 
        public AbilityStatsGenerator(int abilityID, float[] rate, float[] learning, int[] expData) : base(abilityID)
        {
            for (int i = 0; i <rate.Length; i++) 
            {
                this.rate[i] = rate[i]; 
                this.learning[i] = learning[i];
            }
            for (int i = 0; i < expData.Length; i++) { this.expData[i] = expData[i]; }
            SetExperienceCurveParameters(expData);
        }
        
        /**<summary>Clone constructor.</summary>*/ 
        public AbilityStatsGenerator(AbilityStatsGenerator abilityStatsGenerator) : base(abilityStatsGenerator)
        {
            for (int i = 0; i < rate.Length; i++) 
            {
                rate[i] = abilityStatsGenerator.rate[i]; 
                learning[i] = abilityStatsGenerator.learning[i];
            }
            SetExperienceCurveParameters(abilityStatsGenerator.expData);
        }

        #endregion

        #region GETTERS & SETTERS
        
        /**<summary>Values for Rates.</summary>*/
        public float[] Rate { get => rate; set => rate = value; }
        /**<summary>Learning values.</summary>*/
        public float[] Learning { get => learning; set => learning = value; }
        /**<summary>Need points for to level up the ability.</summary>*/
        public int NeedPointsToLevelUp => needPointsToLevelUp;
        /**<summary>Values for calculate exp.</summary>*/
        public int[] ExpData { get => expData; set => expData = value; }
        
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
        private int MainFormulaExperience(int level) 
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
        public void UpdateExperience(int level, int maxLevel) { needPointsToLevelUp = level < maxLevel ? MainFormulaExperience(level) : 0; }
        
        /**<summary>Set the experience value curve.</summary>*/ 
        public void SetExperienceCurveParameters(int[] expValues) 
        { 
            for (int i = 0; i < expValues.Length; i++) { expData[i] = expValues[i]; } 
        }

        #endregion
        
    }
}