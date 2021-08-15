using Data.Database;
using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageStatuses
{
    public class Modifier : EditorWindow
    {
        private static EditorWindow _window;
        private static Status _status;
        private static Statuses _statusDB;
        
        private Vector2 _scroll;

        
        public static void Window(Statuses elementDB, Status element)
        {
            _statusDB = elementDB;
            
            _window = GetWindow<Modifier>();
            Display.Window(_window);
            _status = new Status(element);
            
        }
        
        public void OnGUI()
        {
            
            _scroll = EditorGUILayout.BeginScrollView(_scroll, GUILayout.MinHeight(5), GUILayout.MinHeight(position.height));
            EditorGUILayout.BeginVertical("Box");
            
            Display.Displayed(_status);
            EditorGUI.BeginDisabledGroup(Validator.Validate(_status));
            if (GUILayout.Button("Confirm")) { Modify(); }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            EditorGUI.EndDisabledGroup();

        }

        private static void Modify()
        {
            Undo.RecordObject(_statusDB, "Status Modify");
            _statusDB.Modify(_status);
            EditorUtility.SetDirty(_statusDB);
            _window.Close();
        }
    }
}
