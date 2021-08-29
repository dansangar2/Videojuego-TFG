using Entities;
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

            GUILayout.Label(advice);
            return _res;
        }
    }
}