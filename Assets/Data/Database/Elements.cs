using System;
using System.Linq;
using Entities;
using UnityEngine;

namespace Data.Database
{
    
    /**<summary>The Elements Database system.</summary>*/
    [CreateAssetMenu(menuName = "Database/Elements", fileName = "ElementDB")]
    public class Elements : ScriptableObject
    {
        
        /**<summary>Array that save the Serialize data.</summary>*/ 
        [SerializeField] private Element[] elements = {};

        #region GET
        
        /**<summary>Get the number of items.</summary>*/ 
        public int Count => elements.Length;
    
        /**<summary>Get All data in the array.</summary>*/ 
        public Element[] All => elements;
    
        /**<summary>Get All Names of the items of the array.</summary>*/ 
        public string[] Names 
        { 
            get 
            { 
                string[] res = new string[elements.Length]; 
                for (int i = 0; i < elements.Length; i++) { res[i] = elements[i].Name; } 
                return res;
            }
        }
        
        #endregion
        
        #region FINDBY
        
        /**<summary>Find item by ID.</summary>*/ 
        public Element FindByID(int id) 
        { 
            return elements.FirstOrDefault(element => element.ID == id);
        }
        
        /**<summary>Find item by name.</summary>*/ 
        public Element FindByName(string nameOf) 
        { 
            return elements.FirstOrDefault(element => element.Name == nameOf);
        }
        
        #endregion
        
        #region MANAGE
        
        /**<summary>Add a item to the array.</summary>*/ 
        public void Add(Element element) 
        { 
            Array.Resize(ref elements, Count + 1); 
            elements[Count - 1] = element;
        }
        
        /**<summary>Modify a item of the array. It's modify by the item ID.</summary>*/ 
        public void Modify(Element element) 
        { 
            elements[Array.IndexOf(elements, FindByID(element.ID))] = element;
        }
    
        /**<summary>Remove a item of the array</summary>*/ 
        public void Remove(int id) 
        { 
            for (int i = Array.IndexOf(elements, FindByID(id)); i < Count-1; i++) 
            { 
                elements[i] = elements[i+1]; 
                elements[i].ID--; 
            } 
            Array.Resize(ref elements, Count - 1); 
        }
        
        #endregion
        
    }
}
