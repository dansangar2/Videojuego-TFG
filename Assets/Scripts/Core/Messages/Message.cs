using System.Collections;
using Core.Controls;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core.Messages
{
    /**<summary>A simple message without characters speaking.</summary>*/
    public class Message : MonoBehaviour
    {
        #region ATTRIBUTES

        /**<summary>It indicates that type of message being.</summary>*/
        public TextBoxType type;
        /**<summary>It's indicates that the text appeared up.</summary>*/
        public bool up;
        /**<summary>The array with the messages.</summary>*/
        public TextData[] messages;
        /**<summary>The scene to redirect when it finished. "" won't redirect.</summary>*/
        public string sceneRedirect;
        
        /**<summary>The text where the message is showing.</summary>*/
        protected Text TextWindow;
        /**<summary>It's the transform that contains the TextWindow where the message being.</summary>*/
        protected Transform TextBox;
        
        /**<summary>The index of current message of array message.</summary>*/
        protected int Index;
        /**<summary>It indicates if we can pass to the next message.</summary>*/
        protected bool ToNext;

        /**<summary>It's a global variable that indicates if there are some message on.</summary>*/
        protected static bool IsSomeMessageOn;

        #endregion
        
        #region SYSTEM
        
        private void Awake() 
        { 
            IsSomeMessageOn = true;
            if (messages.Length == 0) messages = new []{new TextData("")}; 
            MessageStart(); 
        }

        private void Update()
        {
            MessageUpdate();
        }
        
        #endregion

        #region START

        /**<summary>Set the text transform for use it in the message.</summary>*/
        protected void MessageStart() 
        { 
            TextBox = transform.GetChild(up ? 0 : 3); 
            
            TextBox.GetChild(1).gameObject.SetActive(false); 
            TextBox.gameObject.SetActive(true);
            
            InitUI();
            UpdateUI();
            
            StartCoroutine(Next()); 
        }

        /**<summary>Init the text.</summary>*/
        protected void InitUI() { TextWindow = TextBox.GetChild(0).GetChild(0).GetComponent<Text>(); }
        
        #endregion
        
        #region NEXT

        /**<summary>Pass the next message.</summary>*/
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
        
        /**<summary>Wait the "retard" seconds and destroy if it's the last message.</summary>*/
        protected IEnumerator Wait() 
        { 
            messages[Index].automated = false; 
            float seconds = 0; 
            while (messages[Index].retard > seconds) 
            { 
                yield return new WaitForSeconds(0.01f); 
                if (Input.GetKeyDown(ControlsKeys.Ok) || Input.GetMouseButtonDown(0)) messages[Index].retard = 0; 
                seconds += 0.01f; 
            }
            
            if (Index < messages.Length - 1) 
            { 
                Index++; 
                TextWindow.text = ""; 
                ToNext = true; 
                StartCoroutine(Next()); 
                ToNext = false; 
            }
            else 
            { 
                IsSomeMessageOn = false; 
                DestroyItem(); 
            } 
        }
        
        #endregion

        #region UPDATE

        /**<summary>Update the message with the new one.</summary>*/
        protected void MessageUpdate() 
        { 
            if (TextWindow.text != messages[Index].text ||
                (!Input.GetKeyDown(ControlsKeys.Ok) && !Input.GetMouseButtonDown(0)) 
                && !messages[Index].automated 
                || !ToNext) return; 
            //ToNext = false; 
            StartCoroutine(Wait()); 
        }

        /**<summary>Update the UI with the new message.</summary>*/
        protected void UpdateUI() 
        { 
            if (messages[Index].speed < 0) messages[Index].speed = 0;
            
        }

        #endregion

        #region DESTROY

        /**<summary>Destroy the message system.</summary>*/
        protected void DestroyItem() 
        { 
            IsSomeMessageOn = false;
            if (!sceneRedirect.Equals("")) SceneManager.LoadScene(sceneRedirect);
            else Destroy(gameObject);
        }

        #endregion

        /**<summary>It's a global variable that indicates if there are some message on.</summary>*/
        public static bool ThereAreMessage() { return IsSomeMessageOn; }
        
        /**<summary>Set true the global variable that indicates that there are a messages or not.</summary>*/
        public static void SetExitsMessage(bool thereAre = true) { IsSomeMessageOn = thereAre; }
    }
}