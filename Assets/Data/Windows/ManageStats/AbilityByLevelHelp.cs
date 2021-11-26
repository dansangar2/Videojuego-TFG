using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageStats
{
    /**<summary>This window shows the evolution of stats with the Stats Generator parameters.</summary>*/
    public class AbilityByLevelHelp: EditorWindow
    {
        
        /**<summary>The window to create.</summary>*/
        private static EditorWindow _window;
        /**<summary>The ability example.</summary>*/
        private static Ability _newAbi;
        /**<summary>Initial level example.</summary>*/
        private static int _level;
        /**<summary>Max level example.</summary>*/
        private static int _max;
        
        /**<summary>Open a Window adapted to the table with the ability stats by level.</summary>*/
        public static void Window(Ability item, int level, int max)
        {
            _window = GetWindow<AbilityByLevelHelp>();
            _window.maxSize = new Vector2(1300, 300);
            _window.minSize = new Vector2(1100, 300);
            _newAbi = item;
            _level = level;
            _max = max;
        }

        /**<summary>Set a Window interface with the ability stats by level.</summary>*/
        public void OnGUI()
        {
            StatsGeneratorWindow.Display(_newAbi, _level, _max);
            if (GUILayout.Button("Close")) _window.Close();

        }

    }
}