using System;
using System.Linq;
using Entities;
using UnityEngine;

namespace Data.Database
{
    
    /**<summary>The Statuses Database system.</summary>*/
    [CreateAssetMenu(menuName = "Database/Statuses", fileName = "StatusDB")]
    public class Statuses : ScriptableObject
    {
        
        /**<summary>Array that save the Serialize data.</summary>*/ 
        [SerializeField] private Status[] statuses;

        
        /**<summary>Clone.</summary>*/ 
        public void Clone(Statuses database)
        {
            this.statuses = new Status[database.Count];
            for (int i = 0; i < database.Count; i++)
            {
                this.statuses[i] = new Status(database.statuses[i]);
            }
        }

        #region GET
        
        /**<summary>Get the number of items.</summary>*/ 
        public int Count => statuses.Length;
    
        /**<summary>Get All data in the array.</summary>*/ 
        public Status[] All => statuses;
    
        /**<summary>Get All Names of the items of the array.</summary>*/ 
        public string[] Names 
        { 
            get 
            { 
                string[] res = new string[statuses.Length]; 
                for (int i = 0; i < statuses.Length; i++) { res[i] = statuses[i].Name; } 
                return res;
            }
        }
        
        #endregion
        
        #region FINDBY
        
        /**<summary>Find item by ID.</summary>*/ 
        public Status FindByID(int id) 
        { 
            return statuses.FirstOrDefault(element => element.ID == id);
        }
        
        /**<summary>Find item by name.</summary>*/ 
        public Status FindByName(string nameOf) 
        { 
            return statuses.FirstOrDefault(element => element.Name == nameOf);
        }
        
        #endregion
        
        #region MANAGE
        
        /**<summary>Add a item to the array.</summary>*/ 
        public void Add(Status status) 
        { 
            Array.Resize(ref statuses, Count + 1); 
            statuses[Count - 1] = status;
        }
        
        /**<summary>Modify a item of the array. It's modify by the item ID.</summary>*/ 
        public void Modify(Status element) 
        { 
            statuses[Array.IndexOf(statuses, FindByID(element.ID))] = element;
        }
    
        /**<summary>Remove a item of the array</summary>*/ 
        public void Remove(int id) 
        { 
            for (int i = Array.IndexOf(statuses, FindByID(id)); i < Count-1; i++) 
            { 
                statuses[i] = statuses[i+1]; 
                statuses[i].ID--; 
            } 
            Array.Resize(ref statuses, Count - 1); 
        }
        
        #endregion
        
    }
}
