using Core.Controls;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core.ButtonsSystem.ButtonType
{
    /**<summary>Base of the buttons, the base movement system and main functions is here.</summary>*/
    public class GenericButton : MonoBehaviour
    {

        #region ATTRIBUTES

        /**<summary>It decides if when the player press it, it'll send you
        to another window. If it's empty, then the player isn't moving.</summary>*/
        public string sceneRedirect;
        /**<summary>If blink is marks, then it'll the velocity of the blinks.</summary>*/
        public float velocity = 0.005f;
        /**<summary>It's the name of the button.</summary>*/
        public Text itemName;
        /**<summary>This windows will appear when the button is pressing.</summary>*/
        public Transform window;
        /**<summary>Close the game.</summary>*/
        public bool quitGame;
        /**<summary>Close the game.</summary>*/
        public bool canSendMessage;
        /**<summary>Send a message.</summary>*/
        public string messageToSend;
        /**<summary>Indicate if it can press.</summary>*/
        public bool canPress = true;
        /**<summary>It's a global variable it can use for:
        <para>You can write in messageToSend for when you'll press the button send this message.</para>
        </summary>*/
        public static string Message = "";
        /**<summary>It decides if when it's marked, it'll blink</summary>*/
        public bool blink;
        
        
        /**<summary>It marks if the button is selected.</summary>*/
        public bool IsSelect { get; set; }
        
        
        /**<summary>It marks The color when it's marked.</summary>*/
        [SerializeField]protected Color buttonColor = Color.green;
        /**<summary>The image button if it'll be necessary.</summary>*/
        [SerializeField]protected Image buttonImage;
        /**<summary>It decides if the blink return back or continue.</summary>*/
        [SerializeField]protected bool buttonChangeColor;
        
        protected const float C = 145f / 255f;

        //public bool IsStopped { get; }

        #endregion

        #region SYSTEM

        protected void Start() { IsBlink(); }

        protected void Update()
        {
            if(blink) ToBlink();
            else buttonImage.color = IsSelect ? buttonColor : Color.white;


            if (!Input.GetKeyDown(ControlsKeys.Ok) || !IsSelect || !canPress) return;


            //Redirect to another scene if "sceneRedirect" != ""
            if (!sceneRedirect.Trim().Equals(""))
            {
                SceneManager.LoadScene(sceneRedirect.Trim(), LoadSceneMode.Single);
            }
            //Appear a window if "window" != ""
            if(!window.Equals(null))
            {
                transform.root.gameObject.SetActive(false);
                window.gameObject.SetActive(true);
            }
            if (quitGame)
            {
                Application.Quit();
            }
            //Send a message if it can.
            if (canSendMessage && !messageToSend.Equals(""))
            {
                Message = messageToSend;
            }

        }

        #endregion

        #region BLINK

        /**<summary>Check if this button will blink and set it or the normal image (option).</summary>*/
        private void IsBlink()
        {
            if (blink)
            {
                buttonImage = transform.Find("Blink").GetComponent<Image>();
                buttonImage.color = new Color(buttonColor.r,buttonColor.g,buttonColor.b,0.8f - velocity);
            }
            else buttonImage = transform.Find("NoBlink").GetComponent<Image>();
            buttonImage.gameObject.SetActive(true);
        }
        
        /**<summary>Blink animation.</summary>*/
        private void ToBlink()
        {
            if (IsSelect)
            {
                buttonImage.enabled = true;

                if (buttonImage.color.a > 0.8||buttonImage.color.a<0.1) buttonChangeColor = !buttonChangeColor;

                switch (buttonChangeColor)
                {
                    case true:
                        buttonImage.color = new Color(
                            buttonImage.color.r, 
                            buttonImage.color.g, 
                            buttonImage.color.b, 
                            buttonImage.color.a - velocity);
                        break;
                    case false:
                        buttonImage.color = new Color(
                            buttonImage.color.r,
                            buttonImage.color.g,
                            buttonImage.color.b,
                            buttonImage.color.a + velocity);
                        break;
                }
            }
            else
            {
                buttonImage.enabled = false;
                buttonImage.color = new Color(buttonColor.r,buttonColor.g,buttonColor.b,0.8f - velocity);
            }
        }

        #endregion

        /**<summary>Set the data of the button with the buttonData object.
        <param name="buttonData">The data that the button will have.</param>
        <param name="nButtonColor">The color (blink or not) of the button.</param>
        <param name="nBlink">If it blinks.</param>
        <param name="nVelocity">It velocity</param></summary>*/
        public void SetUp(ButtonData.GenericButtonData buttonData, 
            Color nButtonColor = default, 
            bool nBlink = false, 
            float nVelocity = 0)
        {
            
            blink = nBlink;
            sceneRedirect = buttonData.SceneRedirect;
            velocity = nVelocity;
            itemName.text = buttonData.ButtonText;
            window = buttonData.Window;
            quitGame = buttonData.QuitGame;
            buttonColor = nButtonColor;
        }

        /**<summary>Set it that if it can press or not.</summary>*/
        protected void CanPress(bool can)
        {
            canPress = can;
        }

    }
}