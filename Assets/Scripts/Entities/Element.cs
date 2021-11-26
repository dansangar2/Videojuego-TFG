using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using Utils;

namespace Entities
{
    /**<summary>The elementos of the game</summary>*/ 
    [Serializable]
    public class Element: Base
    {
        
        #region ATTRIBUTES
        
        [SerializeField] private ElementType type = ElementType.None;
        [SerializeField] private Sprite icon; 
        [SerializeField] private SerializableDictionary<ElementType, float> multiplicity = new SerializableDictionary<ElementType, float>();
           
        #endregion
        
        #region CONSTRUCTORS
        
        /**<summary>Empty Element constructor.</summary>*/ 
        public Element(int id): base(id){ }

        /**<summary>Element clone constructor.</summary>*/ 
        public Element(Element element): base(element) 
        { 
            Type = element.Type; 
            Icon = element.Icon;
            foreach (KeyValuePair<ElementType, float> values in element.multiplicity)
            {
                AddMultiplicityTo(values.Key, values.Value);
            }
        }
        
        #endregion
        
        #region GETTERS & SETTERS
        
        /**<summary>
        The internal identification for the Element.
        </summary>*/ 
        public ElementType Type { get => type; set => type = value; }
        
        /**<summary>
        The icon of the element.
        </summary>*/ 
        public Sprite Icon { get => icon; set => icon = value; }
        
        /**<summary>
        Multiplicity
        <para>The effect of element over other</para>
        value = 1 -> normal damage (Default)
        <para>value > 1 -> more damage than normal</para>
        1 > value > 0 -> less damage than normal
        <para>0 > value -> the attack recover</para>
        </summary>*/
        public SerializableDictionary<ElementType, float> Multiplicity { get => multiplicity; set => multiplicity = value; }
        
        /**<summary>
        Get multiplicity of...
        <para>Get a multiplicity (Damage index) of ElementType that you are passing by parameter.</para>
        <param name="element">The Element type that you like to get its index</param>
        </summary>*/ 
        public float GetMultiplicityOf(ElementType element) 
        { 
            return Multiplicity[element];
        }
        
        /**<summary>
        Get All Elements type of multiplicity of Dictionary.
        </summary>*/
        public HashSet<ElementType> GetMultiplicityElements()
        { 
            return new HashSet<ElementType>(Multiplicity.Keys);
        }
        
        /**<summary>
        Get all values of multiplicity of Dictionary multiplicity.
        </summary>*/ 
        public HashSet<float> GetMultiplicityValues() 
        { 
            return new HashSet<float>(Multiplicity.Values);
        }
        
        /**<summary>
        Get the number of multiplicity that there are.
        </summary>*/ 
        public int GetMultiplicityCount() 
        { 
            return Multiplicity.Count; 
        }
          
        #endregion
        
        #region METHODS
        
        /**<summary>
        Add Multiplicity has 2 functionalities:
        <para>- Add new Type with its multiplicity multiplicity.</para>
        - Change the value of existing Type multiplicity.
        <param name="element">The type that you want add or change.</param>
        <param name="value">The value of the type.</param>
        <param name="toChange">Default yes. If it's true, it'll change the value of key if it exits</param>
        </summary>*/ 
        public void AddMultiplicityTo(ElementType element, float value, bool toChange = true) 
        {
            if (Multiplicity.ContainsKey(element))
            {
                if(toChange) Multiplicity[element] = value;
            }
            else
            {
                Multiplicity.Add(element, value);
            } 
        }
     
        /**<summary>
        Delete Multiplicity.
        <para>Remove from the Dictionary the type and the multiplicity passing by parameter</para>
        <param name="element">The type to remove</param>
        </summary>*/ 
        public void DeleteMultiplicity(ElementType element) 
        { 
            if (Multiplicity.ContainsKey(element)) 
            { 
                Multiplicity.Remove(element); 
            } 
        }
        
        #endregion
        
    }
}