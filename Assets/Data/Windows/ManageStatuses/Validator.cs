﻿using Entities;
using UnityEngine;

namespace Data.Windows.ManageStatuses
{
    public static class Validator
    {
        
        private static bool _res;
        
        public static bool Validate(Status item)
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
/*
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

            #region Stats
            
            foreach (var t in item.Stats)
            {
                if (!(t <= 0)) continue;
                advice += "One or more base values is equals or less of 0, check it and change it by a number upper of 0.\n";
                _res = false;
            }

            #endregion
            */
            //_res = _res || StatsGeneratorWindow.Validator(item);
            
            GUILayout.Label(advice);
            return _res;
        }
    }
}