using UnityEngine;
using UnityEngine.UI;

namespace Core.ButtonsSystem.ButtonType
{
    /**<summary>Base button HUD that can use for items with two icons.</summary>*/
    public class BaseHUDButton : GenericButton
    {
        /**<summary>The main icon of the HUD.</summary>*/
        public Image itemIcon;
        /**<summary>The second icon of the HUD.</summary>*/
        public Image elementIcon;

        public new void Awake()
        {
            base.Awake();
        }

        /**<summary>Destroy a item.</summary>*/
        public void Remove()
        {
            Destroy(gameObject);
            Destroy(this);
        }

        /**<summary>Set the interface if can or not press and indicates if it can press or not.</summary>*/
        protected new void CanPress(bool can)
        {
            base.CanPress(can);
            itemName.color = can ? Color.white : new Color(C, C, C, 1);
            elementIcon.color = can ? Color.white : new Color(C, C, C, 1);
            itemIcon.color = can ? Color.white : new Color(C, C, C, 1);
        }
    }
}