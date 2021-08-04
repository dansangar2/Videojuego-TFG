using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageProfiles
{
    public class Show : EditorWindow
    {
        private static EditorWindow _window;
        private static Profile _profile;
        
        private Vector2 _scroll;
        
        
        public static void Window(Profile profile)
        {
            _window = GetWindow<Show>();
            Display.Window(_window);
            _profile = new Profile(profile);
        }

        public void OnGUI()
        {
            
            _scroll = EditorGUILayout.BeginScrollView(_scroll, GUILayout.MinHeight(5), GUILayout.MinHeight(position.height));
            EditorGUILayout.BeginVertical("Box");
            
            Display.Displayed(_profile, true);
            if (GUILayout.Button("Close")) { _window.Close(); }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            
        }
    }
}

