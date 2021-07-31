using System;
using System.Data;
using System.Globalization;
using Data;
using Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities
{
    [Serializable]
    public class Ability : Base
    {
        
        #region ATTRIBUTES

        [SerializeField] private string formula = "4*{a.atk} - 2*{a.def} < 0 ? 1 : 4*{a.atk} - 2*{a.def}";
        [SerializeField] private AbilityType type = AbilityType.Normal;
        [SerializeField] private TargetRange range = TargetRange.One;
        [SerializeField] private TargetType target = TargetType.Enemy;
        [SerializeField] private Sprite icon;
        [SerializeField] private int hits = 1;
        [SerializeField] private int numberOfTarget = 1;
        [SerializeField] private int cost = 100;
        [SerializeField] private int elementID;

        //power increment/down interval/upper interval
        [SerializeField] private float[] stats = {1f, 0.8f, 1.2f};
        
        /**<summary>
        bases for = {pic, div, uiv}
        </summary>*/
        [SerializeField] protected float[] bases = {1, 0.8f, 1.2f};
        /**<summary>
        rate for = {pic, div, uiv}
        </summary>*/
        [SerializeField] protected float[] rate = {1, 1, 1};
        /**<summary>
        learning for = {pic, div, uiv}
        </summary>*/
        [SerializeField] protected float[] learning = {1, 1, 1};

        [SerializeField] private int maxLevel = 10;
        [SerializeField] private bool canRepeatRandomTarget;
        [SerializeField] private int actExp;
        [SerializeField] private int nedExp;
        [SerializeField] private int[] expData = {10, 5, 15, 10};

        #endregion

        #region CONSTRUCTORS

        /**<summary>
         Empty ability constructor
         </summary>*/
        public Ability(int id): base(id) { }

        /**<summary>
         Ability clone constructor
         </summary>*/
        public Ability(Ability ability, int level = 1): base(ability)
        {
            formula = ability.formula;
            type = ability.type;
            icon = ability.icon;
            hits = ability.hits;
            range = ability.range;
            numberOfTarget = ability.numberOfTarget;
            target = ability.target;
            maxLevel = ability.maxLevel;
            cost = ability.cost;
            elementID = ability.elementID;
            canRepeatRandomTarget = ability.canRepeatRandomTarget;
            for (int i = 0; i < stats.Length; i++)
            {
                bases[i] = ability.bases[i];
                rate[i] = ability.rate[i];
                learning[i] = ability.learning[i];
            }
            expData = ability.expData;
            Update(level);
        }

        #endregion

        #region GETTERS & SETTERS

        public AbilityType Type { get => type; set => type = value; }
        
        public Sprite Icon { get => icon; set => icon = value; }
        
        public int Hits { get=> hits; set => hits = value > 0? value : 1; }
        
        public TargetRange Range { get => range; set => range = value; }
        
        public int NumberOfTarget { get => numberOfTarget; set => numberOfTarget = value > 0? value : 1; }
        
        public TargetType Target { get => target; set => target = value; }
        
        public float UpperInterval { get => stats[2]; set => stats[2] = value; }
        
        public float DownInterval { get => stats[1]; set => stats[1] = value; }
        
        public float PowerIncrement { get => stats[0]; set => stats[0] = value; }
        
        public int ElementID { set => elementID = value; }
        
        public Element Element => GameData.ElementDB.FindByID(elementID);
        
        public float[] Bases { get => bases; set => bases = value; }
        
        public float[] Rate { get => rate; set => rate = value; }

        public float[] Learning { get => learning; set => learning = value; }
        
        public int MaxLevel { get => maxLevel; set => maxLevel = value > 0 ? value > 15 ? 15 : value : 1; }
        
        public int Cost { get => cost; set => cost = value > 0? value : 1; }
        
        public bool CanRepeatRandomTarget { get => canRepeatRandomTarget; set => canRepeatRandomTarget = value; }
        
        public int NedExp => nedExp;
        
        public int ActExp => actExp;
        
        public int[] ExpData { get => expData; set => expData = value; }
        
        //public float[] GetCurrentInterval(int level) { return new []{downInterval[level], upperInterval[level]}; }
        
        public string Formula { get => formula; set => formula = value; } 

        #endregion

        #region METHODS
        
        /**<summary>
        Set all parameters of the item.
        </summary>*/
        public void SetAll(float[] nBase, float[] nRate, float[] nLearning, int[] nExp, int level)
        {
            for (int i = 0; i < 3; i++)
            {
                MainSet(i, nBase[i], nRate[i], nLearning[i]);
            }
            SetExperienceCurveParameters(nExp);
            Update(level);
        }
        

        /**<summary>
        Update All parameters of ability, using the current level.
        </summary>*/
        public void Update(int level)
        {
            for (int i = 0; i < 3; i++)
            {
                stats[i] = Calculate(i, level);
            }
            UpdateExperience(level);
        }
        
        /**<summary>
        Get the final value of parameter of "index". 
        </summary>*/
        private float Calculate(int index, int level)
        {
            return bases[index]*Mathf.Pow(rate[index],level)*LearningRate(index, level);
        }
        
        /**<summary>
        Obtain the damage that does the user and received the target.
        </summary>*/
        public int Damage(Character user, Character destiny)
        {
            #region Init

            string damage = formula;
            string[] cTarget = {"a", "b"};
            string[] statsOf = {"mbp", "mkp", "atk", "def", "spi", "men", "agi", "cbp", 
                "ckp", "kg", "reb", "rek", "rxb", "rxk"};
            Character[] characters = {user, destiny};

            #endregion

            #region Replace

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < user.Main.Length; j++) 
                {
                    damage = damage.Replace("{" + 
                                            cTarget[i] + "." + 
                                            statsOf[j] + "}", 
                        characters[i].Main[j].ToString());
                    
                    
                }
                for (int j = 0; j < user.Special.Length; j++)
                { 
                    damage = damage.Replace("{" + 
                                            cTarget[i] + "." + 
                                            statsOf[j + user.Main.Length] + "}", 
                        characters[i].Special[j].ToString(CultureInfo.InvariantCulture));
                }
                
                damage = damage.Replace("{" + cTarget[i] + ".lv}", characters[i].Level.ToString());
                damage = damage.Replace("{" + cTarget[i] + ".cha}", characters[i].Charge.ToString(CultureInfo.InvariantCulture));
            }

            #endregion

            DataTable dt = new DataTable();

            #region Split

            string[] booleans = damage.Split(new []{ '?' }, 2);

            while (booleans.Length>1)
            {
                string[] result = booleans[1].Split(new []{ ':' }, 2);
                damage = Convert.ToBoolean(dt.Compute(booleans[0], "")) ? result[0] : result[1];
                booleans = damage.Split(new []{ '?' }, 2);
            }

            #endregion
            
            return Convert.ToInt32(
                Convert.ToInt32(dt.Compute(damage, ""))
                *Random.Range(DownInterval, UpperInterval)
                *PowerIncrement);
        }

        private float LearningRate(int index, int level){
            if (level <= 2) return 1;
            return learning[index] + (1 - learning[index]) * 
                Mathf.Pow(level-1 - Convert.ToSingle((MaxLevel-1) / 2), 2) / 
                Mathf.Pow(Convert.ToSingle((MaxLevel-1) / 2), 2);
        }
        
        /**<summary>
        Set the values of normal Main
        <param name="index">The index of stat. Example 0 = mbp</param>
        <param name="baseValue">The base Value of the stat</param>
        <param name="rateValue">The rate Value of the stat</param>
        <param name="learningValue">The exponent Value of the stat</param>
        </summary>*/
        private void MainSet(int index, float baseValue, float rateValue, float learningValue)
        {
            bases[index] = baseValue;
            rate[index] = rateValue;
            learning[index] = learningValue;
        }
        
        /**<summary>
        Set the values of experience curve
        </summary>*/
        private void MainSet(int value1, int value2, int value3, int value4)
        {
            ExpData[0] = value1;
            ExpData[1] = value2;
            ExpData[2] = value3;
            ExpData[3] = value4;
        }
        
        #endregion
        
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
        public bool GainExperience(int experience)
        {
            if ((actExp += experience) < 999999999) actExp += experience;
            else actExp = 999999999;
            return actExp <= nedExp;
        }
        
        /**<summary>
        Update the experience from user, If actExp >= nedExp, nedExp is updated, using the current level.
        </summary>
        */
        public void UpdateExperience(int level) { nedExp = level < MaxLevel ? MainFormulaExperience(level) : 0; }
        
        /**<summary>Set the experience value curve.</summary>*/
        public void SetExperienceCurveParameters(int[] expValues)
        {
            for (int i = 0; i < expValues.Length; i++) { expData[i] = expValues[i]; }
        }
        
        
        
    }
}