using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageStats
{
    /**<summary>This window shows the evolution of stats with the Stats Generator parameters.</summary>*/
    public class AbilityByLevelHelp: EditorWindow
    {
        
        private static EditorWindow _window;
        private static Ability _newAbi;
        private static int _level;
        private static int _max;
        
        public static void Window(Ability item, int level, int max)
        {
            _window = GetWindow<AbilityByLevelHelp>();
            _window.maxSize = new Vector2(1300, 300);
            _window.minSize = new Vector2(1100, 300);
            _newAbi = item;
            _level = level;
            _max = max;
        }

        public void OnGUI()
        {
            StatsGeneratorWindow.Display(_newAbi, _level, _max);
            if (GUILayout.Button("Close")) _window.Close();

        }

    }
}