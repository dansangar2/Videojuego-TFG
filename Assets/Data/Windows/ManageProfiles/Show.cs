using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageProfiles
{
    /**<summary>System that generates a new window to show.</summary>*/
    public class Show : EditorWindow
    {
        /**<summary>The new window to generate.</summary>*/
        private static EditorWindow _window;
        /**<summary>Empty item.</summary>*/
        private static Profile _profile;
        
        /**<summary>For move the list with the DB items.</summary>*/
        private Vector2 _scroll;
        
        
        /**<summary>Create the window.</summary>*/
        public static void Window(Profile profile)
        {
            _window = GetWindow<Show>();
            Display.Window(_window);
            _profile = new Profile(profile);
        }

        /**<summary>Show the window.</summary>*/
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

