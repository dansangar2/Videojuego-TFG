using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageAbilities
{
    public class Show : EditorWindow
    {
        private static EditorWindow _window;
        private static Ability _ability;
        
        private Vector2 _scroll;
        
        
        public static void Window(Ability ability)
        {
            _window = GetWindow<Show>();
            _window.maxSize = new Vector2(450, 700);
            _window.minSize = new Vector2(450, 600);
            _ability = new Ability(ability);
        }

        public void OnGUI()
        {
            
            _scroll = EditorGUILayout.BeginScrollView(_scroll, GUILayout.MinHeight(5), GUILayout.MinHeight(position.height));
            EditorGUILayout.BeginVertical("Box");
            
            Display.Displayed(_ability, true);
            if (GUILayout.Button("Close")) { _window.Close(); }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            
        }
    }
}

