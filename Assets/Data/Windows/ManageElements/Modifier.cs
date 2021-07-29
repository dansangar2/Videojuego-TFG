using Data.Database;
using Entities;
using UnityEditor;
using UnityEngine;


namespace Data.Windows.ManageElements
{
    public class Modifier : EditorWindow
    {
        private static EditorWindow _window;
        private static Element _element;
        private static Elements _elementDB;
        
        private Vector2 _scroll;

        
        public static void Window(Elements elementDB, Element element)
        {
            _elementDB = elementDB;
            
            _window = GetWindow<Modifier>();
            _window.maxSize = new Vector2(350, 500);
            _window.minSize = new Vector2(300, 300);
            _element = new Element(element);
            
        }
        
        public void OnGUI()
        {
            
            _scroll = EditorGUILayout.BeginScrollView(_scroll, GUILayout.MinHeight(5), GUILayout.MinHeight(position.height));
            EditorGUILayout.BeginVertical("Box");
            
            Display.Displayed(_element);
            EditorGUI.BeginDisabledGroup(Validator.Validate(_element));
            if (GUILayout.Button("Confirm")) { Modify(); }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            EditorGUI.EndDisabledGroup();

        }

        private static void Modify()
        {
            Undo.RecordObject(_elementDB, "Element Modify");
            _elementDB.Modify(_element);
            EditorUtility.SetDirty(_elementDB);
            _window.Close();
        }

    }
}
