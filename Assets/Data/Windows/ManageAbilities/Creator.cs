using Data.Database;
using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageAbilities
{
    /**<summary>System that generates a new window to create.</summary>*/
    public class Creator : EditorWindow
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
        public static void Window(Abilities abilityDB)
        {
            _abilityDB = abilityDB;

            _window = GetWindow<Creator>();
            Display.Window(_window);

            _ability = new Ability(_abilityDB.Count);
        }

        /**<summary>Show the window.</summary>*/
        public void OnGUI()
        {
            
            _scroll = EditorGUILayout.BeginScrollView(_scroll, GUILayout.MinHeight(5),
                GUILayout.MinHeight(position.height));
            EditorGUILayout.BeginVertical("Box");
            
            Display.Displayed(_ability);
            EditorGUI.BeginDisabledGroup(Validator.Validate(_ability));
            if (GUILayout.Button("Confirm")) { Add(); }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            EditorGUI.EndDisabledGroup();

        }

        /**<summary>Add a item.</summary>*/
        private static void Add()
        {
            Undo.RecordObject(_abilityDB, "Ability Modify");
            _abilityDB.Add(_ability);
            EditorUtility.SetDirty(_abilityDB);
            _window.Close();
        }
       
    }
}