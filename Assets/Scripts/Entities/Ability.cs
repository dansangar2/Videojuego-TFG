using System;
using System.Data;
using Enums;
using UnityEngine;

namespace Entities
{
    [Serializable]
    public class Ability : Base//StatsGenerator
    {
        
        #region ATTRIBUTES

        [SerializeField] private string formula = "";
        
        [SerializeField] private AbilityType type = AbilityType.Normal;

        [SerializeField] private Sprite icon;

        [SerializeField] private int hits = 1;

        [SerializeField] private TargetRange range = TargetRange.One; 
        [SerializeField] private int numberOfTarget = 1;

        [SerializeField] private TargetType target = TargetType.Enemy;

        /**<summary><para>[mbp, mkp, atk, def, spi, men, agi, abp, akp, kg, cha, nxt] (For target)</para>
         <para>0 -> mbp = Max blood points</para>
         1 -> mkp = Max karma points
         <para>2 -> atk = Attack</para>
         3 -> def = Defense
         <para>4 -> spi = Spirit</para>
         5 -> men = Mentality
         <para>6 -> agi = Agility</para>
         7 -> abp = Actual blood points
         <para>8 -> akp = Actual karma points</para>
         9 -> kg = Weight
         <para>10 -> cha = Charge</para>
         11 -> nxt = Next turn
         </summary>*/
        [SerializeField] private int[] stats2 = {0, 0, 0, -2, 0, 0, 0, 0, 0, 0, 0 ,0};

        /**<summary>
         <para>0 -> improvement by level: user_damage_base*(1 + special[0]*level)</para>
         1 -> improvement interval down: (interval_down_base + special[1]*level)
         <para>2 -> improvement interval down: (interval_up_base + special[2]*level)</para>
         </summary>*/
        [SerializeField] private int baseAttack = 50;
        
        [SerializeField] private float[] interval = {0.8f, 1.2f};

        [SerializeField] private bool useCharacterElement;

        [SerializeField] private int cost;

        [SerializeField] private bool canRepeatRandomTarget;

        #endregion

        #region CONSTRUCTORS

        /**<summary>
         Empty ability constructor
         </summary>
        */
        public Ability(int id): base(id)
        {
            //main = new []{0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0 ,0}; 
            //special = new []{0.2f, 0.05f, 0.1f};
            //expData = new[] {10, 5, 15, 10};
        }

        /**<summary>
         Ability clone constructor
         </summary>
        */
        public Ability(Ability ability): base(ability)
        {
            hits = ability.hits;
            type = ability.type;
            icon = ability.icon;
            //special = ability.special;
            interval = ability.interval;
            useCharacterElement = ability.useCharacterElement;
        }

        #endregion

        #region GETTERS & SETTERS

        public string Formula { get => formula; set => formula = value; } 

        #endregion

        #region METHODS

        /**<summary>
        Obtain the damage that does the user and received the target.
        </summary>*/
        public int Damage(Character user, Character destiny)
        {
            string damage = formula;
            string[] cTarget = {"a", "b"};
            string[] stats = {"mbp", "mkp", "atk", "def", "spi", "men", "agi", "cbp", 
                "ckp", "kg", "reb", "rek", "rxb", "rxk"};
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < user.Main.Length; i++)
                {
                    damage = damage.Replace("{" + cTarget[i] + "." + stats[j] + "}",
                        user.Main[j].ToString());
                }
                for (int j = 0; j < user.Special.Length; i++)
                {
                    damage = damage.Replace("{" + cTarget[i] + "." + stats[j + user.Main.Length] + "}",
                        user.Special[j].ToString());
                }
            }

            Debug.Log(damage);
            
            DataTable dt = new DataTable();
            return (int)dt.Compute(damage,"");
        }

        #endregion
        
    }
}