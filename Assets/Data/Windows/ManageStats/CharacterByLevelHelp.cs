using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageStats
{
    /**<summary>This window shows the evolution of stats with the Stats Generator parameters.</summary>*/
    public class CharacterByLevelHelp: EditorWindow
    {
        
        /**<summary>The window to create.</summary>*/
        private static EditorWindow _window;
        /**<summary>The ability example.</summary>*/
        private static Character _newChar;

        /**<summary>Open a Window adapted to the table with the character stats by level.</summary>*/
        public static void Window(Character chara)
        {
            _window = GetWindow<CharacterByLevelHelp>();
            _window.maxSize = new Vector2(1300, 300);
            _window.minSize = new Vector2(1100, 300);
            _newChar = chara;
        }

        /**<summary>Set a Window interface with the character stats by level.</summary>*/
        public void OnGUI()
        {
            StatsGeneratorWindow.Display(_newChar);
            if (GUILayout.Button("Close")) _window.Close();

        }

    }
}