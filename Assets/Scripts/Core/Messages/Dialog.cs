using System.Collections;
using Core.Controls;
using Data;
using Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Messages
{
    public class Dialog : Monologue
    {

        #region ATTRIBUTES

        private Image _characterIcon2;
        private Text _textWindow2;
        private Text _characterName2;

        private Transform _textBox2;
        private Transform _charImage2;

        private int _iteration;

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
                case TextBoxType.Dialog:
                    DialogStart();
                    break;
            }
        }

        private void Update()
        {
            switch (type)
            {
                case TextBoxType.Message:
                    MessageUpdate();
                    break;
                case TextBoxType.Monologue:
                    MonologueUpdate();
                    break;
                case TextBoxType.Dialog:
                    DialogUpdate();
                    break;
            }
        }

        #endregion
        
        #region START
        
        private void DialogStart()
        {
            IsSomeMessageOn = true;
            
            TextBox = transform.GetChild(up ? 0 : 3);
            CharImage = transform.GetChild(right ? 2 : 1);

            _textBox2 = transform.GetChild(up ? 3 : 0);
            _charImage2 = transform.GetChild(right ? 1 : 2);

            CharImage.gameObject.SetActive(true);
            TextBox.gameObject.SetActive(true);

            InitUI();
            UpdateUI();
            
            StartCoroutine(Next());
        }

        private new void InitUI()
        {
            base.InitUI();
            _characterName2 = _textBox2.GetChild(1).GetChild(0).GetComponentInChildren<Text>();
            _characterIcon2 = _charImage2.GetComponent<Image>();
            _textWindow2 = _textBox2.GetChild(0).GetChild(0).GetComponent<Text>();
        }

        private void DialogUpdate()
        {
            ResetMessage(_iteration == 1 ? _textWindow2 : TextWindow);
        }

        #endregion

        #region NEXT

        private IEnumerator Next()
        {
            UpdateUI();
            if (_iteration == 1) yield return Speaker(_textWindow2);
            else yield return Speaker(TextWindow);
        }
        
        private new IEnumerator Wait()
        {
            messages[Index].automated = false;
            float seconds = 0;
            while (messages[Index].retard > seconds)
            {
                yield return new WaitForSeconds(0.01f);
                if (Input.GetKeyDown(ControlsKeys.Ok)) messages[Index].retard = 0;
                seconds += 0.01f;
            }
            
            if (Index < messages.Length - 1)
            {
                if (messages[Index+1].characterId != messages[Index].characterId) _iteration++;
                _iteration %= 2;
                Index++;
                StartCoroutine(Next());
            }
            else
            {
                IsSomeMessageOn = false;
                DestroyItem();
            }
        }

        #endregion

        #region UPDATE

        private IEnumerator Speaker(Text text)
        {
            text.text = "";
            foreach (char c in messages[Index].text)
            {
                text.text += c;
                yield return new WaitForSeconds(messages[Index].speed);
            }
            
            ToNext = true;
        }
        
        private void ResetMessage(Text text1)
        {
            
            if (text1.text != messages[Index].text ||
                !Input.GetKeyDown(ControlsKeys.Ok) && !messages[Index].automated || !ToNext) return;
            ToNext = false;
            StartCoroutine(Wait());
        }

        private new void UpdateUI()
        {
            if(_iteration==1)
            { 
                if (!_textBox2.gameObject.activeSelf) { _textBox2.gameObject.SetActive(true); }
                if (!_charImage2.gameObject.activeSelf) { _charImage2.gameObject.SetActive(true); } 
                
                Profile current = GameData.ProfileDB.FindByID(messages[Index].characterId); 
                _characterIcon2.sprite = current.Art; 
                _characterName2.text = current.Name;

                if (messages[Index].speed < 0) messages[Index].speed = 0;
            }
            else
            {
                TextBox = transform.GetChild(up ? 0 : 3);
                CharImage = transform.GetChild(right ? 2 : 1);

                base.UpdateUI();
            }

        }

        #endregion
    }
}