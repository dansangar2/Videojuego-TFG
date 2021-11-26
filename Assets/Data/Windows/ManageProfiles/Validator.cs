using Entities;
using UnityEngine;

namespace Data.Windows.ManageProfiles
{
    /**<summary>Check that some data have the restriction correctly.</summary>*/
    public static class Validator
    {
        /**<summary>Save the restriction check result.</summary>*/
        private static bool _res;
        
        /**<summary>Check the restriction is correctly.</summary>*/
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