using Core.ButtonsSystem.ButtonType;

namespace Core.ButtonsSystem.ButtonList
{
    public class GenericButtonsList : BaseButtonsList
    {
        /**<sumary>The data of the buttons.</sumary>*/
        public ButtonData.GenericButtonData[] buttonsData;
        /**<sumary>The list of buttons. It marks what button is select.
        <para>It'll get all entity Button.</para></sumary>*/ 
        private GenericButton[] _buttons;
        /**<sumary>The button prefab.</sumary>*/
        public GenericButton prefab;

        private void Awake()
        {
            foreach (ButtonData.GenericButtonData button in buttonsData)
            {
                prefab.SetUp(button, buttonColor, buttonsBlinks, velocity);
                Instantiate(prefab,
                    transform.GetChild(0)
                        .transform);
            }
            _buttons = gameObject.GetComponentsInChildren<GenericButton>();
            SetColumnsAndRows(_buttons);
            _buttons[0].IsSelect = true;
        }

        protected void Update()
        {
            Move(_buttons);
        }
    }
}