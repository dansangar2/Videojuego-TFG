using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageAbilities
{
    /**<summary>System that generates a new window to show.</summary>*/
    public class Show : EditorWindow
    {
        /**<summary>The new window to generate.</summary>*/
        private static EditorWindow _window;
        /**<summary>Empty item.</summary>*/
        private static Ability _ability;
        
        /**<summary>For move the list with the DB items.</summary>*/
        private Vector2 _scroll;
        
        
        /**<summary>Create the window.</summary>*/
        public static void Window(Ability ability)
        {
            _window = GetWindow<Show>();
            Display.Window(_window);
            _ability = new Ability(ability);
        }

        /**<summary>Show the window.</summary>*/
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

