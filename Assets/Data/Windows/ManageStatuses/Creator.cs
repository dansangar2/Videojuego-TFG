using Data.Database;
using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageStatuses
{
    /**<summary>System that generates a new window to create.</summary>*/
    public class Creator : EditorWindow
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
        public static void Window(Statuses statusDB)
        {
            _statusDB = statusDB;

            _window = GetWindow<Creator>();
            Display.Window(_window);

            _status = new Status(_statusDB.Count);
        }

        /**<summary>Show the window.</summary>*/
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

        /**<summary>Add a item.</summary>*/
        private static void Add()
        {
            Undo.RecordObject(_statusDB, "Status Modify");
            _statusDB.Add(_status);
            EditorUtility.SetDirty(_statusDB);
            _window.Close();
        }
       
    }
}