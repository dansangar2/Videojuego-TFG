using System;
using System.Linq;
using Entities;
using UnityEngine;

namespace Data.Database
{
    [CreateAssetMenu(menuName = "Database/Profiles", fileName = "ProfileDB")]
    public class Profiles : ScriptableObject
    {
        
        /**<summary>Array that save the Serialize data.</summary>*/
        [SerializeField] private Profile[] profiles = {};
        [SerializeField] private Profile[] profilesCharacter = {};


        #region GET

        /**<summary>Get the number of items.</summary>*/
        public int Count => profiles.Length;

        /**<summary>Get All data in the array.</summary>*/
        public Profile[] All => profiles;
        
        /**<summary>Get All profiles of the character in the array.</summary>*/
        public Profile[] AllCharacters => (Profile[]) GameData.CharacterDB.All.Clone();

        /**<summary>Get All Names of the items of the array.</summary>*/
        public string[] Names
        {
            get
            {
                string[] res = new string[profiles.Length];
                for (int i = 0; i < profiles.Length; i++) { res[i] = profiles[i].Name; }
                return res;
            }
        }

        #endregion

        #region FINDBY

        /**<summary>Find item by ID.</summary>*/
        public Profile FindByID(int id)
        {
            if(id<0) return profilesCharacter.FirstOrDefault(profile => profile.ID == -id);
            return profiles.FirstOrDefault(profile => profile.ID == id);
        }
        
        /**<summary>Find item by name.</summary>*/
        public Profile FindByName(string nameOf)
        {
            return profiles.FirstOrDefault(profile => profile.Name == nameOf);
        }

        #endregion

        #region MANAGE

        /**<summary>Add a item to the array.</summary>*/
        public void Add(Profile profile)
        {
            Array.Resize(ref profiles, Count + 1); 
            profiles[Count - 1] = profile;
        }

        /**<summary>Modify a item of the array. It's modify by item ID.</summary>*/
        public void Modify(Profile profile)
        {
            profiles[Array.IndexOf(profiles, FindByID(profile.ID))] = profile;
        }

        /**<summary>Remove a item of the array</summary>*/
        public void Remove(int id)
        {
            for (int i = Array.IndexOf(profiles, FindByID(id)); i < Count-1; i++)
            {
                profiles[i] = profiles[i+1];
                profiles[i].ID--;
            }
            Array.Resize(ref profiles, Count - 1);
        }

        #endregion
        
    }
}