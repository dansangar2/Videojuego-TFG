using Data.Database;
using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageProfiles
{
    public class Creator : EditorWindow
    {
        
        private static EditorWindow _window;

        private static Profile _profile;
        private static Profiles _profileDB;
        
        private Vector2 _scroll;
        
        public static void Window(Profiles profileDB)
        {
            _profileDB = profileDB;

            _window = GetWindow<Creator>();
            Display.Window(_window);

            _profile = new Profile(_profileDB.Count);
        }

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

        private static void Add()
        {
            Undo.RecordObject(_profileDB, "Profile Modify");
            _profileDB.Add(_profile);
            EditorUtility.SetDirty(_profileDB);
            _window.Close();
        }
        
    }
}