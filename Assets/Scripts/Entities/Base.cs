using System;
using UnityEngine;

namespace Entities
{
    
    /**<summary>Base parameters for all entities. The parameters in common.</summary>*/
    [Serializable]
    public class Base
    {
        #region ATTRIBUTES

        [SerializeField] protected int id;
        [SerializeField] protected string name;
        [SerializeField] protected string description;

        #endregion
        
        #region CONSTRUCTORS

        /**<summary>
        A empty constructor for "Base".
        <param name="id">Represent the ID that this object is having</param>
        </summary>*/
        protected Base(int id) { this.id = id; }

        /**<summary>
        A copy constructor.
        <param name="bases">The "Base" that is going to copy</param>
        </summary>*/
        protected Base(Base bases)
        {
            id = bases.ID;
            Name = bases.Name;
            Description = bases.Description;
        }

        #endregion

        #region GETTERS AND SETTERS
        
        /**<summary>The ID of the object.</summary>*/
        public int ID { get => id; set => id = value; }

        /**<summary>The name that will show in the game.</summary>*/
        public string Name { get => name; set => name = value; }
        
        /**<summary>The description of the element of the game.</summary>*/
        public string Description { get => description; set => description = value; }
        
        #endregion
        
        public override string ToString()
        {
            return "(#ID:" + id + ") " + GetType() + ": " + Name;
        }
        
    }
}
