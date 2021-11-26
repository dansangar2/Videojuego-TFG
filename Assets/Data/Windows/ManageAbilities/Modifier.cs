using Data.Database;
using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageAbilities
{
    /**<summary>System that generates a new window to modify.</summary>*/
    public class Modifier : EditorWindow
    {
        /**<summary>The new window to generate.</summary>*/
        private static EditorWindow _window;
        
        /**<summary>Empty item.</summary>*/
        private static Ability _ability;
        /**<summary>Item DB.</summary>*/
        private static Abilities _abilityDB;
        
        /**<summary>For move the list with the DB items.</summary>*/
        private Vector2 _scroll;

        
        /**<summary>Create the window.</summary>*/
        public static void Window(Abilities elementDB, Ability element)
        {
            _abilityDB = elementDB;
            
            _window = GetWindow<Modifier>();
            Display.Window(_window);
            _ability = new Ability(element);
            
        }
        
        /**<summary>Show the window.</summary>*/
        public void OnGUI()
        {
            
            _scroll = EditorGUILayout.BeginScrollView(_scroll, GUILayout.MinHeight(5), GUILayout.MinHeight(position.height));
            EditorGUILayout.BeginVertical("Box");
            
            Display.Displayed(_ability);
            EditorGUI.BeginDisabledGroup(Validator.Validate(_ability));
            if (GUILayout.Button("Confirm")) { Modify(); }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            EditorGUI.EndDisabledGroup();

        }

        /**<summary>Modify a item.</summary>*/
        private static void Modify()
        {
            Undo.RecordObject(_abilityDB, "Ability Modify");
            _abilityDB.Modify(_ability);
            EditorUtility.SetDirty(_abilityDB);
            _window.Close();
        }
    }
}
