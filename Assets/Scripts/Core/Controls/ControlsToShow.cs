using UnityEngine;
using UnityEngine.UI;

namespace Core.Controls
{
    public class ControlsToShow : MonoBehaviour
    {
        /**<summary>The text slots.</summary>*/
        public Text[] texts;

        /**<summary>The key By ID.</summary>*/
        public int[] keys;
        /**<summary>The description of the key.</summary>*/
        public string[] legend;
        
        public void Awake()
        {
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].text = "\"" + ControlsKeys.GetKey(keys[i]) + "\"\n " + legend[i];
            }
        }
    }
}