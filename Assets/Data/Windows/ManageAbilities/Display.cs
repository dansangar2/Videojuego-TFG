using System;
using Entities;
using Enums;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageAbilities
{
    public static class Display
    {
        private static readonly GUILayoutOption[] Options = { GUILayout.MaxWidth(150f), GUILayout.MinWidth(20f) };
        private static readonly GUIStyle TextAreaStyle = new GUIStyle(GUI.skin.textArea) {wordWrap = true};
        private static AbilityType _key;
        
        public static void Displayed(Ability item, bool readOnly = false)
        {
            //if (readOnly) DisplayedReadOnly(item);
            //else
            DisplayedReadWrite(item);
        }
        
        public static void DisplayedReadWrite(Ability item)
        {

            #region Options

            GUIStyle textAreaStyle = new GUIStyle(GUI.skin.textArea) {wordWrap = true};
            
            #endregion
            
            #region ID

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("ID: ");
            GUILayout.Label(item.ID.ToString(), Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Name

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Name: ");
            item.Name = EditorGUILayout.TextField(item.Name, Options);
            EditorGUILayout.EndHorizontal();

            #endregion
            
            
/*
            #region Type

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Type: ");
            item.Type = (AbilityType)EditorGUILayout.EnumPopup(item.Type, Options);
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
            item.Description = EditorGUILayout.TextArea(item.Description, textAreaStyle, GUILayout.MinHeight(100));

            #endregion

            #region Strengths

            byte size = (byte)item.GetMultiplicityCount();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Strengths: " + item.GetMultiplicityCount());
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _key = (AbilityType)EditorGUILayout.EnumPopup(_key, Options);
            if (GUILayout.Button("+", Options) && size < Enum.GetValues(typeof(AbilityType)).Length)
                item.AddMultiplicityTo(_key, 1f);
            EditorGUILayout.EndHorizontal();

            foreach (AbilityType elem in item.GetMultiplicityAbilitys())
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(elem + ": ", Options);
                item.AddMultiplicityTo(elem, EditorGUILayout.FloatField(item.GetMultiplicityOf(elem), Options));
                if (GUILayout.Button("-", Options)) item.DeleteMultiplicity(elem);
                EditorGUILayout.EndHorizontal();
            }

            #endregion
            */

            #region Formula

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Formula: ");
            item.Formula = EditorGUILayout.TextField(item.Formula, Options);
            EditorGUILayout.EndHorizontal();

            #endregion
            
        }
    }
}