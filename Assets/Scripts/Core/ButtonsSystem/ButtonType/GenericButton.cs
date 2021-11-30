using System;
using System.Linq;
using Core.ButtonsSystem.ButtonList;
using Core.Controls;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core.ButtonsSystem.ButtonType
{
    /**<summary>Base of the buttons, the base system and main functions is here.</summary>*/
    public class GenericButton : MonoBehaviour
    {

        #region ATTRIBUTES

        /**<summary>It decides if when the player press it, it'll send you
        to another window. If it's empty, then the player isn't moving.</summary>*/
        public string sceneRedirect = "";
        /**<summary>If blink is marks, then it'll the velocity of the blinks.</summary>*/
        public float velocity = 0.5f;
        /**<summary>It's the main text of the button.</summary>*/
        public Text itemName;
        /**<summary>This windows will appear when the button is pressing.</summary>*/
        public Transform window;
        /**<summary>Close the game.</summary>*/
        public bool quitGame;
        /**<summary>Indicate that the button can send a message.</summary>*/
        public bool canSendMessage;
        /**<summary>Send a message.</summary>*/
        public string messageToSend = "";
        /**<summary>Indicate if it can press.</summary>*/
        public bool canPress = true;
        /**<summary>It's a global variable that get the "messageToSend" value of the pressed button.</summary>*/
        public static string Message = "";
        /**<summary>It decides if when it's marked, it'll blink</summary>*/
        public bool blink;
        /**<summary>The colors that the buttons will have.</summary>*/
        private Color[] _marksColors;
        /**<summary>The color index that turn.</summary>*/
        private int _i;
        
        /**<summary>The time that the color need to change color.</summary>*/
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

        #region Click Button

        /**<summary>It checks if the collider is clicked.</summary>*/
        public bool isCollider;
        
        /**<summary>The main camera.</summary>*/
        private Camera _camera;
        /**<summary>The collider of the button.</summary>*/
        public BoxCollider2D currentCollider;
        /**<summary>The parent of the button.</summary>*/
        private BaseButtonsList _list;
        /**<summary>The RaycastHit obtained from click.</summary>*/
        private RaycastHit2D _hit;
        /**<summary>The RaycastHit obtained from click.</summary>*/
        private bool _clicked;
        /**<summary>Check if the collider it's different after click.</summary>*/
        private bool _haveAnotherCollider;

        #endregion
        
        #endregion

        #region SYSTEM

        private void OnEnable()
        {
            isCollider = false;
            _camera = Camera.main;
            Transform b = transform;
            try
            {
                BoxCollider2D d;
                while (_list == null)
                {
                    b = b.parent;
                    _list = b.transform.GetComponent<BaseButtonsList>();
                    d = b.GetComponent<BoxCollider2D>();
                    if (d || _haveAnotherCollider) _haveAnotherCollider = true;
                }
            }
            catch (Exception)
            {
                // ignored
            }
            
            currentCollider = transform.GetComponent<BoxCollider2D>();
            GridLayoutGroup lg = transform.parent.GetComponent<GridLayoutGroup>();
            if (lg) currentCollider.size = lg.cellSize;
        }

        protected void Awake()
        {
            IsBlink();
        }

        protected void Update()
        {
            #region Click Button

            isCollider = false;
            if(blink) ToBlink();
            else buttonImage.color = IsSelect ? buttonColor : Color.white;
            if (!_list.enabled || _haveAnotherCollider)
            {
                currentCollider.enabled = false;
                return;
            }
            currentCollider.enabled = true;

            //Check if is pressed and keep the mouse the choose button.
            RaycastHit2D hit = default;
            if (Input.GetMouseButtonUp(0))
            {
                _clicked = false;
                if(_hit) hit = _hit;
                _hit = default;
            }else if (Input.GetMouseButtonDown(0)&&!_hit&&!_clicked)
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                _hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
                _clicked = _hit;
            }else if (Input.GetMouseButton(0)&&_clicked)
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                _hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
            }
            
            if (currentCollider == hit.collider && !IsSelect)
            {
                _list.gameObject.SetActive(false);
                _list.gameObject.SetActive(true);
                _list.SelectNone(_list.baseButtons);
                _list.position = Array.IndexOf(_list.baseButtons, 
                    _list.baseButtons.First(s => s.transform.Equals(transform)));
                _list.SelectCurrent(_list.baseButtons);
                hit = default;
            }

            #endregion

            //============================================================
            
            if (!(Input.GetKeyDown(ControlsKeys.Ok) || currentCollider == hit.collider
                ) || !IsSelect || !canPress) return;

            isCollider = true;
            
            //Redirect to another scene if "sceneRedirect" != ""
            if (!sceneRedirect.Trim().Equals(""))
            {
                SceneManager.LoadScene(sceneRedirect.Trim(), LoadSceneMode.Single);
            }
            //Appear a window if "window" != ""
            /*if(!window.Equals(null))
            {
                transform.root.gameObject.SetActive(false);
                window.gameObject.SetActive(true);
            }*/
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
            messageToSend = buttonData.Message;
            if (!messageToSend.Equals("")) canSendMessage = true;
            sceneRedirect = buttonData.SceneRedirect;
            velocity = nVelocity;
            itemName.text = buttonData.ButtonText;
            window = buttonData.Window;
            quitGame = buttonData.QuitGame;
            buttonColor = nButtonColor;
        }
        
        /**<sumary>Marks with red border if true, white if false.</sumary>*/
        public void ToMark(bool toMark, Color color) => buttonImage.color = toMark ? color : Color.white;

        /**<summary>Set it that if it can press or not.</summary>*/
        protected void CanPress(bool can)
        {
            canPress = can;
        }

        /**<summary>Get the image that marks if it's selected.</summary>*/
        public Image ImageOfButton => buttonImage;

    }
}