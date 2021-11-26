using Data.Database;
using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageStatuses
{
    /**<summary>System that generates a new window to modify.</summary>*/
    public class Modifier : EditorWindow
    {
        /**<summary>The new window to generate.</summary>*/
        private static EditorWindow _window;
        
        /**<summary>Empty item.</summary>*/
        private static Status _status;
        /**<summary>Item DB.</summary>*/
        private static Statuses _statusDB;
        
        /**<summary>For move the list with the DB items.</summary>*/
        private Vector2 _scroll;

        
        /**<summary>Create the window.</summary>*/
        public static void Window(Statuses statusDB, Status status)
        {
            _statusDB = statusDB;
            
            _window = GetWindow<Modifier>();
            Display.Window(_window);
            _status = new Status(status);
            
        }
        
        /**<summary>Show the window.</summary>*/
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

        /**<summary>Modify a item.</summary>*/
        private static void Modify()
        {
            Undo.RecordObject(_statusDB, "Status Modify");
            _statusDB.Modify(_status);
            EditorUtility.SetDirty(_statusDB);
            _window.Close();
        }
    }
}
