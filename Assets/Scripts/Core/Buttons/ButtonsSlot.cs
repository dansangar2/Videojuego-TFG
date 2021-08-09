namespace Core.Buttons
{
    public class ButtonsSlot : Buttons
    {
        /**<sumary>The data of the buttons.</sumary>*/
        public ButtonData[] buttonsData;
        /**<sumary>The list of buttons. It marks what button is select.
        <para>It'll get all entity Button.</para></sumary>*/ 
        private Button[] _buttons;
        /**<sumary>The button prefab.</sumary>*/
        public Button prefab;
        
        protected void Start()
        {
            foreach (ButtonData button in buttonsData)
            {
                prefab.SetUp(button, buttonColor, buttonsBlinks, velocity);
                Instantiate(prefab,
                    transform.GetChild(0)
                        .transform);
            }
            _buttons = gameObject.GetComponentsInChildren<Button>();
            SetColumnsAndRows(_buttons);
            _buttons[0].IsSelect = true;
        }
        
        protected void Update()
        {
            Move(_buttons);
        }
    }
}