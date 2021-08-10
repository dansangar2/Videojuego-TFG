using System;
using Core.Saves;
using Entities;
using UnityEngine;

namespace Core.ButtonsSystem.ButtonData
{
    /**<summary>It uses for set the data of the character HUD & buttons.</summary>*/
    [Serializable]
    public class MemberButtonData
    {

        #region ATTRIBUTES

        [SerializeField] protected int id;
        [SerializeField] protected int characterID;

        #endregion
        
        public MemberButtonData(int id) { this.id = id; }

        public void SetMember(int characterId)
        {
            characterID = characterId;
        }

        public int GetCharacterID() { return characterID; }

        /*/**<summary>The ID of the button.</summary>*/
        //public int ID => id;

        /**<summary>It get the character that the button represents.</summary>*/
        public Character Character => SavesFiles.GetSave().Characters[characterID];

    }
}