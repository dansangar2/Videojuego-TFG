using System;
using UnityEngine;

namespace Core.Messages
{
    [Serializable]
    public class TextData
    {
        #region ATTRIBUTES

        /**<summary>The text of the message.</summary>*/
        [TextArea] public string text;
        
        /**<summary>The profile ID. Negative ID uses Characters profiles (-1 = Character 0)</summary>*/
        public int characterId;
        /**<summary>The speed of the message.</summary>*/
        [Range(0, 1)]
        public float speed = 0.1f;
        /**<summary>The second that the message wait when finished (press key or automated).</summary>*/
        [Range(0, 20)]
        public float retard = 0.1f;
        /**<summary>It indicates that the message is passing alone, without you press any key.</summary>*/
        public bool automated;

        #endregion

        #region CONSTRUCTOR

        /**<summary>New simple text generator.</summary>*/
        public TextData(string text)
        {
            this.text = text;
        }

        #endregion

    }
}