using Data.Database;
using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageCharacters
{
    /**<summary>System that generates a new window to modify.</summary>*/
    public class Modifier : EditorWindow
    {
        /**<summary>The new window to generate.</summary>*/
        private static EditorWindow _window;
        
        /**<summary>Empty item.</summary>*/
        private static Character _character;
        /**<summary>Item DB.</summary>*/
        private static Characters _characterDB;
        
        /**<summary>For move the list with the DB items.</summary>*/
        private Vector2 _scroll;

        
        /**<summary>Create the window.</summary>*/
        public static void Window(Characters characterDB, Character character)
        {
            _characterDB = characterDB;

            
            _window = GetWindow<Modifier>();
            Display.Window(_window);
            _character = new Character(character);
            
        }
        
        /**<summary>Show the window.</summary>*/
        public void OnGUI()
        {
            
            _scroll = EditorGUILayout.BeginScrollView(_scroll, GUILayout.MinHeight(5), GUILayout.MinHeight(position.height));
            EditorGUILayout.BeginVertical("Box");
            
            Display.Displayed(_character);
            EditorGUI.BeginDisabledGroup(Validator.Validate(_character));
            if (GUILayout.Button("Confirm")) { Modify(); }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            EditorGUI.EndDisabledGroup();

        }

        /**<summary>Modify a item.</summary>*/
        private static void Modify()
        {
            Undo.RecordObject(_characterDB, "Character Modify");
            _characterDB.Modify(_character);
            EditorUtility.SetDirty(_characterDB);
            _window.Close();
        }
    }
}
