using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageStats
{
    public class StatsAbilityDefinition : EditorWindow
    {

        //Database Controller

        private static EditorWindow _window;
        private readonly GUILayoutOption[] _options = {GUILayout.MaxWidth(100f), GUILayout.MinWidth(20f)};
        private readonly GUILayoutOption[] _options2 = {GUILayout.MaxWidth(500f), GUILayout.MinWidth(20f)};

        public static void Window()
        {
            _window = GetWindow<StatsAbilityDefinition>();
            _window.maxSize = new Vector2(640, 100);
            _window.minSize = new Vector2(640, 100);
        }

        public void OnGUI()
        {
            
            GUILayout.Label("Stats Formula: \n", _options);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("For All except EXP: ", _options);
            GUILayout.Label("Base * Rate^2", _options2);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("For EXP: ", _options);
            GUILayout.Label("Base*(Level - 1)^(0.9+(Rate/250))*Level*(Level+1)/(6+Level^2)/50/Flat)+(Level-1)*Plus", _options2);
            EditorGUILayout.EndHorizontal();
            
            if (GUILayout.Button("Close")) _window.Close();

        }
    }
}