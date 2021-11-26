using System;
using Data.Database;
using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageElements
{
    /**<summary>System that list the DB items.</summary>*/
    [CustomEditor(typeof(Elements))]
    public class List: Editor
    {
        /**<summary>Item DB.</summary>*/
        private Elements _elementDB;
        /**<summary>Item to search by ID or name.</summary>*/
        private string _searchByName = "";
        /**<summary>Check if _searchByName isn't null or empty.</summary>*/
        private bool _canSearch;
        /**<summary>Item to delete.</summary>*/
        private Element _deleted;

        /**<summary>Initialize the DB when the file is clicked.</summary>*/
        private void OnEnable()
        {
            _elementDB = (Elements)target;
        }

        #region INSPECTOR

        /**<summary>List all items of the DB, with the filter and creator.</summary>*/
        public override void OnInspectorGUI()
        {
            if (!_elementDB) return;
            
            #region Total

            EditorGUILayout.BeginHorizontal("Box");
            GUILayout.Label("Element created: " + _elementDB.Count);
            EditorGUILayout.EndHorizontal();

            #endregion
            
            #region Search

            if (_elementDB.Count != 0)
            {
                EditorGUILayout.BeginHorizontal("Box"); 
                GUILayout.Label("Search: ");
                _searchByName = GUILayout.TextField(_searchByName); 
                EditorGUILayout.EndHorizontal();
            }

            #endregion

            #region Manage

            if (GUILayout.Button("Add Element"))
            {
                Creator.Window(_elementDB);
            }
            _canSearch = !String.IsNullOrEmpty(_searchByName);
            
            foreach (Element element in _elementDB.All)
            {
                if (_canSearch)
                {
                    if (element.Name == _searchByName || element.Name.Contains(_searchByName) || element.ID.ToString() == _searchByName)
                    {
                        Display(element);
                    }
                }
                else Display(element);

                if (_deleted != null)
                {
                    _elementDB.Remove(_deleted.ID);
                        
                }
            }

            #endregion
        }

        #endregion
        
        #region DISPLAY

        /**<summary>Show an item, with some data and buttons to delete, show and modify.</summary>*/
        private void Display(Element element)
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
            GUILayout.Label(element.ID.ToString(), valuesStyle);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Name

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Name: ");
            GUILayout.Label(element.Name, valuesStyle);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Show

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Show"))
            {
                Show.Window(element);
            }

            #endregion

            #region Modify

            if (GUILayout.Button("Modify"))
            {
                Modifier.Window(_elementDB, element);
            }

            #endregion

            #region Delete

            _deleted = GUILayout.Button("Delete") ? element : null;            

            #endregion
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            
        }
        
        #endregion
    }
}