using System;
using Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities
{
    [Serializable]
    public abstract class Stats: Profile
    {
        
        #region ATTRIBUTES
        
        [SerializeField] protected int level  = 1; 
        [SerializeField] protected int maxLevel = 99; 
        [SerializeField] protected int[] main = new int[9]; 
        [SerializeField] protected float[] special = new float[4]; 
        [SerializeField] protected int[] expData = {30, 20, 30, 20}; 
        [SerializeField] protected int elementID; 
        [SerializeField] protected int actExp; 
        [SerializeField] protected int nedExp;

        /**<summary>
        Next
        <para>This are incrementing until become a 100, when it´s being 100, the character can move, later return 0</para>
        <para>You can move when it´s 100%</para>
        <para>Formula: (agi*numberOfMembers)/kg, where number of members = enemies + characters</para>
        </summary>*/
        public float Charge { get; private set; }
        
        #endregion
        
        #region CONSTRUCTORS
        
        /**<summary>Empty Stats constructor</summary>*/ 
        protected Stats(int id): base(id){ }
        
        /**<summary>Clone Stats constructor</summary>*/ 
        protected Stats(Stats bases): base(bases) 
        { 
            level = bases.level; 
            maxLevel = bases.maxLevel; 
            for (int i =0; i<bases.Main.Length; i++) 
            { 
                main[i] = bases.main[i];
            }
            
            for (int i = 0; i < bases.special.Length; i++) 
            {
                special[i] = bases.special[0];
            }
            
            elementID = bases.elementID;
            
            for (int i = 0; i < bases.expData.Length; i++) 
            { 
                expData[i] = bases.expData[i];
            }
            
            actExp = bases.actExp; 
            nedExp = bases.nedExp;
            
        }
        
        #endregion
        
        #region GETTERS & SETTERS
        
        /**<summary>The level of the character.</summary>*/ 
        public int Level 
        { 
            get => level; 
            set 
            { 
                level = level < MaxLevel ? value : MaxLevel; 
                level = level > 0 ? value : 1;
            }
        }
        
        /**<summary>The max level that the character can be.</summary>*/ 
        public int MaxLevel 
        { 
            get => maxLevel; 
            set 
            { 
                maxLevel = value > 0 ? value : 1; 
                level = level > maxLevel ? maxLevel : level;
            }
        }
        
        /**<summary><para>[mbp, mkp, atk, def, spi, men, agi, cbp, ckp]</para>
        <para>0 -> mbp = Max blood points</para>
        1 -> mkp = Max karma points
        <para>2 -> atk = Attack</para>
        3 -> def = Defense
        <para>4 -> spi = Spirit</para>
        5 -> men = Mentality
        <para>6 -> agi = Agility</para>
        7 -> cbp = Current blood points
        <para>8 -> ckp = Current karma points</para>
        </summary>*/ 
        public int[] Main => main;
        
        /**<summary><para>[reb, rek, rxb, rxk, kg]</para>
        <para>0 -> reb = Recovery blood effect</para>
        1 -> rek = Recovery karma effect
        <para>2 -> rxb = Regenerate blood per turn</para>
        3 -> rxk = Regenerate karma per turn
        </summary>*/ 
        public float[] Special => special;
        
        /**<summary>The experience curve for level up.</summary>*/ 
        public int[] ExpData => expData;
        
        /**<summary>The ID of the element of the Character.</summary>*/ 
        public int ElementID { set => elementID = value; }
        
        /**<summary>The exp that you have.</summary>*/ 
        public int ActExp => actExp;
        
        /**<summary>The exp that you need.</summary>*/ 
        public int NedExp => nedExp;
        
        /**<summary>
        Max. Blood point
        <para>Max Life points that you can get.</para>
        <para>Formula: (base + plus) * rate + flat</para>
        </summary>*/ 
        public int MaxBloodPoints 
        { 
            get => Main[0]; 
            set => Main[0] = value;
        }
        
        /**<summary>
        Max. Karma point
        <para>Max of magic points that you can get.</para>
        <para>Formula: (base + plus) * rate + flat</para>
        </summary>*/ 
        public int MaxKarmaPoints 
        { 
            get => Main[1]; 
            set => Main[1] = value;
        }
        
        /**<summary>
        Current Blood point
        <para>When your BP is 0 you are going to be unconscious.</para>
        <para>less than mhp</para>
        </summary>*/ 
        public int CurrentBloodPoints 
        { 
            get => Main[7]; 
            set => Main[7] = value;
        }
        
        /**<summary>
        Actual Karma Points
        <para>When your KP is less than a ability, you cannot use this ability.</para>
        <para>less than mkp</para>
        </summary>*/ 
        public int CurrentKarmaPoints 
        { 
            get => Main[8]; 
            set => Main[8] = value;
        }

        /**<summary>
        Attack
        <para>Physical damage that you can do.</para>
        <para>Formula: (base + plus) * rate + flat</para>
        </summary>*/ 
        public int Attack 
        { 
            get => Main[2]; 
            set => Main[2] = value;
        }
        
        /**<summary>
        Defense
        <para>Physical resistance that you can endure.</para>
        <para>Formula: (base + plus) * rate + flat</para>
        </summary>*/ 
        public int Defense { 
            get => Main[3]; 
            set => Main[3] = value; 
        }
        
        /**<summary>
        Spirit
        <para>Karmic damage that you can do.</para>
        <para>Formula: (base + plus) * rate + flat</para>
        </summary>*/ 
        public int Spirit 
        { 
            get => Main[4]; 
            set => Main[4] = value;
        }
        
        /**<summary>
        Mentality
        <para>Karmic resistance that you can endure.</para>
        <para>Formula: (base + plus) * rate + flat</para>
        </summary>*/ 
        public int Mentality 
        { 
            get => Main[5]; 
            set => Main[5] = value;
        }
        
        /**<summary>
        Agility
        <para>Time between turns, evasion, etc...</para>
        <para>Formula: (base + plus) * rate + flat</para>
        </summary>*/ 
        public int Agility 
        { 
            get => Main[6];
            set => Main[6] = value;
        }
        
        /**<summary>
        Blood recovery plus
        <para>The char recover this% of blood more.</para>
        <para>Formula: spi/(base*rate)*yes?</para>
        </summary>*/ 
        public float BloodRecoveryPlus 
        { 
            get => Special[0]; 
            set => Special[0] = value;
        }
        
        /**<summary>
        Karma recovery plus
        <para>The char recover this% of karma more.</para>
        <para>Formula: men/(base*rate)*yes?</para>
        </summary>*/ 
        public float KarmaRecoveryPlus 
        { 
            get => Special[1]; 
            set => Special[1] = value;
        }
        
        /**<summary>
        Blood regeneration per turn
        <para>Each turn you recover this% of blood points.</para>
        <para>Formula: spi/(base*rate)*yes?</para>
        </summary>*/ 
        public float Regeneration 
        { 
            get => Special[2]; 
            set => Special[2] = value;
        }
        
        /**<summary>
        Karma regeneration per turn
        <para>Time between turns, evasion, etc...</para>
        <para>Formula: kar/(base*rate)*yes?</para>
        </summary>*/ 
        public float KarmaRegeneration 
        { 
            get => Special[3]; 
            set => Special[3] = value;
        }
        
        /**<summary>The element of the object.</summary>*/ 
        public Element Element => GameData.ElementDB.FindByID(elementID);
        
        /**<summary>Set the experience value curve.</summary>*/ 
        public void SetExperienceCurveParameters(int[] expValues) 
        { 
            for (int i = 0; i < expValues.Length; i++) { expData[i] = expValues[i]; }
        }
        
        #endregion
        
        #region METHODS
        
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

        /**<summary>
        Return if the Character can move.
        </summary>*/ 
        public bool AddCharge(int total) 
        { 
            Charge += Convert.ToSingle(Agility)*Random.Range(0.95f, 1.05f) / total;
            if (Charge >= total) Charge = 0;
            else return false; 
            return true;
        }

        /**<summary>
        <para>ROUND(e[0]*(level - 1)^(0.9+(e[2]/250))*l*(level+1)/(6+l^2)/50/e[3])+(l-1)*e[1])</para>
        <para>Where "l" is the current level</para>
        Where "e" it´s the expData
        </summary>*/ 
        private int MainFormulaExperience() 
        { 
            int nextLevel = Level + 1; 
            return Convert.ToInt32(Mathf.Round(ExpData[0] 
                *Mathf.Pow(nextLevel - 1, 0.9f+Convert.ToSingle(ExpData[2])/250) 
                *nextLevel 
                *(nextLevel+1)/(6+Mathf.Pow(nextLevel, 2)/50/ExpData[3])+(nextLevel-1) 
                *ExpData[1])); 
        }
        
        /**<summary>
        Update the experience from user, If actExp >= nedExp, nedExp is updated, using the current level.
        </summary>*/ 
        public void UpdateExperience() { nedExp = Level < MaxLevel ? MainFormulaExperience() : 0; }

        /**<summary>
        Return true if the character has dead (if current blood is less than 0).
        </summary>*/
        public bool IsKo() { return 0 >= CurrentBloodPoints; }

        /**<summary>Init the charge.</summary>*/
        public void ResetCharge() { Charge = 0; }
        
        #endregion
        
    }
}