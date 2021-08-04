using System;
using System.Linq;
using Data.Database;
using Entities;
using UnityEngine;

namespace Core.Saves
{
    [Serializable]
    public class Save
    {

        /**<summary>The Character status in this save file.</summary>*/
        [SerializeField] private Characters characters;

        /**<summary>The current party of the player.</summary>*/
        [SerializeField] private int[] charactersInParty;

        /**<summary>The Money of the player.</summary>*/
        [SerializeField] private int money;

        [SerializeField] private bool isEmpty;
        
        /**<summary>Init the save file.</summary>*/
        public Save()
        {
            IsEmpty = true;
            characters = Data.GameData.CharacterDB;
        }
        
        #region CHARACTERS

        /**<summary>Get all character status of the file.</summary>*/
        public Character[] Characters => characters.All;
        /**<summary>Get all character in the party of the player.</summary>*/
        public Character[] Party => characters.All.Where(c => charactersInParty.Contains(c.ID)).ToArray();

        #region Add

        /**<summary>Add character to the party.</summary>*/
        public void AddCharacter(params int[] charIds)
        {
            foreach (int c in charIds)
            {
                if(charactersInParty.Contains(c)) return;
                Array.Resize(ref charactersInParty, charactersInParty.Length + 1);
                charactersInParty[charactersInParty.Length - 1] = c;
            }
        }
        
        /**<summary>Add character to the party.</summary>*/
        public void AddCharacter(params Character[] charIds)
        {
            foreach (Character c in charIds)
            {
                if(charactersInParty.Contains(c.ID)) return;
                Array.Resize(ref charactersInParty, charactersInParty.Length + 1);
                charactersInParty[charactersInParty.Length - 1] = c.ID;
            }
        }

        #endregion

        #region Remove

        /**<summary>Remove the characters of the party.</summary>*/
        public void RemoveCharacter(params Character[] chars)
        {
            foreach (Character character in chars)
            {
                if (!charactersInParty.Contains(character.ID)) return;
                charactersInParty = charactersInParty
                    .Where(c => c != character.ID).ToArray();
            }
        }
        
        
        /**<summary>Remove the characters of the party.</summary>*/
        public void RemoveCharacter(params int[] charIds)
        {
            foreach (int id in charIds)
            {
                if (!charactersInParty.Contains(id)) return;
                charactersInParty = charactersInParty
                    .Where(c => c != id).ToArray();
            }
        }

        #endregion
        
        #region Switch
        
        /**<summary>Switch two members of the by position.</summary>*/
        public void SwitchCharacters(int character1, int character2)
        {
            if (charactersInParty.Length < Mathf.Max(character1, character2) + 1)
            {
                Console.WriteLine("ALERT! some of these two values is greater than the array size.");
                return;
            }
            int c = charactersInParty[character1];
            charactersInParty[character1] = charactersInParty[character2];
            charactersInParty[character2] = c;
        }

        #endregion
        
        #endregion

        #region GAME DATA

        /**<summary>It mark if the save data is empty.</summary>*/
        public bool IsEmpty { get => isEmpty; set => isEmpty = value; }
        
        /**<summary>It get the money of the player.</summary>*/
        public int Money
        {
            get => money;
            set
            {
                money = value < 0 ? 0:value;
                money = value >= 9999999 ? 9999999 : value;
            }
        }

        #endregion
    }
}
