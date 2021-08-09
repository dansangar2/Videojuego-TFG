using System;
using Entities;
using UnityEngine;

namespace Core.Saves
{
    [Serializable]
    public static class SavesFiles
    {
        
        /**<summary>All save files.</summary>*/
        private static Save[] _saves = {new Save(), new Save(), new Save()};

        /**<summary>The current file to open.</summary>*/
        public static int CurrentSave { get; set; }
        
        /**<summary>String where the file is going to save.</summary>*/
        private static string _jsonFile;
        
        /**<summary>Save file name. It should have a number of file with it.</summary>*/
        private static string _fileName = "Savef-";

        #region MANAGE

        /**<summary>Initialize all saves.</summary>*/
        public static void Init()
        {
            for (int i = 0; i < _saves.Length; i++)
            {
                _saves[i] = new Save();
            }

            CurrentSave = 0;
        }
        
        /**<summary>Save the game in the save file using the current file index.</summary>*/
        public static void SaveData()
        {
            string file = _fileName + CurrentSave;
            _jsonFile = JsonUtility.ToJson(_saves[CurrentSave]);
            _saves[CurrentSave].IsEmpty = false;
            //It save the _jsonFile with the data of _currentFile.
            PlayerPrefs.SetString(file, _jsonFile);
        }

        /**<summary>Load the game in the save file using the current file index.</summary>*/
        public static void LoadData()
        {
            string file = _fileName + CurrentSave;
            _jsonFile = PlayerPrefs.GetString(file);
            _saves[CurrentSave] = JsonUtility.FromJson<Save>(_jsonFile);
        }

        /**<summary>Delete the game in the save file using the current file index.</summary>*/
        public static void DeleteData()
        {
            string file = _fileName + CurrentSave;
            _saves[CurrentSave] = new Save();
            _jsonFile = JsonUtility.ToJson(_saves[CurrentSave]);
            PlayerPrefs.SetString(file, _jsonFile);
        }

        #endregion

        #region GETTERS

        /**<summary>Get the current data.</summary>*/
        public static Save GetSave()
        {
            string file = _fileName + CurrentSave;
            _jsonFile = PlayerPrefs.GetString(file);
            return _saves[CurrentSave];
        }
        
        /**<summary>Get the party of the player.</summary>*/
        public static Character[] GetParty()
        {
            return GetSave().Party;
        }

        /**<summary>Get a specific Character of the party.</summary>*/
        public static Character GetCharacterOfParty(int i = 1)
        {
            return GetParty()[i];
        }

        #endregion
        
    }
}
