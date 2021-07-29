using System;
using Data.Database;
using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageElements
{
    [CustomEditor(typeof(Elements))]
    public class List: Editor
    {
        private Elements _elementDB;
        private string _searchByName = "";
        private bool _canSearch;
        private Element _deleted;

        private void OnEnable()
        {
            _elementDB = (Elements)target;
        }

        #region INSPECTOR

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