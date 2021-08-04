using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageProfiles
{
    public static class Display
    {
        private static readonly GUILayoutOption[] Options = { GUILayout.MaxWidth(150f), GUILayout.MinWidth(20f) };
        //private static readonly GUIStyle TextAreaStyle = new GUIStyle(GUI.skin.textArea) {wordWrap = true};
        
        public static void Window(EditorWindow window)
        {
            window.maxSize = new Vector2(180, 130);
            window.minSize = new Vector2(170, 100);
        }

        public static void Displayed(Profile item, bool readOnly = false)
        {
            if(readOnly) DisplayedReadOnly(item);
            else DisplayedReadWrite(item);        
        }
        
        private static void DisplayedReadWrite(Profile item)
        {

            #region ID

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("ID: ");
            GUILayout.Label(item.ID.ToString(), Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Name

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("*Name: ");
            item.Name = EditorGUILayout.TextField(item.Name, Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Art

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("*Art: ");
            item.Art = (Sprite)EditorGUILayout.ObjectField(item.Art, typeof(Sprite), true, Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            /*
            #region Description

            GUILayout.Label("Description: ");
            item.Description = EditorGUILayout.TextArea(item.Description, TextAreaStyle, GUILayout.MinHeight(100));

            #endregion
            */

        }
        
        private static void DisplayedReadOnly(Profile item)
        {

            #region ID

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("ID: ");
            GUILayout.Label(item.ID.ToString(), Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Name

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("*Name: ");
            GUILayout.Label(item.Name, Options);
            EditorGUILayout.EndHorizontal();

            #endregion
            
            #region Art

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("*Art: ");
            GUILayout.Label(item.Art.name, Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            /*
            #region Description

            GUILayout.Label("Description: ");
            GUILayout.Label(item.Description, textAreaStyle, GUILayout.MinHeight(100));

            #endregion
            */
            
        }
    }
}