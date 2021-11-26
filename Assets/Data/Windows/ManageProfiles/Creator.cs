using Data.Database;
using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageProfiles
{
    public class Creator : EditorWindow
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
        public static void Window(Profiles profileDB)
        {
            _profileDB = profileDB;

            _window = GetWindow<Creator>();
            Display.Window(_window);

            _profile = new Profile(_profileDB.Count);
        }

        /**<summary>Show the window.</summary>*/
        public void OnGUI()
        {
            
            _scroll = EditorGUILayout.BeginScrollView(_scroll, GUILayout.MinHeight(5),
                GUILayout.MinHeight(position.height));
            EditorGUILayout.BeginVertical("Box");
            
            Display.Displayed(_profile);
            EditorGUI.BeginDisabledGroup(Validator.Validate(_profile));
            if (GUILayout.Button("Confirm")) { Add(); }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            EditorGUI.EndDisabledGroup();

        }

        /**<summary>Add a item.</summary>*/
        private static void Add()
        {
            Undo.RecordObject(_profileDB, "Profile Modify");
            _profileDB.Add(_profile);
            EditorUtility.SetDirty(_profileDB);
            _window.Close();
        }
        
    }
}