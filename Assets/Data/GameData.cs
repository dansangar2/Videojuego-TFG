using Data.Database;
using UnityEditor;

namespace Data
{
    public static class GameData
    {
        
        private const string CurrentDirectory = "Assets/Data/";
        
        /**<summary>The initial data of the characters that the player can fight.</summary>*/
        public static readonly Characters CharacterDB = (Characters)AssetDatabase.LoadAssetAtPath(CurrentDirectory + "CharacterDB.asset", typeof(Characters));
        /**<summary>The initial data of the enemies that the player must defeat.</summary>*/
        public static readonly Characters EnemyDB = (Characters)AssetDatabase.LoadAssetAtPath(CurrentDirectory + "EnemyDB.asset", typeof(Characters));
        /**<summary>The initial data of the elements.</summary>*/
        public static readonly Elements ElementDB = (Elements)AssetDatabase.LoadAssetAtPath(CurrentDirectory + "ElementDB.asset", typeof(Elements));
        /**<summary>The initial data of the abilities.</summary>*/
        public static readonly Abilities AbilityDB = (Abilities)AssetDatabase.LoadAssetAtPath(CurrentDirectory + "AbilityDB.asset", typeof(Abilities));
        /**<summary>The initial data of the profiles.</summary>*/
        public static readonly Profiles ProfileDB = (Profiles)AssetDatabase.LoadAssetAtPath(CurrentDirectory + "ProfileDB.asset", typeof(Profiles));
        /**<summary>The initial data of the statuses.</summary>*/
        public static readonly Statuses StatusDB = (Statuses)AssetDatabase.LoadAssetAtPath(CurrentDirectory + "StatusDB.asset", typeof(Statuses));
    }
}
