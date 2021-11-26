using Data.Database;
using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageElements
{
    /**<summary>System that generates a new window to create.</summary>*/
    public class Creator : EditorWindow
    {
        
        /**<summary>The new window to generate.</summary>*/
        private static EditorWindow _window;

        /**<summary>Empty item.</summary>*/
        private static Element _element;
        /**<summary>Item DB.</summary>*/
        private static Elements _elementDB;
        
        /**<summary>For move the list with the DB items.</summary>*/
        private Vector2 _scroll;
        
        /**<summary>Create the window.</summary>*/
        public static void Window(Elements elementDB)
        {
            _elementDB = elementDB;

            _window = GetWindow<Creator>();
            Display.Window(_window);

            _element = new Element(_elementDB.Count);
        }

        /**<summary>Show the window.</summary>*/
        public void OnGUI()
        {
            
            _scroll = EditorGUILayout.BeginScrollView(_scroll, GUILayout.MinHeight(5),
                GUILayout.MinHeight(position.height));
            EditorGUILayout.BeginVertical("Box");
            
            Display.Displayed(_element);
            EditorGUI.BeginDisabledGroup(Validator.Validate(_element));
            if (GUILayout.Button("Confirm")) { Add(); }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            EditorGUI.EndDisabledGroup();

        }

        /**<summary>Add a item.</summary>*/
        private static void Add()
        {
            Undo.RecordObject(_elementDB, "Element Modify");
            _elementDB.Add(_element);
            EditorUtility.SetDirty(_elementDB);
            _window.Close();
        }
        
    }
}