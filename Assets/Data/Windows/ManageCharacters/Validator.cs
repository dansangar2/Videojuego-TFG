using Data.Windows.ManageStats;
using Entities;
using UnityEngine;

namespace Data.Windows.ManageCharacters
{
    public static class Validator
    {
        
        private static bool _res;
        
        public static bool Validate(Character item)
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

            #region Face

            if (item.Face == null)
            {
                advice += "Face can´t be empty.\n";
                _res = true;
            }

            #endregion
            
            #region Face

            if (item.Model == null)
            {
                advice += "Model can´t be empty.\n";
                _res = true;
            }

            #endregion
            
            _res = _res || StatsGeneratorWindow.Validator(item);
            
            /*#region Icon

            if (item.Icon == null)
            {
                advice += "Icon can´t be empty.\n";
                _res = true;
            }

            #endregion

            #region Type

            if (item.Type.Equals(CharacterType.None))
            {
                advice += "It is recommendable that Character won´t be \"None\".\n";
            }

            if (item.Type.Equals(null))
            {
                advice += "Character represent the type of character, can´t be null.\n";
                _res = true;
            }
            
            #endregion

            #region Strengths

            foreach (CharacterType elem in item.GetMultiplicityCharacters().
                Where(content => item.GetMultiplicityOf(content) > 10 || 
                                 item.GetMultiplicityOf(content) < -10))
            {
                advice += elem + " has value that are not between 10 && -10.\n";
                _res = true;
            }

            #endregion*/

            GUILayout.Label(advice);
            return _res;
        }
    }
}