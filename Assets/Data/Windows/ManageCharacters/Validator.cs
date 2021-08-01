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
            
            #region Model

            if (item.Model == null)
            {
                advice += "Model can´t be empty.\n";
                _res = true;
            }

            #endregion
            
            _res = _res || StatsGeneratorWindow.Validator(item);

            GUILayout.Label(advice);
            return _res;
        }
    }
}