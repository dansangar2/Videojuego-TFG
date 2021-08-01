using System.Collections;
using Core.Controls;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Messages
{
    public class Message : MonoBehaviour
    {
        #region ATTRIBUTES

        public TextBoxType type;
        
        public bool up;
        
        public TextData[] messages;
        
        protected Text TextWindow;
        protected Transform TextBox;
        protected int Index;
        protected bool ToNext;

        protected static bool IsSomeMessageOn;

        #endregion
        
        #region SYSTEM
        
        private void Start() 
        { 
            if (messages.Length == 0) messages = new []{new TextData("")}; 
            MessageStart(); 
        }
        
        private void Update() { MessageUpdate(); }
        
        #endregion

        #region START

        protected void MessageStart() 
        { 
            IsSomeMessageOn = true; 
            TextBox = transform.GetChild(up ? 0 : 3); 
            
            TextBox.GetChild(1).gameObject.SetActive(false); 
            TextBox.gameObject.SetActive(true);
            
            InitUI();
            UpdateUI();
            
            StartCoroutine(Next()); 
        }

        protected void InitUI() { TextWindow = TextBox.GetChild(0).GetChild(0).GetComponent<Text>(); }
        
        #endregion
        
        #region NEXT

        private IEnumerator Next() 
        { 
            TextWindow.text = "";
            
            UpdateUI();
            
            foreach (char c in messages[Index].text) 
            { 
                TextWindow.text += c; 
                yield return new WaitForSeconds(messages[Index].speed); 
            }
            
            ToNext = true;
            
        }
        
        protected IEnumerator Wait() 
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
                Index++; 
                TextWindow.text = ""; 
                ToNext = true; 
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

        protected void MessageUpdate() 
        { 
            if (TextWindow.text != messages[Index].text || 
                !Input.GetKeyDown(ControlsKeys.Ok) && !messages[Index].automated || !ToNext) return; 
            ToNext = false; 
            StartCoroutine(Wait()); 
        }

        protected void UpdateUI() 
        { 
            //textWindow = currentTextBox.GetChild(0).GetChild(0).GetComponent<Text>();
            if (messages[Index].speed < 0) messages[Index].speed = 0;
            
        }

        #endregion

        #region DESTROY

        protected void DestroyItem() 
        { 
            Destroy(gameObject); 
        }

        #endregion

        public static bool ThereAreMessage() { return IsSomeMessageOn; }
    }
}