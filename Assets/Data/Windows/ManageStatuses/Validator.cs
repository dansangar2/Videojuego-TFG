using Entities;
using UnityEngine;

namespace Data.Windows.ManageStatuses
{
    /**<summary>Check that some data have the restriction correctly.</summary>*/
    public static class Validator
    {
        /**<summary>Save the restriction check result.</summary>*/
        private static bool _res;
        
        /**<summary>Check the restriction is correctly.</summary>*/
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