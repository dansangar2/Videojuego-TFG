using System;
using System.Linq;
using Data;
using Enums;
using UnityEngine;

namespace Entities
{
    /**<summary>Status that the character can have. Status are the advantages and disadvantages.</summary>*/
    [Serializable]
    public class Status : Base
    {
        
        #region ATTRIBUTES

        [SerializeField] private Sprite icon;
        [SerializeField] private EffectType effect = EffectType.None;
        [SerializeField] private bool quitWhenFinish;
        [SerializeField] private float quitByHitRate;
        
        [SerializeField] private float[] incrementPower = {1,1,1,1,1,1,1,0,0,0,0};
        [SerializeField] private int[] statusToQuit = {};

        #endregion

        #region CONSTRUCTOR

        /**<summary>Empty Status constructor</summary>*/ 
        public Status(int id): base(id){ }
        
        /**<summary>Clone Status constructor.</summary>*/ 
        public Status(Status status): base(status) 
        { 
            icon = status.icon;
            effect = status.effect;
            quitWhenFinish = status.quitWhenFinish;
            quitByHitRate = status.quitByHitRate;
            
            incrementPower = new float[status.incrementPower.Length];
            for (int i = 0; i < status.incrementPower.Length; i++)
            {
                incrementPower[i] = status.incrementPower[i];
            }

            statusToQuit = new int[status.statusToQuit.Length];
            for (int i=0; i < status.statusToQuit.Length; i++) { statusToQuit[i] = status.statusToQuit[i]; }
        }
        
        /**<summary>Clone Status constructor with level.</summary>*/ 
        public Status(Status status, int level = 1, float[] increments = null): this(status) 
        { 
            increments ??= new float[]{1,1,1,1,1,1,1,0,0,0,0,1,1,1};
            
            for (int i = 0; i < status.incrementPower.Length; i++)
            {
                incrementPower[i] = status.incrementPower[i]*Mathf.Pow(increments[i],level-1);
            }
        }

        #endregion

        #region GETTERS & SETTERS

        /**<summary>Icon of status.</summary>*/ 
        public Sprite Icon { get => icon; set => icon = value; }

        /**<summary>The effect of the status.</summary>*/ 
        public EffectType Effect { get => effect; set => effect = value; }

        /**<summary>It indicates if when the character end the battle the state will dead.</summary>*/ 
        public bool QuitWhenFinish { get => quitWhenFinish; set => quitWhenFinish = value; }

        /**<summary>The rate with the possibility to quit when someone is beat.</summary>*/
        public float QuitByHitRate { get => quitByHitRate; set => quitByHitRate = value < 0 ? 0 : value > 1 ? 1 : value; }

        /**<summary>Increment the stats of the character.
        <para>[mbp, mkp, atk, def, spi, men, agi, reb, rek, rxb, rxk]</para>
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
        </summary>*/ 
        public float[] IncrementPower { get => incrementPower; set => incrementPower = value; }

        /**<summary>When the status is in some character, all this status that the character
        has that are in this array will remove. For to indicate that all statuses must remove, the array
        needs to have a "-1" in.</summary>*/ 
        public Status[] StatusToQuit  { get
            {
                if (statusToQuit.Contains(-1)) return GameData.StatusDB.All.Where(s => s.ID != id).ToArray();
                return statusToQuit.Select(s => GameData.StatusDB.FindByID(s)).ToArray();
            }
        }

        /**<summary>Return if the "all status will quit" is marked.</summary>*/ 
        public bool AllToQuitIsMark => statusToQuit.Contains(-1);

        /**<summary>Add status to quit when the character have this status.</summary>*/ 
        public void AddStatusToQuit(int statusId)
        {
            if(statusToQuit.Contains(statusId) || statusId == ID) return;
            Array.Resize(ref statusToQuit, statusToQuit.Length + 1); 
            statusToQuit[statusToQuit.Length - 1] = statusId;
        }
        
        /**<summary>Remove a status of statusToQuit list.</summary>*/ 
        public void RemoveStatusToQuit(int statusId) 
        {
            if(!statusToQuit.Contains(statusId)) return;
            for (int i = Array.IndexOf(statusToQuit, statusId); i < statusToQuit.Length-1; i++) 
            {
                statusToQuit[i] = statusToQuit[i+1];
            }
            Array.Resize(ref statusToQuit, statusToQuit.Length - 1);
        }
        
        #endregion
        
    }
}