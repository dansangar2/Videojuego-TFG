using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageElements
{
    /**<summary>System that generates a new window to show.</summary>*/
    public class Show : EditorWindow
    {
        /**<summary>The new window to generate.</summary>*/
        private static EditorWindow _window;
        /**<summary>Empty item.</summary>*/
        private static Element _element;
        
        /**<summary>For move the list with the DB items.</summary>*/
        private Vector2 _scroll;
        
        
        /**<summary>Create the window.</summary>*/
        public static void Window(Element element)
        {
            _window = GetWindow<Show>();
            Display.Window(_window);
            _element = new Element(element);
        }

        /**<summary>Show the window.</summary>*/
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

