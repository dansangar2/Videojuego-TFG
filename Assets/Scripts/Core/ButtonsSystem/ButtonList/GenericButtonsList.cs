using Core.ButtonsSystem.ButtonData;
using Core.ButtonsSystem.ButtonType;

namespace Core.ButtonsSystem.ButtonList
{
    public class GenericButtonsList : BaseButtonsList
    {
        
        /**<sumary>The button prefab.</sumary>*/
        public GenericButton prefab;
        /**<sumary>The data of the buttons.</sumary>*/
        public GenericButtonData[] buttonsData;
        /**<sumary>The list of buttons. It marks what button is select.
        <para>It'll get all entity Button.</para></sumary>*/ 
        private GenericButton[] _buttons;

        private void Awake()
        {
            foreach (GenericButtonData button in buttonsData)
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