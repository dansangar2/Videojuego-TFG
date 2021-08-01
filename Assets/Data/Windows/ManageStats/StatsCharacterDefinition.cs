using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageStats
{
    /**<summary>This window shows the formulas for generate Character stats.</summary>*/
    public class StatsCharacterDefinition : EditorWindow
    {

        //Database Controller

        private static EditorWindow _window;
        private readonly GUILayoutOption[] _options = {GUILayout.MaxWidth(100f), GUILayout.MinWidth(20f)};
        private readonly GUILayoutOption[] _options2 = {GUILayout.MaxWidth(500f), GUILayout.MinWidth(20f)};

        public static void Window()
        {
            _window = GetWindow<StatsCharacterDefinition>();
            _window.maxSize = new Vector2(640, 200);
            _window.minSize = new Vector2(640, 200);
        }

        public void OnGUI()
        {
            
            GUILayout.Label("Stats Formula: \n", _options);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("For MBP-AGI: ", _options);
            GUILayout.Label("FLOOR(Level*Base + Plus)*Rate + Flat)", _options2);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("For REB: ", _options);
            GUILayout.Label("MIN(MEN/((MaxLevel*Base + Plus) * Rate), Max)", _options2);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("For REK: ", _options);
            GUILayout.Label("MIN(SPI/((MaxLevel*Base + Plus) * Rate), Max)", _options2);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("For RXB: ", _options);
            GUILayout.Label("MEN/((MaxLevel*Base + Plus) * Rate)*Yes", _options2);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("For RXK: ", _options);
            GUILayout.Label("SPI/((MaxLevel*Base + Plus) * Rate)*Yes", _options2);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("For EXP: ", _options);
            GUILayout.Label("Base*(Level - 1)^(0.9+(Rate/250))*Level*(Level+1)/((6+Level^2)/50/Flat)+(Level-1)*Plus", _options2);
            EditorGUILayout.EndHorizontal();
            
            if (GUILayout.Button("Close")) _window.Close();

        }
    }
}