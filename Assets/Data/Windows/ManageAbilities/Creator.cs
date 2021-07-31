using Data.Database;
using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageAbilities
{
    public class Creator : EditorWindow
    {
        
        private static EditorWindow _window;

        private static Ability _ability;
        private static Abilities _abilityDB;
        
        private Vector2 _scroll;
        
        public static void Window(Abilities abilityDB)
        {
            _abilityDB = abilityDB;

            _window = GetWindow<Creator>();
            _window.maxSize = new Vector2(450, 700);
            _window.minSize = new Vector2(450, 600);

            _ability = new Ability(_abilityDB.Count);
        }

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

        private static void Add()
        {
            Undo.RecordObject(_abilityDB, "Ability Modify");
            _abilityDB.Add(_ability);
            EditorUtility.SetDirty(_abilityDB);
            _window.Close();
        }
        
    }
}