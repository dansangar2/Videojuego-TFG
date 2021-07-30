using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageStats
{
    public class StatsByLevelHelp: EditorWindow
    {
        
        private static EditorWindow _window;
        private static Character _newChar;
        
        public static void Window(Character chara)
        {
            _window = GetWindow<StatsByLevelHelp>();
            _window.maxSize = new Vector2(1300, 300);
            _window.minSize = new Vector2(1100, 300);
            _newChar = chara;
        }

        public void OnGUI()
        {
            StatsGeneratorWindow.Display(_newChar);
            if (GUILayout.Button("Close")) _window.Close();

        }
        
        

    }
}