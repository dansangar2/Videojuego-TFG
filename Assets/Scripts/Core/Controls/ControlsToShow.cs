using UnityEngine;
using UnityEngine.UI;

namespace Core.Controls
{
    /**<summary>Create a slot where explain the function of keys.</summary>*/
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
                texts[i].text = ControlsKeys.GetKey(keys[i]) + ": " + legend[i];
            }
        }
    }
}