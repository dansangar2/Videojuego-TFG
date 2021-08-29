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
        public float velocity = 0.5f;
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
        /**<summary>The colors that the buttons will have.</summary>*/
        private Color[] _marksColors;
        /**<summary>The color index that turn.</summary>*/
        private int _i;
        
        /**<summary>The time that the color need to full change.</summary>*/
        private const float Wait = 0.8f;

        /**<summary>It marks if the button is selected.</summary>*/
        public bool IsSelect { get; set; }
        
        
        /**<summary>It marks The color when it's marked.</summary>*/
        [SerializeField]protected Color buttonColor = Color.green;
        /**<summary>The image button if it'll be necessary.</summary>*/
        [SerializeField]protected Image buttonImage;
        /**<summary>It decides if the blink return back or continue.</summary>*/
        [SerializeField]protected bool buttonChangeColor;
        
        //Used for "cannot choose" color.
        protected const float C = 145f / 255f;

        #endregion

        #region SYSTEM

        protected void Awake()
        {
            IsBlink();
        }

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
                buttonImage.color = new Color(buttonColor.r,buttonColor.g,buttonColor.b,0.8f);
                _lerp = 1 - Wait;
                _marksColors = new[] {
                    new Color(
                        buttonImage.color.r, 
                        buttonImage.color.g, 
                        buttonImage.color.b,
                        0f),
                    new Color(
                        buttonImage.color.r, 
                        buttonImage.color.g, 
                        buttonImage.color.b,
                        0.9f) 
                };
            }
            else buttonImage = transform.Find("NoBlink").GetComponent<Image>();
            buttonImage.gameObject.SetActive(true);
        }
        
        private float _lerp;
        
        /**<summary>Blink animation.</summary>*/
        private void ToBlink()
        {
            //If it's selected, then the blink animation started.
            if (IsSelect)
            {
                float vel = velocity / 60f;
                buttonImage.enabled = true;

                buttonImage.color = Color.Lerp(buttonImage.color, _marksColors[_i], vel);

                _lerp = Mathf.Lerp(_lerp, 1f, vel);
                if (_lerp <= Wait) return;
                _lerp = 1 - Wait;
                _i++;
                _i %= _marksColors.Length;
                
            }
            //If not, the color, index and _lerp time must init.
            else
            {
                buttonImage.color = new Color(buttonColor.r,buttonColor.g,buttonColor.b,0.8f);
                buttonImage.enabled = false;
                _lerp = 1 - Wait;
                _i = 0;
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