using System;
using Data.Database;
using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageAbilities
{
    /**<summary>System that list the DB items.</summary>*/
    [CustomEditor(typeof(Abilities))]
    public class List: Editor
    {
        /**<summary>Item DB.</summary>*/
        private Abilities _abilityDB;
        /**<summary>Item to search by ID or name.</summary>*/
        private string _searchByName = "";
        /**<summary>Check if _searchByName isn't null or empty.</summary>*/
        private bool _canSearch;
        /**<summary>Item to delete.</summary>*/
        private Ability _deleted;

        /**<summary>Initialize the DB when the file is clicked.</summary>*/
        private void OnEnable()
        {
            _abilityDB = (Abilities)target;
        }

        #region INSPECTOR

        
        /**<summary>List all items of the DB, with the filter and creator.</summary>*/
        public override void OnInspectorGUI()
        {
            if (!_abilityDB) return;
            
            #region Total

            EditorGUILayout.BeginHorizontal("Box");
            GUILayout.Label("Ability created: " + _abilityDB.Count);
            EditorGUILayout.EndHorizontal();

            #endregion
            
            #region Search

            if (_abilityDB.Count != 0)
            {
                EditorGUILayout.BeginHorizontal("Box"); 
                GUILayout.Label("Search: ");
                _searchByName = GUILayout.TextField(_searchByName); 
                EditorGUILayout.EndHorizontal();
            }

            #endregion

            #region Manage

            if (GUILayout.Button("Add Ability"))
            {
                Creator.Window(_abilityDB);
            }
            _canSearch = !String.IsNullOrEmpty(_searchByName);
            
            foreach (Ability ability in _abilityDB.All)
            {
                if (_canSearch)
                {
                    if (ability.Name == _searchByName || ability.Name.Contains(_searchByName) || ability.ID.ToString() == _searchByName)
                    {
                        Display(ability);
                    }
                }
                else Display(ability);

                if (_deleted != null)
                {
                    _abilityDB.Remove(_deleted.ID);
                        
                }
            }

            #endregion
        }

        #endregion
        
        #region DISPLAY

        /**<summary>Show an item, with some data and buttons to delete, show and modify.</summary>*/
        private void Display(Ability ability)
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
            GUILayout.Label(ability.ID.ToString(), valuesStyle);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Name

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Name: ");
            GUILayout.Label(ability.Name, valuesStyle);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Show

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Show"))
            {
                Show.Window(ability);
            }

            #endregion

            #region Modify

            if (GUILayout.Button("Modify"))
            {
                Modifier.Window(_abilityDB, ability);
            }

            #endregion

            #region Delete

            _deleted = GUILayout.Button("Delete") ? ability : null;            

            #endregion
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            
        }
        
        #endregion
    }
}