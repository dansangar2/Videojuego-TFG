

using System;
using System.Linq;
using Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities
{
    [Serializable]
    public class StatusOf
    {
        #region ATTRIBUTES

        [SerializeField] private int statusId;
        [SerializeField] private float[] incrementPowerPlus = new float[14];
        [SerializeField] private int level;

        #endregion
        
        #region CONSTRUCTORS

        /**<summary>
        Empty constructor.
        </summary>*/ 
        public StatusOf(){}
        
        /**<summary>
        Set the status with the evolution parameters.
        </summary>*/ 
        public StatusOf(int statusId, float[] incrementPowerPlus)
        {
            this.statusId = statusId;
            for (int i=0; i < this.incrementPowerPlus.Length; i++) { this.incrementPowerPlus[i] = incrementPowerPlus[i]; }
        }
        
        /**<summary>
        Clone constructor.
        </summary>*/ 
        public StatusOf(StatusOf status)
        {
            statusId = status.statusId;
            for (int i = 0; i < incrementPowerPlus.Length; i++) 
            {
                incrementPowerPlus[i] = status.incrementPowerPlus[i]; 
            }
        }

        #endregion

        #region GETTERS & SETTERS

        /**<summary>It will uses for increments the "IncrementsPower" values by the level of the ability.
        <para>[mbp, mkp, atk, def, spi, men, agi, reb, rek, rxb, rxk, d, tlv, pos]</para>
        <para>0 -> mbp = Max blood points</para>
        1 -> mkp = Max karma points
        <para>2 -> atk = Attack</para>
        3 -> def = Defense
        <para>4 -> spi = Spirit</para>
        5 -> men = Mentality
        <para>6 -> agi = Agility</para>
        7 -> reb = Recovery Blood Plus Rate
        <para>8 -> rek = Recovery Karma Plus Rate</para>
        9 -> rxb = Regenerate Blood Rate
        <para>10 -> rxk = Regenerate Karma Rate</para>
        11 -> d = Duration
        <para>12 -> tlv = Temporal level Up</para>
        13 -> pos = Possibility</summary>*/ 
        public float[] IncrementPowerPlus { get => incrementPowerPlus; set => incrementPowerPlus = value; }

        /**<summary>Get the base status.</summary>*/ 
        public Status Status => GameData.StatusDB.FindByID(statusId);

        public float[] IncrementPower => incrementPowerPlus.Zip(Status.IncrementPower,
            (x, y) => x * y).ToArray();
        
        /**<summary>It increments or decrements the level temporally.</summary>*/ 
        public int TemporalLevelUp => Convert.ToInt32(Mathf.Round(Status.TemporalLevelUp*Calculate(12)));

        /**<summary>The status duration by turns. If it's 0, then it's don't quit by turns.</summary>*/ 
        public int Duration => Convert.ToInt32(Mathf.Round(Status.Duration*Calculate(12)));
        
        /**<summary>The level of the status.</summary>*/ 
        public int Level { get => level; set => level = value; }

        #endregion

        public void ApplyStatusToCharacter(Character character, float possibility, int nLevel = 1)
        {
            if (Random.Range(0f, 1f) > Mathf.Abs(possibility*Calculate(13))) return;
            if (possibility < 0)
            {
                character.RemoveStatus(statusId);
                return;
            }
            StatusOf of = new StatusOf(this) {level = nLevel};
            character.AddStatus(of);
            foreach (Status s in of.Status.StatusToQuit)
            {
                character.RemoveStatus(s.ID);
            }
        }

        public void SetIncrements(float[] nIncrementPowerPlus)
        {
            if(nIncrementPowerPlus != null)
            {
                for (int i = 0; i < incrementPowerPlus.Length; i++)
                {
                    incrementPowerPlus[i] = nIncrementPowerPlus[i];
                }
            }
        }
        
        /**<summary>Get the final value of parameter of "index". </summary>*/ 
        private float Calculate(int index) 
        {
            return Status.IncrementPower[index]*Mathf.Pow(IncrementPowerPlus[index],level);
        }
    }
}