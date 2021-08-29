

using System.Collections;
using Core.Controls;
using Data;
using Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Messages
{
    
    /**<summary>A message text with only one Character speaking.</summary>*/
    public class Monologue : Message
    {
        #region ATTRIBUTES

        /**<summary>It indicates that the character image appear on the right.</summary>*/
        public bool right;
        
        /**<summary>It's the character face or body.</summary>*/
        private Image _characterIcon;
        /**<summary>It's the character name.</summary>*/
        private Text _characterName;
        /**<summary>It's the transform that contains the image where the character image being.</summary>*/
        protected Transform CharImage;

        #endregion

        #region SYSTEM

        private void Awake()
        {
            IsSomeMessageOn = true;
            if (messages.Length == 0) messages = new []{new TextData("")};
            switch (type)
            {
                case TextBoxType.Message:
                    MessageStart();
                    break;
                case TextBoxType.Monologue:
                    MonologueStart();
                    break;
            }
        }
        
        void Update()
        {
            switch (type)
            {
                case TextBoxType.Message:
                    MessageUpdate();
                    break;
                case TextBoxType.Monologue:
                    MonologueUpdate();
                    break;
            }
        }

        #endregion

        #region START

        /**<summary>Set the text and image transform for use it in the monologue.</summary>*/
        protected void MonologueStart()
        {
            
            TextBox = transform.GetChild(up ? 0 : 3);
            CharImage = transform.GetChild(right ? 2 : 1);
            
            CharImage.gameObject.SetActive(true);
            TextBox.gameObject.SetActive(true);

            InitUI();
            UpdateUI();
            
            StartCoroutine(Next());
        }

        /**<summary>Init the text, and character name and face.</summary>*/
        protected new void InitUI()
        {
            base.InitUI();
            _characterIcon = CharImage.GetComponent<Image>();
            _characterName = TextBox.GetChild(1).GetChild(0).GetComponentInChildren<Text>();
        }
        
        #endregion

        #region NEXT

        /**<summary>Pass the next message.</summary>*/
        private IEnumerator Next()
        {
            TextWindow.text = "";

            foreach (char c in messages[Index].text)
            {
                TextWindow.text += c;
                yield return new WaitForSeconds(messages[Index].speed);
            }

            ToNext = true;
        }

        #endregion
        
        #region UPDATE

        /**<summary>Update the message with the new one.</summary>*/
        protected void MonologueUpdate()
        {
            UpdateUI();
            if (TextWindow.text != messages[Index].text ||
                !Input.GetKeyDown(ControlsKeys.Ok) && !messages[Index].automated || !ToNext) return;
            ToNext = false;
            StartCoroutine(Wait());
        }
        
        /**<summary>Update the UI with the character that are speaking and new message.</summary>*/
        protected new void UpdateUI()
        {
            Profile current = GameData.ProfileDB.FindByID(messages[Index].characterId);
            _characterIcon.sprite = current.Art; 
            _characterName.text = current.Name;

            base.UpdateUI();

        }
        
        #endregion

    }
}