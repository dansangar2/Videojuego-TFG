using System;
using UnityEngine;

namespace Entities
{
    /**<summary>The profiles of the characters.</summary>*/ 
    [Serializable]
    public class Profile: Base
    {
        
        #region ATTRIBUTES
        
        [SerializeField] private Sprite art;
        
        #endregion
        
        #region CONSTRUCTORS
        
        /**<summary>
        Empty Profile constructor
        </summary>*/ 
        public Profile(int id) : base(id) { }
        
        /**<summary>
        Profile clone constructor
        </summary>*/ 
        public Profile(Profile profile) : base(profile) 
        { 
            Art = profile.Art;
        }
        
        #endregion
        
        #region GETTERS & SETTERS
     
        /**<summary>Official art of the character.</summary>*/ 
        public Sprite Art { get => art; set => art = value; }
        
        #endregion
    
    }
}