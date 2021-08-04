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
            Display.Window(_window);
            _element = new Element(element);
        }

        public void OnGUI()
        {
            
            _scroll = EditorGUILayout.BeginScrollView(_scroll, GUILayout.MinHeight(5), GUILayout.MinHeight(position.height));
            EditorGUILayout.BeginVertical("Box");
            
            Display.Displayed(_element, true);
            if (GUILayout.Button("Close")) { _window.Close(); }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            
        }
    }
}

