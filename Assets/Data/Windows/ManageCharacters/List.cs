using System;
using Data.Database;
using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageCharacters
{
    [CustomEditor(typeof(Characters))]
    public class List: Editor
    {
        private Characters _characterDB;
        private string _searchByName = "";
        private bool _canSearch;
        private Character _deleted;

        private void OnEnable()
        {
            _characterDB = (Characters)target;
        }

        #region INSPECTOR

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