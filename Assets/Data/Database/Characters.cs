using System;
using System.Linq;
using Entities;
using UnityEngine;

namespace Data.Database
{
    /**<summary>The Character Database system.</summary>*/
    [CreateAssetMenu(menuName = "Database/Characters", fileName = "CharacterDB")]
    public class Characters : ScriptableObject
    {
        
        /**<summary>Array that save the Serialize data.</summary>*/
        [SerializeField] private Character[] characters;

        /**<summary>Clone.</summary>*/ 
        public void Clone(Characters database)
        {
            this.characters = new Character[database.Count];
            for (int i = 0; i < database.Count; i++)
            {
                this.characters[i] = new Character(database.characters[i])
                {  
                    //Clone the abilities too.
                    SpecialAbilities = database.characters[i].SpecialAbilities
                };
            }
        }
        
        #region GET
        
        /**<summary>Get the number of items.</summary>*/ 
        public int Count => characters.Length;
        
        /**<summary>Get All data in the array.</summary>*/ 
        public Character[] All => characters;
     
        /**<summary>Get All Names of the items of the array.</summary>*/ 
        public string[] Names 
        { 
            get 
            { 
                string[] res = new string[characters.Length]; 
                for (int i = 0; i < characters.Length; i++) { res[i] = characters[i].Name; } 
                return res;
            }
        }
        
        #endregion
        
        #region FINDBY
        
        /**<summary>Find item by ID.</summary>*/ 
        public Character FindByID(int id) 
        { 
            return characters.FirstOrDefault(element => element.ID == id);
        }
        
        /**<summary>Find item by name.</summary>*/ 
        public Character FindByName(string nameOf) 
        { 
            return characters.FirstOrDefault(element => element.Name == nameOf);
        }
        
        #endregion
        
        #region MANAGE
        
        /**<summary>Add a item to the array.</summary>*/ 
        public void Add(Character character) 
        { 
            Array.Resize(ref characters, Count + 1); 
            characters[Count - 1] = character;
        }
        
        /**<summary>Modify a item of the array. It's modify by the item ID.</summary>*/ 
        public void Modify(Character character) 
        { 
            characters[Array.IndexOf(characters, FindByID(character.ID))] = character;
        }
        
        /**<summary>Remove a item of the array</summary>*/ 
        public void Remove(int id) 
        { 
            for (int i = Array.IndexOf(characters, FindByID(id)); i < Count-1; i++) 
            { 
                characters[i] = characters[i+1]; 
                characters[i].ID--; 
            } 
            Array.Resize(ref characters, Count - 1); 
        }
        
        #endregion
        
    }
}