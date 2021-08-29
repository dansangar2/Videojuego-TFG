using System;
using Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities
{
    [Serializable]
    public class StatusOf
    {
        #region ATTRIBUTES

        [SerializeField] private int statusID;
        [SerializeField] private int level;
        [SerializeField] private int duration;
        [SerializeField] private float possibility;
        [SerializeField] protected float[] incrementPowerPlus = new float[14];

        #endregion
        
        #region CONSTRUCTORS

        /**<summary>Empty constructor.</summary>*/ 
        public StatusOf(){}
        
        /**<summary>Set the status with the evolution parameters.</summary>*/ 
        public StatusOf(int statusID, int level, int duration, float possibility, float[] incrementStats)
        {
            for (int i = 0; i < incrementStats.Length; i++)
            {
                incrementPowerPlus[i] = incrementStats[i];
                Debug.Log(incrementPowerPlus[i]);
            }
            this.statusID = statusID;
            this.level = level;
            this.duration = duration;
            this.possibility = possibility;
        }
        
        /**<summary>Clone constructor.</summary>*/ 
        public StatusOf(StatusOf status)
        {
            statusID = status.Status.ID;
            level = status.level;
            duration = status.duration;
            possibility = status.possibility;
            for (int i = 0; i < status.incrementPowerPlus.Length; i++)
            {
                incrementPowerPlus[i] = status.incrementPowerPlus[i];
            }
        }

        #endregion

        #region GETTERS & SETTERS

        /**<summary>Get the base status.</summary>*/ 
        public Status Status => new Status(GameData.StatusDB.FindByID(statusID), level, incrementPowerPlus);
        
        /**<summary>The level of the status.</summary>*/ 
        public int Level { get => level; set => level = value; }

        /**<summary>The level of the status.</summary>*/
        public float Possibility
        {
            get
            {
                possibility = Mathf.Min(1, possibility*Mathf.Pow(IncrementPowerPlus[13],level-1));
                return Mathf.Max(-1, possibility);
            }
            set
            {
                possibility = Mathf.Min(1, value);
                possibility = Mathf.Max(-1, possibility);
            }
        }

        /**<summary>The status duration by turns. If it's "-1", then it's don't quit by turns.</summary>*/ 
        public int Duration
        {
            get => Convert.ToInt32(duration*Mathf.Pow(incrementPowerPlus[11],level-1)) < -1 
                ? -1 : Convert.ToInt32(duration*Mathf.Pow(incrementPowerPlus[11],level-1));
            set => duration = value;
        }

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

        #endregion

        #region METHODS

        /**<summary>Add Status to Character.
        <param name = "character">The character that will add this status.</param></summary>*/
        public void ApplyStatusToCharacter(Character character)
        {
            bool quit = possibility < 0;
            //float p = Mathf.Abs(possibility);
            if (Random.Range(0f, 1f) > Mathf.Abs(possibility)) return;
            if (quit)
            {
                character.RemoveStatus(Status.ID);
                return;
            }character.AddStatus(this);
        }
        
        #endregion
    }
}