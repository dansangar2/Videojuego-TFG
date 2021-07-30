using Data.Database;
using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageCharacters
{
    public class Modifier : EditorWindow
    {
        private static EditorWindow _window;
        private static Character _character;
        private static Characters _characterDB;
        
        private Vector2 _scroll;

        
        public static void Window(Characters characterDB, Character character)
        {
            _characterDB = characterDB;
            
            _window = GetWindow<Modifier>();
            _window.maxSize = new Vector2(1000, 600);
            _window.minSize = new Vector2(900, 600);
            _character = new Character(character);
            
        }
        
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

        private static void Modify()
        {
            Undo.RecordObject(_characterDB, "Character Modify");
            _characterDB.Modify(_character);
            EditorUtility.SetDirty(_characterDB);
            _window.Close();
        }

    }
}
