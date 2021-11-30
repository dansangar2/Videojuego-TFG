using Core.ButtonsSystem.ButtonList;
using Core.ButtonsSystem.ButtonType;
using Core.Saves;
using UnityEngine;

namespace Core.MenuSystem
{
    public class Menu : MonoBehaviour
    {
        /**<summary>Saves files list prefab.</summary>*/
        public SaveButtonsList saves;
        /**<summary>Generic buttons got of the menu.</summary>*/
        private GenericButtonsList _list;

        private void Awake()
        {
            //Get the buttons of the menu for deactivate when the save list is ON.  
            _list = GetComponent<GenericButtonsList>();
            //SavesFiles.DeleteAllData();
        }

        private void Update()
        {
            switch (Option)
            {
                case "Load":
                    _list.enabled = false;
                    saves.gameObject.SetActive(true);
                    break;
                default:
                    SavesFiles.Init();
                    saves.gameObject.SetActive(false);
                    _list.enabled = true;
                    break;
            }
        }

        /**<summary>The value of the action of the menu.</summary>*/
        private static string Option => GenericButton.Message;
        
    }
}