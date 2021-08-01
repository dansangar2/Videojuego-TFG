using System;
using UnityEngine;

namespace Core.Messages
{
    [Serializable]
    public class TextData
    {
        #region ATTRIBUTES

        [TextArea]
        public string text;
        
        public int characterId;
        [Range(0, 1)]
        public float speed = 0.1f;
        [Range(0, 20)]
        public float retard = 0.1f;
        public bool automated;

        #endregion

        #region CONSTRUCTOR

        public TextData(string text)
        {
            this.text = text;
        }

        #endregion

    }
}