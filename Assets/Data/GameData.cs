using Data.Database;
using UnityEngine;

namespace Data
{
    public static class GameData
    {

        private const string CurrentDirectory = "";//"/Assets/Data/";

        /**<summary>The initial data of the characters that the player can fight.</summary>*/
        public static readonly Characters CharacterDB = Resources.Load<Characters>("CharacterDB");
        /**<summary>The initial data of the enemies that the player must defeat.</summary>*/
        public static readonly Characters EnemyDB = Resources.Load<Characters>(CurrentDirectory + "EnemyDB");
        /**<summary>The initial data of the elements.</summary>*/
        public static readonly Elements ElementDB = Resources.Load<Elements>(CurrentDirectory + "ElementDB");
        /**<summary>The initial data of the abilities.</summary>*/
        public static readonly Abilities AbilityDB = Resources.Load<Abilities>(CurrentDirectory + "AbilityDB");
        /**<summary>The initial data of the profiles.</summary>*/
        public static readonly Profiles ProfileDB = Resources.Load<Profiles>(CurrentDirectory + "ProfileDB");
        /**<summary>The initial data of the statuses.</summary>*/
        public static readonly Statuses StatusDB = Resources.Load<Statuses>(CurrentDirectory + "StatusDB");
    }
}
