using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageElements
{
    public class Show : EditorWindow
    {
        private static EditorWindow _window;
        private static Element _element;
        
        private Vector2 _scroll;
        
        
        public static void Window(Element element)
        {
            _window = GetWindow<Show>();
            _window.maxSize = new Vector2(350, 500);
            _window.minSize = new Vector2(300, 300);
            _element = new Element(element);
        }

        public void OnGUI()
        {
            
            _scroll = EditorGUILayout.BeginScrollView(_scroll, GUILayout.MinHeight(5), GUILayout.MinHeight(position.height));
            EditorGUILayout.BeginVertical("Box");
            
            Display.Displayed(_element);
            if (GUILayout.Button("Close")) { _window.Close(); }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            
        }
    }
}

