using System;
using System.Linq;
using Entities;
using UnityEngine;

namespace Data.Database
{
    [CreateAssetMenu(menuName = "Database/Abilities", fileName = "AbilityDB")]
    public class Abilities : ScriptableObject
    {
        
        /**<summary>Array that save the Serialize data.</summary>*/
        [SerializeField] private Ability[] abilities = {};


        #region GET

        /**<summary>Get the number of items.</summary>*/
        public int Count => abilities.Length;

        /**<summary>Get All data in the array.</summary>*/
        public Ability[] All => abilities;

        /**<summary>Get All Names of the items of the array.</summary>*/
        public string[] Names
        {
            get
            {
                string[] res = new string[abilities.Length];
                for (int i = 0; i < abilities.Length; i++) { res[i] = abilities[i].Name; }
                return res;
            }
        }

        #endregion

        #region FINDBY

        /**<summary>Find item by ID.</summary>*/
        public Ability FindByID(int id)
        {
            return abilities.FirstOrDefault(element => element.ID == id);
        }
        
        /**<summary>Find item by name.</summary>*/
        public Ability FindByName(string nameOf)
        {
            return abilities.FirstOrDefault(element => element.Name == nameOf);
        }

        #endregion

        #region MANAGE

        /**<summary>Add a item to the array.</summary>*/
        public void Add(Ability ability)
        {
            Array.Resize(ref abilities, Count + 1); 
            abilities[Count - 1] = ability;
        }

        /**<summary>Modify a item of the array. It's modify by item ID.</summary>*/
        public void Modify(Ability ability)
        {
            abilities[Array.IndexOf(abilities, FindByID(ability.ID))] = ability;
        }

        /**<summary>Remove a item of the array</summary>*/
        public void Remove(int id)
        {
            for (int i = Array.IndexOf(abilities, FindByID(id)); i < Count-1; i++)
            {
                abilities[i] = abilities[i+1];
                abilities[i].ID--;
            }
            Array.Resize(ref abilities, Count - 1);
        }

        #endregion
        
    }
}