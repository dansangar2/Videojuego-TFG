using System;
using Data.Windows.ManageStats;
using Entities;
using UnityEngine;

namespace Data.Windows.ManageAbilities
{
    public static class Validator
    {
        
        private static bool _res;
        
        public static bool Validate(Ability item)
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

            #region Formula

            try
            {
                item.Damage(GameData.CharacterDB.FindByID(0), GameData.CharacterDB.FindByID(0));
            }
            catch (Exception)
            {
                advice += "The formula has a error.";
                _res = true;
            }

            #endregion
            
            _res = _res || StatsGeneratorWindow.Validator(item);
            
            GUILayout.Label(advice);
            return _res;
        }
    }
}