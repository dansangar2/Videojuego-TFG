using Data.Database;
using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageAbilities
{
    public class Modifier : EditorWindow
    {
        private static EditorWindow _window;
        private static Ability _ability;
        private static Abilities _abilityDB;
        
        private Vector2 _scroll;

        
        public static void Window(Abilities elementDB, Ability element)
        {
            _abilityDB = elementDB;
            
            _window = GetWindow<Modifier>();
            _window.maxSize = new Vector2(350, 500);
            _window.minSize = new Vector2(300, 300);
            _ability = new Ability(element);
            
        }
        
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

        private static void Modify()
        {
            Undo.RecordObject(_abilityDB, "Ability Modify");
            _abilityDB.Modify(_ability);
            EditorUtility.SetDirty(_abilityDB);
            _window.Close();
        }

    }
}
