using Core.ButtonsSystem.ButtonData;
using Core.ButtonsSystem.ButtonType;
using Core.Saves;

namespace Core.ButtonsSystem.ButtonList
{
    /**<summary>A list system for choose Rest action.</summary>*/
    public class RestButtonsList : BaseButtonsList
    {
        
        /**<sumary>The button prefab.</sumary>*/
        public RestButton prefab;
        /**<sumary>The data of the buttons.</sumary>*/
        public RestButtonData[] buttonsData;
        /**<sumary>The list of buttons. It marks what button is select.
        <para>It'll get all entity Button.</para></sumary>*/ 
        private RestButton[] _restButtons;

        /**<sumary>Get the action to do.</sumary>*/
        public static string Option = "";
        
        protected void Awake()
        {
            foreach (RestButtonData b in buttonsData)
            {
                prefab.SetUp(b);
                Instantiate(prefab, transform);
            }

            _restButtons = GetComponentsInChildren<RestButton>();
            _restButtons[0].IsSelect = true;
            SetColumnsAndRows(_restButtons);
        }

        public void Update()
        {
            Move(_restButtons);
            _restButtons[2].CanPress(SavesFiles.GetSave().Level != 1);
            //Debug.Log(Option);
            //if (Input.GetKeyDown(ControlsKeys.Ok) || Input.GetMouseButtonUp(0))
            //if(Option.Equals("")) 
            Option = GenericButton.Message; // _restButtons[position].messageToSend;
        }

        /**<summary>Get the current button.</summary>*/
        public RestButton Selected => _restButtons[position];
    }
}