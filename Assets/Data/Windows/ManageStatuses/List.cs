using System;
using Data.Database;
using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageStatuses
{
    /**<summary>System that list the DB items.</summary>*/
    [CustomEditor(typeof(Statuses))]
    public class List: Editor
    {
        /**<summary>Item DB.</summary>*/
        private Statuses _statusDB;
        /**<summary>Item to search by ID or name.</summary>*/
        private string _searchByName = "";
        /**<summary>Check if _searchByName isn't null or empty.</summary>*/
        private bool _canSearch;
        /**<summary>Item to delete.</summary>*/
        private Status _deleted;

        /**<summary>Initialize the DB when the file is clicked.</summary>*/
        private void OnEnable()
        {
            _statusDB = (Statuses)target;
        }

        #region INSPECTOR

        /**<summary>List all items of the DB, with the filter and creator.</summary>*/
        public override void OnInspectorGUI()
        {
            //if (_statusDB.All.Length == 0) return;
            
            #region Total

            EditorGUILayout.BeginHorizontal("Box");
            GUILayout.Label("Status created: " + _statusDB.Count);
            EditorGUILayout.EndHorizontal();

            #endregion
            
            #region Search

            if (_statusDB.Count != 0)
            {
                EditorGUILayout.BeginHorizontal("Box"); 
                GUILayout.Label("Search: ");
                _searchByName = GUILayout.TextField(_searchByName); 
                EditorGUILayout.EndHorizontal();
            }

            #endregion

            #region Manage

            if (GUILayout.Button("Add Status"))
            {
                Creator.Window(_statusDB);
            }
            _canSearch = !String.IsNullOrEmpty(_searchByName);
            
            foreach (Status status in _statusDB.All)
            {
                if (_canSearch)
                {
                    if (status.Name == _searchByName || status.Name.Contains(_searchByName) || status.ID.ToString() == _searchByName)
                    {
                        Display(status);
                    }
                }
                else Display(status);

                if (_deleted != null)
                {
                    _statusDB.Remove(_deleted.ID);
                        
                }
            }

            #endregion
        }

        #endregion
        
        #region DISPLAY

        /**<summary>Show an item, with some data and buttons to delete, show and modify.</summary>*/
        private void Display(Status status)
        {

            #region Options

            GUIStyle valuesStyle = new GUIStyle(GUI.skin.label)
            {
                wordWrap = true, alignment = TextAnchor.MiddleCenter, margin = new RectOffset(0, 50, 0, 0)
            };

            #endregion

            EditorGUILayout.BeginVertical("Box");
            
            #region ID

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("ID: ");
            GUILayout.Label(status.ID.ToString(), valuesStyle);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Name

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Name: ");
            GUILayout.Label(status.Name, valuesStyle);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Show

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Show"))
            {
                Show.Window(status);
            }

            #endregion

            #region Modify

            if (GUILayout.Button("Modify"))
            {
                Modifier.Window(_statusDB, status);
            }

            #endregion

            #region Delete

            _deleted = GUILayout.Button("Delete") ? status : null;            

            #endregion
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            
        }
        
        #endregion
    }
}