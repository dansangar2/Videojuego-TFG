using System.Globalization;
using Entities;
using Enums;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageStatuses
{
    /**<summary>Display for edit and/or show the data of Statuses.</summary>*/
    public static class Display
    {
        /**<summary>Configuration for the spaces to fill.</summary>*/
        private static readonly GUILayoutOption[] Options = { GUILayout.MaxWidth(150f), GUILayout.MinWidth(20f) };
        /**<summary>Configuration for the text area space.</summary>*/
        private static readonly GUIStyle TextAreaStyle = new GUIStyle(GUI.skin.textArea) {wordWrap = true};
        /**<summary>For easy way to show the stats to fill interface.</summary>*/
        private static readonly string[] Stats = { "mbp", "mkp", "atk", "def", "spi", "men", "agi", "reb", "rek", "rxb", "rxk" };
        /**<summary>Check if the status remove all other statuses.</summary>*/
        private static bool _all;
        /**<summary>Index that save the ID of status to quit.</summary>*/
        private static int _index;
        
        /**<summary>Generate the window with the size necessary to display the window.</summary>*/
        public static void Window(EditorWindow window)
        {
            window.maxSize = new Vector2(600, 600);
            window.minSize = new Vector2(550, 500);
        }
        
        /**<summary>Generate the window interface, adapted to the entity.
        With the data of item.</summary>*/
        public static void Displayed(Status item, bool readOnly = false)
        {
            if (readOnly) DisplayedReadOnly(item);
            else DisplayedReadWrite(item);
        }
        
        /**<summary>You can edit.</summary>*/
        private static void DisplayedReadWrite(Status item)
        {

            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            
            #region ID

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("ID: ", Options);
            GUILayout.Label(item.ID.ToString(), Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Icon

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Icon: ", Options);
            item.Icon = (Sprite)EditorGUILayout.ObjectField(item.Icon, typeof(Sprite), true, Options);
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

            #region Effect

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Effect: ", Options);
            item.Effect = (EffectType)EditorGUILayout.EnumPopup(item.Effect, Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();

            #region Quit When Finish

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Quit when the battle finish?: ");
            item.QuitWhenFinish = EditorGUILayout.Toggle(item.QuitWhenFinish, Options);
            EditorGUILayout.EndHorizontal();

            #endregion
            
            #region Hit quit

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Hit quit rate: ");
            item.QuitByHitRate = EditorGUILayout.FloatField(item.QuitByHitRate, Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            
            #region Description

            GUILayout.Label("Description: ");
            item.Description = EditorGUILayout.TextArea(item.Description, TextAreaStyle, GUILayout.MinHeight(100));

            #endregion

            #region Stats Generator

            EditorGUILayout.BeginHorizontal();
            
            GUILayout.Label("Increment Rate power (stat*rate)");
            
            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < item.IncrementPower.Length; i+=2)
            {
                
                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(Stats[i] + ": ",Options);
                item.IncrementPower[i] = EditorGUILayout.FloatField(item.IncrementPower[i], Options);
                EditorGUILayout.EndHorizontal();
                
                if(!(i + 2 > item.IncrementPower.Length))
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(Stats[i + 1] + ": ", Options);
                    item.IncrementPower[i + 1] = EditorGUILayout.FloatField(item.IncrementPower[i + 1], Options);
                    EditorGUILayout.EndHorizontal();
                }
                
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }

            #endregion
            
            #region Statuses To quit
            
            EditorGUILayout.BeginHorizontal();

            _all = item.AllToQuitIsMark;
            
            GUILayout.Label("All?: ", Options);
            _all = EditorGUILayout.Toggle(_all, Options);

            EditorGUILayout.EndHorizontal();
            
            if(_all) item.AddStatusToQuit(-1);
            else
            {
                EditorGUILayout.BeginHorizontal();
                item.RemoveStatusToQuit(-1);
                if (GameData.StatusDB.Count > 0)
                {

                    GUILayout.Label("Status to quit: ", Options);
                    _index = EditorGUILayout.Popup(_index, GameData.StatusDB.Names, Options);
                    if (GUILayout.Button("+", Options)) item.AddStatusToQuit(_index);

                }

                EditorGUILayout.EndHorizontal();

                foreach (Status s in item.StatusToQuit)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("Name: ", Options);
                    GUILayout.Label(s.Name + ": ", Options);
                    GUILayout.Label("Effect: ", Options);
                    GUILayout.Label(s.Effect + ": ", Options);
                    if (GUILayout.Button("-", Options)) item.RemoveStatusToQuit(s.ID);
                    EditorGUILayout.EndHorizontal();
                }
            }
            #endregion
            
        }
        
        /**<summary>You can't edit.</summary>*/
        private static void DisplayedReadOnly(Status item) 
        {
            
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            
            #region ID

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("ID: ", Options);
            GUILayout.Label(item.ID.ToString(), Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            #region Icon

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Icon: ", Options);
            GUILayout.Label(item.Icon.name, Options);
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

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            #region Effect

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Effect: ", Options);
            GUILayout.Label(item.Effect.ToString(), Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();

            #region Quit When Finish

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Quit when the battle finish?: ");
            GUILayout.Label(item.QuitWhenFinish.ToString(), Options);
            EditorGUILayout.EndHorizontal();

            #endregion
            
            #region Hit quit

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Hit quit rate: ");
            GUILayout.Label(item.QuitByHitRate.ToString(CultureInfo.InvariantCulture), Options);
            EditorGUILayout.EndHorizontal();

            #endregion

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            
            #region Description

            GUILayout.Label("Description: ");
            GUILayout.Label(item.Description, TextAreaStyle, GUILayout.MinHeight(100));

            #endregion

            #region Stats Generator

            EditorGUILayout.BeginHorizontal();
            
            GUILayout.Label("Increment Rate power (stat*rate)");
            
            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < item.IncrementPower.Length; i+=2)
            {
                
                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(Stats[i] + ": ",Options);
                GUILayout.Label(item.IncrementPower[i].ToString(CultureInfo.InvariantCulture), Options);
                EditorGUILayout.EndHorizontal();
                
                if(!(i + 2 > item.IncrementPower.Length))
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(Stats[i + 1] + ": ", Options);
                    GUILayout.Label(item.IncrementPower[i + 1].ToString(CultureInfo.InvariantCulture), Options);
                    EditorGUILayout.EndHorizontal();
                }
                
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }

            #endregion
            
            #region Statuses To quit

            foreach (Status s in item.StatusToQuit) 
            { 
                EditorGUILayout.BeginHorizontal(); 
                GUILayout.Label("Name: ", Options); 
                GUILayout.Label(s.Name + ": ", Options); 
                GUILayout.Label("Effect: ", Options); 
                GUILayout.Label(s.Effect + ": ", Options);
                EditorGUILayout.EndHorizontal();
            }
            
            #endregion
            
        }
    }
}