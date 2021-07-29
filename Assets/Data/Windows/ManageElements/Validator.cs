using System.Linq;
using Entities;
using Enums;
using UnityEngine;

namespace Data.Windows.ManageElements
{
    public static class Validator
    {
        
        private static bool _res;
        
        public static bool Validate(Element item)
        {
            _res = false;
            string advice = "";

            #region Name

            if (string.IsNullOrEmpty(item.Name))
            {
                advice += "Name can´t be empty.\n";
                _res = true;
            }

            #endregion

            #region Icon

            if (item.Icon == null)
            {
                advice += "Icon can´t be empty.\n";
                _res = true;
            }

            #endregion

            #region Type

            if (item.Type.Equals(ElementType.None))
            {
                advice += "It is recommendable that Element won´t be \"None\".\n";
            }

            if (item.Type.Equals(null))
            {
                advice += "Element represent the type of element, can´t be null.\n";
                _res = true;
            }
            
            #endregion

            #region Strengths

            foreach (ElementType elem in item.GetMultiplicityElements().
                Where(content => item.GetMultiplicityOf(content) > 10 || 
                                 item.GetMultiplicityOf(content) < -10))
            {
                advice += elem + " has value that are not between 10 && -10.\n";
                _res = true;
            }

            #endregion

            GUILayout.Label(advice);
            return _res;
        }
    }
}