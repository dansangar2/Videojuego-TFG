using Data.Database;
using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageCharacters
{
    public class Creator : EditorWindow
    {
        
        private static EditorWindow _window;

        private static Character _character;
        private static Characters _characterDB;
        
        private Vector2 _scroll;
        
        public static void Window(Characters characterDB)
        {
            _characterDB = characterDB;

            _window = GetWindow<Creator>();
            Display.Window(_window);

            _character = new Character(_characterDB.Count);
        }

        public void OnGUI()
        {
            
            _scroll = EditorGUILayout.BeginScrollView(_scroll, GUILayout.MinHeight(5),
                GUILayout.MinHeight(position.height));
            EditorGUILayout.BeginVertical("Box");
            
            Display.Displayed(_character);
            EditorGUI.BeginDisabledGroup(Validator.Validate(_character));
            if (GUILayout.Button("Confirm")) { Add(); }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            EditorGUI.EndDisabledGroup();

        }

        private static void Add()
        {
            Undo.RecordObject(_characterDB, "Character Modify");
            _characterDB.Add(_character);
            EditorUtility.SetDirty(_characterDB);
            _window.Close();
        }
        
    }
}