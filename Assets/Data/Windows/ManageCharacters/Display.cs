﻿using Data.Windows.ManageStats;
using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageCharacters
{
    public static class Display
    {
        private static readonly GUILayoutOption[] Options = { GUILayout.MaxWidth(150f), GUILayout.MinWidth(20f) };
        private static readonly GUIStyle TextAreaStyle = new GUIStyle(GUI.skin.textArea) {wordWrap = true};
        private static int _index;
        private static int _melee;
        private static int _long;
        private static int _ability;
        private static int _need;
        private static int _level;

        public static void Displayed(Character item, bool readOnly = false)
        {
            if (readOnly) DisplayedReadOnly(item);
            else DisplayedReadWrite(item);
        }
        
        private static void DisplayedReadWrite(Character item)
        {
            
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            
            #region ID

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("ID: ", Options);
            GUILayout.Label(item.ID.ToString(), Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Initial Level

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Initial Level: ", Options);
            item.Level = EditorGUILayout.IntField(item.Level, Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Element

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Element: ", Options);
            _index = EditorGUILayout.Popup(_index, GameData.ElementDB.Names, Options);
            item.ElementID = _index;
            EditorGUILayout.EndHorizontal();
            
            #endregion

            #region Model

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Model: ", Options);
            item.Model = (Mesh)EditorGUILayout.ObjectField(item.Model, typeof(Mesh), true, Options);
            EditorGUILayout.EndHorizontal();

            #endregion
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            
            #region Name

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("*Name: ", Options);
            item.Name = EditorGUILayout.TextField(item.Name, Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Max Level

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Max Level: ", Options);
            item.MaxLevel = EditorGUILayout.IntField(item.MaxLevel, Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Face

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Face: ", Options);
            item.Face = (Sprite)EditorGUILayout.ObjectField(item.Face, typeof(Sprite), true, Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Art

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Art: ", Options);
            item.Art = (Sprite)EditorGUILayout.ObjectField(item.Art, typeof(Sprite), true, Options);
            EditorGUILayout.EndHorizontal();        

            #endregion
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            
            #region Description

            GUILayout.Label("Description: ");
            item.Description = EditorGUILayout.TextArea(item.Description, TextAreaStyle, GUILayout.MinHeight(100));

            #endregion

            #region Stats

            StatsGeneratorWindow.CharacterStats(item, Options);
            if (GUILayout.Button("See Stats by Level")) StatsByLevelHelp.Window(item);

            #endregion
            
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            
            #region Melee Attack

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Melee Attack: ", Options);
            _melee = EditorGUILayout.Popup(_melee,GameData.AbilityDB.Names, Options);
            item.MeleeAttackID = _melee;
            EditorGUILayout.EndHorizontal();
            
            #endregion
            
            #region Long Attack
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Long Attack: ", Options);
            _long = EditorGUILayout.Popup(_long,GameData.AbilityDB.Names, Options);
            item.LongAttackID = _long;
            EditorGUILayout.EndHorizontal();
            
            #endregion
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Total of abilities: " + item.AllDataAbilities.Length);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            #region Abilities

            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Ability", Options);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Initial Level", Options);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Need to unlock", Options);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("To add", Options);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            _ability = EditorGUILayout.Popup(_ability, GameData.AbilityDB.Names, Options);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            _level = EditorGUILayout.IntField(_level, Options);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            _need = EditorGUILayout.IntField(_need, Options);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+", Options)) item.AddAbility(_ability, _need, _level);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            
            foreach (SpecialAbility abi in item.AllDataAbilities)
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(abi.SpAbility.Name + ": ", Options);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                int level = EditorGUILayout.IntField(abi.Level, Options);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                int needLevel = EditorGUILayout.IntField(abi.NeedLevel, Options);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                item.AddAbility(abi.SpAbility.ID, needLevel, level);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("-", Options)) item.RemoveAbility(abi.SpAbility.ID);
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
            
            #endregion
            
        }
        
        private static void DisplayedReadOnly(Character item)
        {
            
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            
            #region ID

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("ID: ", Options);
            GUILayout.Label(item.ID.ToString(), Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Initial Level

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Initial Level: ", Options);
            GUILayout.Label(item.Level.ToString(), Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Element

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Element: ", Options);
            GUILayout.Label(item.Element.Name, Options);
            EditorGUILayout.EndHorizontal();
            
            #endregion

            #region Model

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Model: ", Options);
            GUILayout.Label(item.Model.name, Options);
            EditorGUILayout.EndHorizontal();

            #endregion
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            
            #region Name

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Name: ", Options);
            GUILayout.Label(item.Name, Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Max Level

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Max Level: ", Options);
            GUILayout.Label(item.MaxLevel.ToString(), Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Face

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Face: ", Options);
            GUILayout.Label(item.Face.name, Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Art

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Art: ", Options);
            GUILayout.Label(item.Art.name, Options);
            EditorGUILayout.EndHorizontal();        

            #endregion
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            
            #region Description

            GUILayout.Label("Description: ");
            GUILayout.Label(item.Description, TextAreaStyle, GUILayout.MinHeight(100));

            #endregion

            #region Stats

            StatsGeneratorWindow.Display(item, Options);
            if (GUILayout.Button("See Stats by Level")) StatsByLevelHelp.Window(item);

            #endregion
            
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            
            #region Melee Attack

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Melee Attack: ", Options);
            GUILayout.Label(item.MeleeAttack.Name, Options);
            EditorGUILayout.EndHorizontal();
            
            #endregion
            
            #region Long Attack
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Long Attack: ", Options);
            GUILayout.Label(item.LongAttack.Name, Options);
            EditorGUILayout.EndHorizontal();
            
            #endregion
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Total of abilities: " + item.AllDataAbilities.Length);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            #region Abilities

            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Ability", Options);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Initial Level", Options);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Need to unlock", Options);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            
            
            foreach (SpecialAbility abi in item.AllDataAbilities)
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(abi.SpAbility.Name + ": ", Options);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(abi.Level.ToString(), Options);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(abi.NeedLevel.ToString(), Options);
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
            
            #endregion
            
        }
    }
}