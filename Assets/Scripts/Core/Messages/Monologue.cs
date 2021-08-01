

using System.Collections;
using Core.Controls;
using Data;
using Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Messages
{
    
    public class Monologue : Message
    {
        #region ATTRIBUTES

        public bool right;
        
        protected Image CharacterIcon;
        protected Text CharacterName;
        protected Transform CharImage;

        #endregion

        #region SYSTEM

        private void Start()
        {
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

        protected void MonologueStart()
        {
            IsSomeMessageOn = true;
            
            TextBox = transform.GetChild(up ? 0 : 3);
            CharImage = transform.GetChild(right ? 2 : 1);
            
            CharImage.gameObject.SetActive(true);
            TextBox.gameObject.SetActive(true);

            InitUI();
            UpdateUI();
            
            StartCoroutine(Next());
        }

        protected new void InitUI()
        {
            base.InitUI();
            CharacterIcon = CharImage.GetComponent<Image>();
            CharacterName = TextBox.GetChild(1).GetChild(0).GetComponentInChildren<Text>();
        }
        
        #endregion

        #region NEXT

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

        protected void MonologueUpdate()
        {
            UpdateUI();
            if (TextWindow.text != messages[Index].text ||
                !Input.GetKeyDown(ControlsKeys.Ok) && !messages[Index].automated || !ToNext) return;
            ToNext = false;
            StartCoroutine(Wait());
        }
        
        protected new void UpdateUI()
        {
            Profile current = GameData.ProfileDB.FindByID(messages[Index].characterId);
            CharacterIcon.sprite = current.Art; 
            CharacterName.text = current.Name;

            base.UpdateUI();

        }
        
        #endregion

    }
}