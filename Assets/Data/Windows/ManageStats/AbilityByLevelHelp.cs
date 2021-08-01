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
        
        
        public static void Window(Ability item)
        {
            _window = GetWindow<AbilityByLevelHelp>();
            _window.maxSize = new Vector2(1300, 300);
            _window.minSize = new Vector2(1100, 300);
            _newAbi = item;
        }

        public void OnGUI()
        {
            StatsGeneratorWindow.Display(_newAbi);
            if (GUILayout.Button("Close")) _window.Close();

        }

    }
}