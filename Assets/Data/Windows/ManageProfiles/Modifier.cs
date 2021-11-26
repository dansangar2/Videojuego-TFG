using Data.Database;
using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageProfiles
{
    /**<summary>System that generates a new window to modify.</summary>*/
    public class Modifier : EditorWindow
    {
        /**<summary>The new window to generate.</summary>*/
        private static EditorWindow _window;
        
        /**<summary>Empty item.</summary>*/
        private static Profile _profile;
        /**<summary>Item DB.</summary>*/
        private static Profiles _profileDB;
        
        /**<summary>For move the list with the DB items.</summary>*/
        private Vector2 _scroll;

        
        /**<summary>Create the window.</summary>*/
        public static void Window(Profiles profileDB, Profile profile)
        {
            _profileDB = profileDB;
            
            _window = GetWindow<Modifier>();
            Display.Window(_window);
            _profile = new Profile(profile);
            
        }
        
        /**<summary>Show the window.</summary>*/
        public void OnGUI()
        {
            
            _scroll = EditorGUILayout.BeginScrollView(_scroll, GUILayout.MinHeight(5), GUILayout.MinHeight(position.height));
            EditorGUILayout.BeginVertical("Box");
            
            Display.Displayed(_profile);
            EditorGUI.BeginDisabledGroup(Validator.Validate(_profile));
            if (GUILayout.Button("Confirm")) { Modify(); }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            EditorGUI.EndDisabledGroup();

        }

        /**<summary>Modify a item.</summary>*/
        private static void Modify()
        {
            Undo.RecordObject(_profileDB, "Profile Modify");
            _profileDB.Modify(_profile);
            EditorUtility.SetDirty(_profileDB);
            _window.Close();
        }
    }
}
