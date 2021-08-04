using Entities;
using UnityEngine;

namespace Data.Windows.ManageProfiles
{
    public static class Validator
    {
        
        private static bool _res;
        
        public static bool Validate(Profile item)
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

            if (item.Art == null)
            {
                advice += "Art can´t be empty.\n";
                _res = true;
            }

            #endregion

            GUILayout.Label(advice);
            return _res;
        }
    }
}