using System;
using UnityEngine;

namespace Entities
{
    /**<summary>It generates a stats of "Stats". It uses a formulas for it.</summary>*/
    [Serializable]
    public class StatsGenerator : Stats
    {
        
        #region ATTRIBUTES

        /**<summary>
        bases for = {mbp, mkp, atk, def, spi, men, agi, rec, rek, rxb, rxk}
        </summary>*/ 
        [SerializeField] protected float[] bases = {50, 30, 6, 5.5f, 5, 5, 5, 5, 5, 5, 5}; 
        /**<summary>
        plus for = {mbp, mkp, atk, def, spi, men, agi, rec, rek, rxb, rxk}
        </summary>*/ 
        [SerializeField] protected int[] plus = {400, 70, 45, 40, 35, 30, 25, 30, 35, 35, 30}; 
        /**<summary>
        rates for = {mbp, mkp, atk, def, spi, men, agi, rec, rek, rxb, rxk}
        </summary>*/ 
        [SerializeField] protected float[] rate = {1, 1, 1, 1, 1, 1, 1, 6, 4, 15, 15};
        /**<summary>
        flats for = {mbp, mkp, atk, def, spi, men, agi}
        </summary>*/ 
        [SerializeField] protected int[] flat = {0, 0, 0, 0, 0, 0, 0};
        /**<summary>
        learning for = {mbp, mkp, atk, def, spi, men, agi, rec, rek, rxb, rxk}
        </summary>*/ 
        [SerializeField] protected float[] learning = {1, 1, 1, 1, 1, 1, 1};
        /**<summary>
        max for = {rec, rek}
        </summary>*/ 
        [SerializeField] protected float[] max = {0.5f, 0.5f}; 
        /**<summary>
        yes for = {rxb, rxk}
        </summary>*/ 
        [SerializeField] protected bool[] yes = {false, false};
    
        #endregion
        
        #region CONSTRUCTORS
        
        /**<summary>Empty StatsGenerator constructor</summary>*/ 
        protected StatsGenerator(int id): base(id){ }
        
        /**<summary>StatsGenerator clone constructor.</summary>*/ 
        public StatsGenerator(StatsGenerator stats) : base(stats) 
        { 
            for (int i = 0; i < stats.bases.Length; i++) { bases[i] = stats.bases[i]; }
            
            for (int i = 0; i < stats.plus.Length; i++) { plus[i] = stats.plus[i]; }
            
            for (int i = 0; i < stats.rate.Length; i++) { rate[i] = stats.rate[i]; }
            
            for (int i = 0; i < stats.learning.Length; i++) { learning[i] = stats.learning[i]; }
            
            for (int i = 0; i < stats.flat.Length; i++) { flat[i] = stats.flat[i]; }
            
            for (int i = 0; i < stats.max.Length; i++) { max[i] = stats.max[i]; }
            
            for (int i = 0; i < stats.yes.Length; i++) { yes[i] = stats.yes[i]; }
            
            Update();
        }
        
        #endregion
        
        #region GETTERS & SETTERS
        
        /**<summary>Get the bases parameters.</summary>*/ 
        public float[] Bases => bases;
        
        /**<summary>Get the pluses parameters.</summary>*/ 
        public int[] Plus => plus;
        
        /**<summary>Get the rates parameters.</summary>*/ 
        public float[] Rate => rate;
        
        /**<summary>Get the learning rates parameters.</summary>*/ 
        public float[] Learning => learning;
        
        /**<summary>Get the flats parameters.</summary>*/ 
        public int[] Flat => flat;
        
        /**<summary>Get the maximum parameters.</summary>*/ 
        public float[] Max => max;
        
        /**<summary>Get if the regeneration values is activate.</summary>*/ 
        public bool[] Yes => yes;

        /**<summary>Set all parameters of the item.</summary>*/ 
        public void SetAll(float[] nBase, int[] nPlus, float[] nRate, int[] nFlat, float[] nLearning, float[] nMax, bool[] nYes, int[] nExp) 
        { 
            for (int i = 0; i < flat.Length; i++) 
            { 
                MainSet(i, nBase[i], nPlus[i], nRate[i], nFlat[i], nLearning[i]);
            } 
            for (int i = 0; i < max.Length; i++) 
            { 
                MainSet(i, nBase[i+7], nPlus[i+7], nRate[i+7], nMax[i]);
            } 
            for (int i = 0; i < yes.Length; i++) 
            {
                MainSet(i, nBase[i+9], nPlus[i+9], nRate[i+9], nYes[i]);
            }
            SetExperienceCurveParameters(nExp); 
            Update();
            main[7] = main[0]; 
            main[8] = main[1];
        }

        #endregion
        
        #region METHODS
        
        #region Public
        
        /**<summary>Update the Max Blood Point from user, using the current level.</summary>*/ 
        public void UpdateMaxBloodPoints() { Main[0] = Calculate(0); } 
        /**<summary>Update the Max Karma Point from user, using the current level.</summary>*/ 
        public void UpdateMaxKarmaPoints() { Main[1] = Calculate(1); } 
        /**<summary>Update the Attack from user, using the current level.</summary>*/
        public void UpdateAttack() { Main[2] = Calculate(2); } 
        /**<summary>Update the Defense from user, using the current level.</summary>*/ 
        public void UpdateDefense() { Main[3] = Calculate(3); }
        /**<summary>Update the Spirit from user, using the current level.</summary>*/
        public void UpdateSpirit() { Main[4] = Calculate(4); } 
        /**<summary>Update the Mentality from user, using the current level.</summary>*/ 
        public void UpdateMentality() { Main[5] = Calculate(5); }
        /**<summary>Update the Agility from user, using the current level.</summary>*/ 
        public void UpdateAgility() { Main[6] = Calculate(6); }
        /**<summary>Update the Recovery Rate from user, using the current level.</summary>*/ 
        public void UpdateRecoveryBlood() { Special[0] = MainFormulaForRecoveryStats(0); } 
        /**<summary>Update the Recovery Karma Rate from user, using the current level.</summary>*/ 
        public void UpdateRecoveryKarma() { Special[1] = MainFormulaForRecoveryStats(1); }
        /**<summary>Update the Regenerate Blood Rate from user, using the current level.</summary>*/ 
        public void UpdateRegenerateBlood() { Special[2] = MainFormulaForRegenerateStats(0); }
        /**<summary>Update the Regenerate Karma Rate from user, using the current level.</summary>*/ 
        public void UpdateRegenerateKarma() { Special[3] = MainFormulaForRegenerateStats(1); }
    
        /**<summary>Update All parameters from user, using the current level.
        <param name="updateExp">Uses for update the need exp or not</param></summary>*/
        public void Update(bool updateExp = true) 
        { 
            for (int i = 0; i < main.Length-2; i++) 
            { 
                Main[i] = Calculate(i);
            }
            
            for (int i = 0; i < special.Length/2; i++) 
            { 
                Special[i] = MainFormulaForRecoveryStats(i); 
                Special[i+2] = MainFormulaForRegenerateStats(i);
            } 
            if(!updateExp) return;
            UpdateExperience();
        }
        
        /**<summary>
        Update the experience from user, If actExp >= nedExp, nedExp is updated, using the current level.
        </summary>*/ 
        public void GainExperience(int experience) 
        { 
            if(!IsKo()) return;
            
            if ((actExp += experience) < 999999999) actExp += experience;
            else actExp = 999999999; 
            if (ActExp < NedExp ) return; 
            Leveling();
        }
        
        /**<summary>Increment in 1 the level and update stats.</summary>*/ 
        public void Leveling() 
        { 
            if (MaxLevel <= level) return; 
            level++; 
            Update();
        }
        
        #endregion
        
        #region Internal
        
        /**<summary>
        <para>FLOOR((l*bases + plus)*rate + flat)</para>
        <para>Where "l" is the current level</para>
        <param name="index">The index of stat. Example 0 = mbp</param>
        </summary>*/ 
        private int MainFormulaForNormalStats(int index) 
        { 
            return Convert.ToInt32(Mathf.Floor((level* bases[index] + plus[index])*rate[index]) + flat[index]);
        }
        
        /**<summary>
        MIN((spi||men)/(base*rate), Max/100))
        <param name="index">0 => Blood, 1 => Karma</param>
        </summary>*/ 
        private float MainFormulaForRecoveryStats(int index) 
        { 
            float i; 
            if (index == 0) i = Main[5] / ((MaxLevel*bases[7] + plus[7]) * rate[7]);
            else i = Main[4] / ((MaxLevel*bases[8] + plus[8]) * rate[8]); 
            return Mathf.Min(Mathf.Round(i*1000f)/1000f, max[index]);
        }
        
        /**<summary>
        ((spi||men)/(base*rate))*yes
        <param name="index">0 => Blood, 1 => Karma</param>
        </summary>*/ 
        private float MainFormulaForRegenerateStats(int index) 
        { 
            float i; 
            if (index == 0) i = Main[5] / ((MaxLevel*bases[9] + plus[9]) * rate[9]);
            else i = Main[4] / ((MaxLevel*bases[10] + plus[10]) * rate[10]); 
            return Mathf.Round(i*Convert.ToInt32(yes[index])*1000f)/1000f;
        }
        
        /**<summary>
        Set the values of normal Main
        <param name="index">The index of stat. Example 0 = mbp</param>
        <param name="baseValue">The base Value of the stat</param>
        <param name="plusValue">The plus Value of the stat</param>
        <param name="rateValue">The rate Value of the stat</param>
        <param name="flatValue">The flat Value of the stat</param>
        <param name="learningValue">The exponent Value of the stat</param>
        </summary>*/ 
        private void MainSet(int index, float baseValue, int plusValue, float rateValue, int flatValue, float learningValue) 
        { 
            bases[index] = baseValue; 
            plus[index] = plusValue; 
            rate[index] = rateValue; 
            flat[index] = flatValue; 
            learning[index] = learningValue;
        }
        
        /**<summary>
        Set the values of recovery Main
        <param name="index">0 => Blood, 1 => Karma</param>
        <param name="baseValue">The base Value of the stat</param>
        <param name="plusValue">The plus Value of the stat</param>
        <param name="rateValue">The rate Value of the stat</param>
        <param name="maxValue">The max value that it can get</param>
        </summary>*/ 
        private void MainSet(int index, float baseValue, int plusValue, float rateValue, float maxValue) 
        {
            bases[index + 7] = baseValue;
            plus[index + 7] = plusValue; 
            rate[index + 7] = rateValue; 
            max[index] = maxValue;
        }
        
        /**<summary>
        Set the values of regeneration Main
        <param name="index">0 => Blood, 1 => Karma</param>
        <param name="baseValue">The base Value of the stat</param>
        <param name="plusValue">The plus Value of the stat</param>
        <param name="rateValue">The rate Value of the stat</param>
        <param name="yesValue">If get regeneration</param>
        </summary>*/ 
        private void MainSet(int index, float baseValue, int plusValue, float rateValue, bool yesValue) 
        { 
            bases[index + 9] = baseValue; 
            plus[index + 9] = plusValue; 
            rate[index + 9] = rateValue; 
            yes[index] = yesValue;
        }
        
        /**<summary>
        Set the values of experience curve.
        </summary>*/ 
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
        Where "e" it´s the expData.
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
        <para>((level*50 + 400 + plus)*rate + flat), Interval(99, level, pow))</para>
        <para>Where "l" is the current level</para>
        Where "e" it´s the expData.
        </summary>*/ 
        private float LearningRate(int index){ 
            //decimal p = (1 - (decimal)learning[index])/(decimal) Mathf.Pow(Convert.ToSingle(maxLevel/2), 2);
            //return (decimal)learning[index] + p * (decimal) Mathf.Pow(level - Convert.ToSingle(maxLevel/2), 2);
            if (level <= 2) return 1; 
            return learning[index] + (1 - learning[index]) * 
                Mathf.Pow((level-1) - Convert.ToSingle((MaxLevel-1) / 2), 2) / 
                Mathf.Pow(Convert.ToSingle((MaxLevel-1) / 2), 2);
        }
        
        /**<summary>
        Get the final value of parameter of "index". 
        </summary>*/ 
        private int Calculate(int index) 
        {
            
            int res = Convert.ToInt32(MainFormulaForNormalStats(index)*LearningRate(index));
            
            if (res > 99999 && (index == 0 || index == 1)) res = 99999;
            else if (res > 999 && index > 1) res = 999;
            else if (res < 1) res = 1;
            
            return res;
            
        }
        
        #endregion
        
        #endregion
    }
}
