using Data.Database;
using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageStatuses
{
    public class Creator : EditorWindow
    {
        
        private static EditorWindow _window;

        private static Status _status;
        private static Statuses _statusDB;
        
        private Vector2 _scroll;
        
        public static void Window(Statuses statusDB)
        {
            _statusDB = statusDB;

            _window = GetWindow<Creator>();
            Display.Window(_window);

            _status = new Status(_statusDB.Count);
        }

        public void OnGUI()
        {
            
            _scroll = EditorGUILayout.BeginScrollView(_scroll, GUILayout.MinHeight(5),
                GUILayout.MinHeight(position.height));
            EditorGUILayout.BeginVertical("Box");
            
            Display.Displayed(_status);
            EditorGUI.BeginDisabledGroup(Validator.Validate(_status));
            if (GUILayout.Button("Confirm")) { Add(); }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            EditorGUI.EndDisabledGroup();

        }

        private static void Add()
        {
            Undo.RecordObject(_statusDB, "Status Modify");
            _statusDB.Add(_status);
            EditorUtility.SetDirty(_statusDB);
            _window.Close();
        }
       
    }
}