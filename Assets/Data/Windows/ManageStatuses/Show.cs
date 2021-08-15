using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageStatuses
{
    public class Show : EditorWindow
    {
        private static EditorWindow _window;
        private static Status _status;
        
        private Vector2 _scroll;
        
        
        public static void Window(Status status)
        {
            _window = GetWindow<Show>();
            Display.Window(_window);
            _status = new Status(status);
        }

        public void OnGUI()
        {
            
            _scroll = EditorGUILayout.BeginScrollView(_scroll, GUILayout.MinHeight(5), GUILayout.MinHeight(position.height));
            EditorGUILayout.BeginVertical("Box");
            
            Display.Displayed(_status, true);
            if (GUILayout.Button("Close")) { _window.Close(); }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            
        }
    }
}

