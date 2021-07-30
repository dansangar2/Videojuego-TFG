using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageCharacters
{
    public class Show : EditorWindow
    {
        private static EditorWindow _window;
        private static Character _character;
        
        private Vector2 _scroll;
        
        
        public static void Window(Character character)
        {
            _window = GetWindow<Show>();
            _window.maxSize = new Vector2(1000, 600);
            _window.minSize = new Vector2(900, 600);
            _character = new Character(character);
        }

        public void OnGUI()
        {
            
            _scroll = EditorGUILayout.BeginScrollView(_scroll, GUILayout.MinHeight(5), GUILayout.MinHeight(position.height));
            EditorGUILayout.BeginVertical("Box");
            
            Display.Displayed(_character, true);
            if (GUILayout.Button("Close")) { _window.Close(); }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            
        }
    }
}

