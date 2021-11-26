using System;
using Data.Database;
using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageCharacters
{
    /**<summary>System that list the DB items.</summary>*/
    [CustomEditor(typeof(Characters))]
    public class List: Editor
    {
        /**<summary>Item DB.</summary>*/
        private Characters _characterDB;
        /**<summary>Item to search by ID or name.</summary>*/
        private string _searchByName = "";
        /**<summary>Check if _searchByName isn't null or empty.</summary>*/
        private bool _canSearch;
        /**<summary>Item to delete.</summary>*/
        private Character _deleted;

        /**<summary>Initialize the DB when the file is clicked.</summary>*/
        private void OnEnable()
        {
            _characterDB = (Characters)target;
        }

        #region INSPECTOR

        /**<summary>List all items of the DB, with the filter and creator.</summary>*/
        public override void OnInspectorGUI()
        {
            if (!_characterDB) return;
            
            #region Total

            EditorGUILayout.BeginHorizontal("Box");
            GUILayout.Label("Character created: " + _characterDB.Count);
            EditorGUILayout.EndHorizontal();

            #endregion
            
            #region Search

            if (_characterDB.Count != 0)
            {
                EditorGUILayout.BeginHorizontal("Box"); 
                GUILayout.Label("Search: ");
                _searchByName = GUILayout.TextField(_searchByName); 
                EditorGUILayout.EndHorizontal();
            }

            #endregion

            #region Manage

            if (GUILayout.Button("Add Character"))
            {
                Creator.Window(_characterDB);
            }
            _canSearch = !String.IsNullOrEmpty(_searchByName);
            
            foreach (Character character in _characterDB.All)
            {
                if (_canSearch)
                {
                    if (character.Name == _searchByName || character.Name.Contains(_searchByName) || character.ID.ToString() == _searchByName)
                    {
                        Display(character);
                    }
                }
                else Display(character);

                if (_deleted != null)
                {
                    _characterDB.Remove(_deleted.ID);
                        
                }
            }

            #endregion
        }

        #endregion
        
        #region DISPLAY

        /**<summary>Show an item, with some data and buttons to delete, show and modify.</summary>*/
        private void Display(Character character)
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
            GUILayout.Label(character.ID.ToString(), valuesStyle);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Name

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Name: ");
            GUILayout.Label(character.Name, valuesStyle);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Show

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Show"))
            {
                Show.Window(character);
            }

            #endregion

            #region Modify

            if (GUILayout.Button("Modify"))
            {
                Modifier.Window(_characterDB, character);
            }

            #endregion

            #region Delete

            _deleted = GUILayout.Button("Delete") ? character : null;            

            #endregion
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            
        }
        
        #endregion
    }
}