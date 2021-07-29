using Data.Database;
using UnityEditor;

namespace Data
{
    public static class GameData
    {
        private const string CurrentDirectory = "Assets/Data/";
        
        //public static readonly Characters CharacterDB = (Characters)AssetDatabase.LoadAssetAtPath(CurrentDirectory + "CharacterDB.asset", typeof(Characters));
        //public static readonly Characters EnemyDB = (Characters)AssetDatabase.LoadAssetAtPath(CurrentDirectory + "EnemyDB.asset", typeof(Characters));
        public static readonly Elements ElementDB = (Elements)AssetDatabase.LoadAssetAtPath(CurrentDirectory + "ElementDB.asset", typeof(Elements));
        //public static readonly Abilities AbilityDB = (Abilities)AssetDatabase.LoadAssetAtPath(CurrentDirectory + "AbilityDB.asset", typeof(Abilities));
        //public static readonly Profiles ProfileDB = (Profiles)AssetDatabase.LoadAssetAtPath(CurrentDirectory + "ProfileDB.asset", typeof(Profiles));
    }
}
