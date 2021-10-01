using System;
using System.Linq;
using Data;
using Data.Database;
using Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Saves
{
    [Serializable]
    public class Save
    {

        /**<summary>The Character status in this save file.</summary>*/
        [SerializeField] private Characters characters = ScriptableObject.CreateInstance<Characters>();

        /**<summary>The Character status in this save file.</summary>*/
        [SerializeField] private Characters enemies = ScriptableObject.CreateInstance<Characters>();
        
        /**<summary>The current party of the player.</summary>*/
        [SerializeField] private int[] charactersInParty = {};

        /**<summary>The Money of the player.</summary>*/
        [SerializeField] private int money;

        /**<summary>The current scene where the character is.</summary>*/
        [SerializeField] private string currentScene;
        
        /**<summary>The level where the player is.</summary>*/
        [SerializeField] private int level = 1;
        
        /**<summary>It marks if it's empty.</summary>*/
        [SerializeField] private bool isEmpty;

        /**<summary>Init the save file.</summary>*/
        public Save()
        {
            characters.Clone(GameData.CharacterDB);
            enemies.Clone(GameData.EnemyDB);
            IsEmpty = true;
        }
        
        #region CHARACTERS

        /**<summary>Get all character status of the file.</summary>*/
        public Character[] Characters => characters.All;
        /**<summary>Get all enemies of the file.</summary>*/
        public Character[] Enemies => enemies.All;
        /**<summary>Get all character in the party of the player.</summary>*/
        public Character[] Party => charactersInParty.Select(c => characters.FindByID(c)).ToArray();// .All.Where(c => charactersInParty.Contains(c.ID)).ToArray();
        /**<summary>Get the character with the ID.</summary>*/
        public Character GetCharacter(int id) => characters.FindByID(id);
        /**<summary>Get the enemy with the ID.</summary>*/
        public Character GetEnemy(int id) => enemies.FindByID(id);
        /**<summary>Get the character with the ID.</summary>*/
        public Ability AbilityOf(int idChar, int idAbi) => characters.FindByID(idChar).GetAbility(idAbi);


        #region Add

        /**<summary>Add character to the party.</summary>*/
        public void AddCharacter(params int[] charIds)
        {
            foreach (int c in charIds)
            {
                if(charactersInParty.Contains(c)) continue;
                Array.Resize(ref charactersInParty, charactersInParty.Length + 1);
                charactersInParty[charactersInParty.Length - 1] = c;
            }
        }
        
        /**<summary>Add character to the party.</summary>*/
        public void AddCharacter(params Character[] charIds)
        {
            foreach (Character c in charIds)
            {
                if(charactersInParty.Contains(c.ID)) continue;
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
                if (!charactersInParty.Contains(character.ID)) continue;
                charactersInParty = charactersInParty
                    .Where(c => c != character.ID).ToArray();
            }
        }
        
        
        /**<summary>Remove the characters of the party.</summary>*/
        public void RemoveCharacter(params int[] charIds)
        {
            foreach (int id in charIds)
            {
                if (!charactersInParty.Contains(id)) continue;
                charactersInParty = charactersInParty
                    .Where(c => c != id).ToArray();
            }
        }

        #endregion
        
        #region Switch
        
        /**<summary>Switch two members of the by character ID.</summary>*/
        public void SwitchCharactersByID(int character1, int character2)
        {
            if(character1==character2) return;
            int i = Array.IndexOf(charactersInParty, character1);
            int j = Array.IndexOf(charactersInParty, character2);
            charactersInParty[i] = character2;
            charactersInParty[j] = character1;
        }
        
        /**<summary>Switch two members of the by position.</summary>*/
        public void SwitchCharactersByPosition(int character1, int character2)
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

        public void MoveCharacter(int character, int position)
        {
            position = position < Party.Length ? position : Party.Length - 1;
            for (int i = Array.IndexOf(charactersInParty, character); i < position; i++)
            {
                charactersInParty[i] = charactersInParty[i + 1];
            }

            charactersInParty[position] = character;

        }

        #endregion
        
        #endregion

        #region GAME DATA

        /**<summary>The level where the player is.</summary>*/
        public int Level { get => level; set => level = value; }

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

        /**<summary>Set the scene where the player is.
        <param name="scene">The scene name where the character is.</param>
        <param name="goThere">Optional param, default true. If it's true, 
        then the player move to the new scene</param>
        </summary> */
        public void SetScene(string scene, bool goThere = true)
        {
            currentScene = scene;
            if (goThere) SceneManager.LoadScene(currentScene);
        }
        
        /**<summary>Go to the scene registered.</summary> */
        public void GoToScene()
        {
            SceneManager.LoadScene(currentScene);
        }

        #endregion
        
    }
}
