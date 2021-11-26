using System.Linq;
using Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Saves
{
    /**<summary>Get all save data files of the game.</summary>*/
    public static class SavesFiles
    {
        
        /**<summary>All save files.</summary>*/
        private static Save[] _saves = {new Save(), new Save(), new Save()};

        /**<summary>The "new save file" or other one of _save if is load.</summary>*/
        private static Save _currentSave = new Save();

        /**<summary>The current file to open. Select 3 for default (new data).</summary>*/
        public static int CurrentSave { get; set; }
        
        /**<summary>String where the file is going to save.</summary>*/
        private static string _jsonFile;
        
        /**<summary>Save file name. It should have a number of file with it.</summary>*/
        private static string _fileName = "Savef-";

        /**<summary>Check if the file was load.</summary>*/
        private static bool _isLoaded;

        #region MANAGE

        /**<summary>Initialize all saves.</summary>*/
        public static void Init()
        {
            _currentSave = new Save();
            for (int i = 0; i < _saves.Length; i++)
            {
                _saves[i] = new Save();
            }
        }
        
        /**<summary>Save the game in the save file using the current file index.</summary>*/
        public static void SaveData()
        {
            string file = _fileName + CurrentSave;
            _currentSave.IsEmpty = false;
            _currentSave.SetScene(SceneManager.GetActiveScene().name);
            _jsonFile = JsonUtility.ToJson(_currentSave, true);
            _saves[CurrentSave] = JsonUtility.FromJson<Save>(_jsonFile);
            //It save the _jsonFile with the data of _currentFile
            PlayerPrefs.SetString(file, _jsonFile);
        }

        /**<summary>Load the game in the save file using the current file index.</summary>*/
        public static bool LoadData()
        {
            _isLoaded = true;
            string file = _fileName + CurrentSave;
            _jsonFile = PlayerPrefs.GetString(file);
            if (_jsonFile.Equals("")) return false;
            _currentSave = JsonUtility.FromJson<Save>(_jsonFile);
            _currentSave.GetScene();
            return true;
        }
        
        /**<summary>Get all data.</summary>*/
        public static Save[] GetData()
        {
            for (int i = 0; i < 3; i++)
            {
                string file = _fileName + i;
                _jsonFile = PlayerPrefs.GetString(file);
                if (_jsonFile.Equals("")) _saves[i] = new Save();
                else _saves[i] = JsonUtility.FromJson<Save>(_jsonFile);
            }

            return _saves;
        }

        /**<summary>Delete the game in the save file using the current file index.</summary>*/
        public static void DeleteData()
        {
            string file = _fileName + CurrentSave;
            _saves[CurrentSave] = new Save();
            _jsonFile = JsonUtility.ToJson(_saves[CurrentSave]);
            PlayerPrefs.SetString(file, _jsonFile);
        }
        
        /**<summary>Delete all saves.
        <param name="excludes">The saves that won't delete</param>
        </summary>*/
        public static void DeleteAllData(params int[] excludes)
        {
            for (int i = 0; i < 3; i++)
            {
                if(excludes.Contains(i)) continue;
                string file = _fileName + i;
                _saves[i] = new Save();
                _jsonFile = JsonUtility.ToJson(_saves[i]);
                PlayerPrefs.SetString(file, _jsonFile);
            }
        }

        #endregion

        #region GETTERS
        
        /**<summary>Check if one file was loaded. If that method was used and returned true,
        the next time, it'll return false.</summary>*/
        public static bool IsLoaded
        {
            get
            {
                bool l = _isLoaded;
                _isLoaded = false;
                return l;
            }
        }

        /**<summary>Get all saves files.</summary>*/
        public static Save[] Saves => _saves;

        /**<summary>Get the current save loaded.</summary>*/
        public static Save GetSave()
        {
            return _currentSave;
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
