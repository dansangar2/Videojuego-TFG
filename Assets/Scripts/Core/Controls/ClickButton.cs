using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Controls
{
    /**<summary>System for can click in the buttons that show the key actions.</summary>*/
    [Serializable]
    public class ClickButton
    {
        /**<summary>Image to mark that is pressed.</summary>*/
        public Image image;
        /**<summary>The signal to send.</summary>*/
        public string key;
        /**<summary>The collider of the button.</summary>*/
        public BoxCollider2D collider;
        /**<summary>The ID of the button.</summary>*/
        public int id;
        /**<summary>Can press (not change color).</summary>*/
        public bool can;
        /**<summary>Get the current signal of the last button pressed.</summary>*/
        private static string _keyUsed = "";

        /**<summary>The main constructor.</summary>*/
        public ClickButton(int id, Image image, BoxCollider2D collider, string key, bool can = true)
        {
            this.image = image;
            this.key = key;
            this.id = id;
            this.collider = collider;
            this.can = can;
        }
        
        /**<summary>Get the current signal of the last button pressed.</summary>*/
        public static string KeyUsed
        {
            get => _keyUsed;
            set => _keyUsed = value;
        }
        
    }
}