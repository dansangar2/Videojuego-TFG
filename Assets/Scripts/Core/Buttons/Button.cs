using Core.Controls;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core.Buttons
{
    /**<summary>Base of the buttons, the base movement system is here</summary>*/
    //[Serializable]
    public class Button : MonoBehaviour
    {

        #region ATTRIBUTES

        /**<summary>It decides if when the player press it, it'll send you
        to another window. If it's empty, then the player isn't moving.</summary>*/
        [SerializeField]private string sceneRedirect;
        /**<summary>If blink is marks, then it'll the velocity of the blinks.</summary>*/
        [SerializeField]private float velocity = 0.005f;
        /**<summary>It's the name of the button.</summary>*/
        [SerializeField]private string buttonName;
        /**<summary>This windows will appear when the button is pressing.</summary>*/
        [SerializeField]private Transform window;
        /**<summary>Close the game.</summary>*/
        [SerializeField]private bool quitGame;
        
        /**<summary>It marks if the button is selected.</summary>*/
        public bool IsSelect { get; set; }
        /**<summary>It marks The color when it's marked.</summary>*/
        [SerializeField]private Color buttonColor = Color.green;
        
        /**<summary>It decides if when it's marked, it'll blink</summary>*/
        [SerializeField]private bool blink;
        /**<summary>The image button if it'll be necessary.</summary>*/
        [SerializeField]private Image buttonImage;
        /**<summary>It decides if the blink return back or continue.</summary>*/
        [SerializeField]private bool buttonChangeColor;

        //public bool IsStopped { get; }

        #endregion

        #region SYSTEM

        protected void Start()
        {
            if (blink)
            {
                buttonImage = transform.Find("Blink").GetComponent<Image>();
                buttonImage.color = new Color(buttonColor.r,buttonColor.g,buttonColor.b,0.8f - velocity);
            }
            else buttonImage = transform.Find("NoBlink").GetComponent<Image>();
            buttonImage.gameObject.SetActive(true);
            GetComponentInChildren<Text>().text = buttonName;
            buttonImage.color = new Color(
                buttonImage.color.r, 
                buttonImage.color.g, 
                buttonImage.color.b, 
                0.8f);
        }

        protected void Update()
        {
            if(blink) ToBlink();
            else buttonImage.color = IsSelect ? buttonColor : Color.white;
            
            //Redirect to another scene if "sceneRedirect" != ""
            if (Input.GetKeyDown(ControlsKeys.Ok)
                && IsSelect)
            {
                if (!sceneRedirect.Trim().Equals(""))
                {
                    SceneManager.LoadScene(sceneRedirect.Trim(), LoadSceneMode.Single);
                }
                if(!window.Equals(null))
                {
                    transform.root.gameObject.SetActive(false);
                    window.gameObject.SetActive(true);
                }
                if (quitGame)
                {
                    Application.Quit();
                }
                
            }
            
        }

        #endregion

        #region BLINK

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
            if (Input.GetKeyDown(ControlsKeys.Ok)
                && IsSelect)
            {
                Application.Quit();
            }
        }

        #endregion

        public void SetUp(ButtonData buttonData, 
            bool nBlink = false, 
            float nVelocity = 0, 
            Color nButtonColor = default)
        {
            
            blink = nBlink;
            sceneRedirect = buttonData.sceneRedirect;
            velocity = nVelocity;
            buttonName = buttonData.buttonName;
            window = buttonData.window;
            quitGame = buttonData.quitGame;
            buttonColor = nButtonColor;
        }
    }
}