using System;
using Data.Database;
using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageProfiles
{
    [CustomEditor(typeof(Profiles))]
    public class List: Editor
    {
        private Profiles _profileDB;
        private string _searchByName = "";
        private bool _canSearch;
        private Profile _deleted;

        private void OnEnable()
        {
            _profileDB = (Profiles)target;
        }

        #region INSPECTOR

        public override void OnInspectorGUI()
        {
            if (!_profileDB) return;
            
            #region Total

            EditorGUILayout.BeginHorizontal("Box");
            GUILayout.Label("Profile created: " + _profileDB.Count);
            EditorGUILayout.EndHorizontal();

            #endregion
            
            #region Search

            if (_profileDB.Count != 0)
            {
                EditorGUILayout.BeginHorizontal("Box"); 
                GUILayout.Label("Search: ");
                _searchByName = GUILayout.TextField(_searchByName); 
                EditorGUILayout.EndHorizontal();
            }

            #endregion

            #region Manage

            if (GUILayout.Button("Add Profile"))
            {
                Creator.Window(_profileDB);
            }
            _canSearch = !String.IsNullOrEmpty(_searchByName);
            
            foreach (Profile profile in _profileDB.All)
            {
                if (_canSearch)
                {
                    if (profile.Name == _searchByName || profile.Name.Contains(_searchByName) || profile.ID.ToString() == _searchByName)
                    {
                        Display(profile);
                    }
                }
                else Display(profile);

                if (_deleted != null)
                {
                    _profileDB.Remove(_deleted.ID);
                        
                }
            }

            #endregion
        }

        #endregion
        
        #region DISPLAY

        private void Display(Profile profile)
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
            GUILayout.Label(profile.ID.ToString(), valuesStyle);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Name

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Name: ");
            GUILayout.Label(profile.Name, valuesStyle);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Show

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Show"))
            {
                Show.Window(profile);
            }

            #endregion

            #region Modify

            if (GUILayout.Button("Modify"))
            {
                Modifier.Window(_profileDB, profile);
            }

            #endregion

            #region Delete

            _deleted = GUILayout.Button("Delete") ? profile : null;            

            #endregion
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            
        }
        
        #endregion
    }
}