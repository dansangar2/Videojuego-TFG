using Core.Saves;
using Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Core.ButtonsSystem.ButtonType
{
    /**<summary>Base button HUD that can use for items with two icons and IDs.</summary>*/
    public class BaseHUDButton : GenericButton
    {
        [SerializeField] protected int id;
        /**<summary>The main icon of the HUD.</summary>*/
        public Image itemIcon;
        /**<summary>The second icon of the HUD.</summary>*/
        public Image elementIcon;

        public new void Start()
        {
            base.Start();
        }

        /**<summary>Set the ID.</summary>*/
        protected void SetUp(int member) 
        { 
            id = member;
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

        /**<summary>The id of the member that it represents.</summary>*/
        public int MemberID => id;

        /**<summary>Get the character that it represents.</summary>*/
        public Character Character => SavesFiles.GetSave().GetCharacter(id);
    }
}