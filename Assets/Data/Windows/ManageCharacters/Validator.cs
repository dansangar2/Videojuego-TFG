using Data.Windows.ManageStats;
using Entities;
using UnityEngine;

namespace Data.Windows.ManageCharacters
{
    /**<summary>Check that some data have the restriction correctly.</summary>*/
    public static class Validator
    {
        /**<summary>Save the restriction check result.</summary>*/
        private static bool _res;
        
        /**<summary>Check the restriction is correctly.</summary>*/
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
            
            #region Model

            if (item.Model == null)
            {
                advice += "Model can´t be empty.\n";
                _res = true;
            }

            #endregion

            foreach (Ability ability in item.Abilities(true))
            {
                _res = _res || StatsGeneratorWindow.Validator(ability);   
            }
            
            _res = _res || StatsGeneratorWindow.Validator(item);

            GUILayout.Label(advice);
            return _res;
        }
    }
}