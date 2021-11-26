using System;
using System.Globalization;
using Entities;
using Enums;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageElements
{
    /**<summary>Display for edit and/or show the data of Elements.</summary>*/
    public static class Display
    {
        /**<summary>Configuration for the spaces to fill.</summary>*/
        private static readonly GUILayoutOption[] Options = { GUILayout.MaxWidth(150f), GUILayout.MinWidth(20f) };
        /**<summary>Configuration for the text area space.</summary>*/
        private static readonly GUIStyle TextAreaStyle = new GUIStyle(GUI.skin.textArea) {wordWrap = true};
        /**<summary>It save the element choose to add to the multiplicity Dictionary.</summary>*/
        private static ElementType _key;

        /**<summary>Generate the window with the size necessary to display the window.</summary>*/
        public static void Window(EditorWindow window)
        {
            window.maxSize = new Vector2(350, 500);
            window.minSize = new Vector2(300, 300);
        }
        
        /**<summary>Generate the window interface, adapted to the entity.
        With the data of item.</summary>*/
        public static void Displayed(Element item, bool readOnly = false)
        {
            if(readOnly) DisplayedReadOnly(item);
            else DisplayedReadWrite(item);        
        }
        
        /**<summary>You can edit.</summary>*/
        private static void DisplayedReadWrite(Element item)
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

            #region Type

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Type: ");
            item.Type = (ElementType)EditorGUILayout.EnumPopup(item.Type, Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Icon

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("*Icon: ");
            item.Icon = (Sprite)EditorGUILayout.ObjectField(item.Icon, typeof(Sprite), true, Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Description

            GUILayout.Label("Description: ");
            item.Description = EditorGUILayout.TextArea(item.Description, TextAreaStyle, GUILayout.MinHeight(100));

            #endregion

            #region Strengths

            byte size = (byte)item.GetMultiplicityCount();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Strengths: " + item.GetMultiplicityCount());
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _key = (ElementType)EditorGUILayout.EnumPopup(_key, Options);
            if (GUILayout.Button("+", Options) && size < Enum.GetValues(typeof(ElementType)).Length)
                item.AddMultiplicityTo(_key, 1f, false);
            EditorGUILayout.EndHorizontal();

            foreach (ElementType elem in item.GetMultiplicityElements())
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(elem + ": ", Options);
                item.AddMultiplicityTo(elem, EditorGUILayout.FloatField(item.GetMultiplicityOf(elem), Options));
                if (GUILayout.Button("-", Options)) item.DeleteMultiplicity(elem);
                EditorGUILayout.EndHorizontal();
            }

            #endregion
            
        }
        
        /**<summary>You can't edit.</summary>*/
        private static void DisplayedReadOnly(Element item)
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

            #region Type

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Type: ");
            GUILayout.Label(item.Type.ToString(), Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Icon

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("*Icon: ");
            GUILayout.Label(item.Icon.name, Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Description

            GUILayout.Label("Description: ");
            GUILayout.Label(item.Description, TextAreaStyle, GUILayout.MinHeight(100));

            #endregion

            #region Strengths

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Strengths: " + item.GetMultiplicityCount());
            EditorGUILayout.EndHorizontal();

            foreach (ElementType elem in item.GetMultiplicityElements())
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(elem + ": ", Options);
                GUILayout.Label(item.GetMultiplicityOf(elem).ToString(CultureInfo.InvariantCulture), Options);
                EditorGUILayout.EndHorizontal();
            }

            #endregion
            
        }
    }
}