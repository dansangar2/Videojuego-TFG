using Data.Windows.ManageStats;
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
        private static readonly string[] Elements = GameData.ElementDB.Names;
        private static bool _haveElement;
        private static int _index;
        
        public static void Displayed(Ability item, bool readOnly = false)
        {
            if (readOnly) DisplayedReadOnly(item);
            else DisplayedReadWrite(item);
        }
        
        private static void DisplayedReadWrite(Ability item)
        {

            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            
            #region ID

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("ID: ", Options);
            GUILayout.Label(item.ID.ToString(), Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Type

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Type: ", Options);
            item.Type = (AbilityType)EditorGUILayout.EnumPopup(item.Type, Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            
            #region Name

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Name: ", Options);
            item.Name = EditorGUILayout.TextField(item.Name, Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Range

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Range: ", Options);
            item.Range = (TargetRange)EditorGUILayout.EnumPopup(item.Range, Options);
            EditorGUILayout.EndHorizontal();

            #endregion
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            
            #region Max Level

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Max Level: ", Options);
            item.MaxLevel = EditorGUILayout.IntField(item.MaxLevel, Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Target

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Target: ", Options);
            item.Target = (TargetType)EditorGUILayout.EnumPopup(item.Target, Options);
            EditorGUILayout.EndHorizontal();

            #endregion
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            
            #region Icon

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Icon: ", Options);
            item.Icon = (Sprite)EditorGUILayout.ObjectField(item.Icon, typeof(Sprite), true, Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Hits

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Hits: ", Options);
            item.Hits = EditorGUILayout.IntField(item.Hits, Options);
            EditorGUILayout.EndHorizontal();

            #endregion
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();

            #region Cost

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Cost: ", Options);
            item.Cost = EditorGUILayout.IntField(item.Cost, Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Number Of Target

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Targets number: ", Options);
            item.NumberOfTarget = EditorGUILayout.IntField(item.NumberOfTarget, Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();

            #region Can Repeat Random Target?

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Can repeat random?: ");
            item.CanRepeatRandomTarget = EditorGUILayout.Toggle(item.CanRepeatRandomTarget, Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Have element?

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Have Element?: ");
            _haveElement = EditorGUILayout.Toggle(_haveElement, Options);
            if (_haveElement)
            {
                if (item.Element == null) _index = 0; 
                _index = EditorGUILayout.Popup(_index, Elements, Options);
            }
            else
            {
                _index = -1;
                GUILayout.Label("NULL", Options);
            }
            _haveElement = item.Element != null;
            item.ElementID = _index;
            EditorGUILayout.EndHorizontal();

            #endregion
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            
            #region Description

            GUILayout.Label("Description: ");
            item.Description = EditorGUILayout.TextArea(item.Description, TextAreaStyle, GUILayout.MinHeight(100));

            #endregion
            
            #region Stats

            StatsGeneratorWindow.GenerateStats(item, Options);
            if (GUILayout.Button("See Stats by Level")) AbilityByLevelHelp.Window(item);

            #endregion
            
            #region Formula

            FormulaDescription();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Formula: ", Options);
            item.Formula = EditorGUILayout.TextField(item.Formula, GUILayout.MaxWidth(350f), GUILayout.MinWidth(20f));
            EditorGUILayout.EndHorizontal();

            #endregion
            
        }
        
        private static void DisplayedReadOnly(Ability item) 
        {
            
            EditorGUILayout.BeginVertical(); 
            EditorGUILayout.BeginHorizontal();
             
            #region ID
             
            EditorGUILayout.BeginHorizontal(); 
            GUILayout.Label("ID: ", Options); 
            GUILayout.Label(item.ID.ToString(), Options); 
            EditorGUILayout.EndHorizontal();
             
            #endregion
            
            #region Type
             
            EditorGUILayout.BeginHorizontal(); 
            GUILayout.Label("Type: ", Options); 
            GUILayout.Label(item.Type.ToString(), Options); 
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
             
            #region Range
             
            EditorGUILayout.BeginHorizontal(); 
            GUILayout.Label("Range: ", Options); 
            GUILayout.Label(item.Range.ToString(), Options); 
            EditorGUILayout.EndHorizontal();
             
            #endregion
             
            EditorGUILayout.EndHorizontal(); 
            EditorGUILayout.EndVertical();
             
            EditorGUILayout.BeginVertical(); 
            EditorGUILayout.BeginHorizontal();
             
            #region Max Level
             
            EditorGUILayout.BeginHorizontal(); 
            GUILayout.Label("Max Level: ", Options); 
            GUILayout.Label(item.MaxLevel.ToString(), Options); 
            EditorGUILayout.EndHorizontal();
             
            #endregion
             
            #region Target
             
            EditorGUILayout.BeginHorizontal(); 
            GUILayout.Label("Target: ", Options); 
            GUILayout.Label(item.Target.ToString(), Options); 
            EditorGUILayout.EndHorizontal();
             
            #endregion
             
            EditorGUILayout.EndHorizontal(); 
            EditorGUILayout.EndVertical();
             
            EditorGUILayout.BeginVertical(); 
            EditorGUILayout.BeginHorizontal();
             
            #region Icon
             
            EditorGUILayout.BeginHorizontal(); 
            GUILayout.Label("Icon: ", Options); 
            GUILayout.Label(item.Icon.ToString(), Options); 
            EditorGUILayout.EndHorizontal();
             
            #endregion
             
            #region Hits
             
            EditorGUILayout.BeginHorizontal(); 
            GUILayout.Label("Hits: ", Options); 
            GUILayout.Label(item.Hits.ToString(), Options); 
            EditorGUILayout.EndHorizontal();
             
            #endregion
             
            EditorGUILayout.EndHorizontal(); 
            EditorGUILayout.EndVertical();
             
            EditorGUILayout.BeginVertical(); 
            EditorGUILayout.BeginHorizontal();
             
            #region Cost
             
            EditorGUILayout.BeginHorizontal(); 
            GUILayout.Label("Cost: ", Options); 
            GUILayout.Label(item.Cost.ToString(), Options); 
            EditorGUILayout.EndHorizontal();
             
            #endregion
             
            #region Number Of Target
             
            EditorGUILayout.BeginHorizontal(); 
            GUILayout.Label("Targets number: ", Options); 
            GUILayout.Label(item.NumberOfTarget.ToString(), Options); 
            EditorGUILayout.EndHorizontal();
             
            #endregion
             
            EditorGUILayout.EndHorizontal(); 
            EditorGUILayout.EndVertical();
             
            EditorGUILayout.BeginVertical(); 
            EditorGUILayout.BeginHorizontal();
             
            #region Can Repeat Random Target?
             
            EditorGUILayout.BeginHorizontal(); 
            GUILayout.Label("Can repeat random?: ", Options); 
            GUILayout.Label(item.CanRepeatRandomTarget.ToString(), Options); 
            EditorGUILayout.EndHorizontal();
             
            #endregion
             
            #region Have element?
             
            EditorGUILayout.BeginHorizontal(); 
            GUILayout.Label("Have Element?: ", Options); 
            GUILayout.Label(item.Element == null ? "NULL" : item.Element.Name, Options); 
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
             
            #endregion
             
            #region Formula
             
            FormulaDescription();
             
            EditorGUILayout.BeginHorizontal(); 
            GUILayout.Label("Formula: ", Options); 
            GUILayout.Label(item.Formula, GUILayout.MaxWidth(350f), GUILayout.MinWidth(20f)); 
            EditorGUILayout.EndHorizontal();
             
            #endregion
            
        }

         
        #region Formula description
        
        private static void FormulaDescription() 
        { 
            GUILayoutOption[] options = {GUILayout.MaxWidth(200f), GUILayout.MinWidth(20f)};
            
            EditorGUILayout.BeginHorizontal(); 
            GUILayout.Label("The format for to use a stat is {user.stat}. For example {a.atk} (user attack)"); EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("For use the stat of user you must write a.stat, and the target b.stat");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Where \"stat\" is: ");
            EditorGUILayout.EndHorizontal();
                
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("mbp => Max Blood Points", options);
            GUILayout.Label("mkp => Max Karma Points", options);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("atk => Attack", options);
            GUILayout.Label("def => Defense", options);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("spi => Spirit", options);
            GUILayout.Label("men => Mentality", options);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("agi => Agility", options);
            GUILayout.Label("kg => Weight", options);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("lv => Level", options);
            GUILayout.Label("cha => Charge", options);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("cbp => Current Blood Points", options);
            GUILayout.Label("ckp => Current Karma Points", options);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("reb => Rec. Blood plus (x100)", options);
            GUILayout.Label("rek => Rec. Karma plus (x100)", options);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("rxb => Regeneration (x100)", options);
            GUILayout.Label("rxk => Regeneration Karma (x100)", options);
            EditorGUILayout.EndHorizontal();
            
        }

        #endregion
    }
}