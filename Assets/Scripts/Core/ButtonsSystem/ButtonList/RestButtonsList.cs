using Core.ButtonsSystem.ButtonData;
using Core.ButtonsSystem.ButtonType;
using Core.Controls;
using Core.Saves;
using Entities;
using UnityEngine;

namespace Core.ButtonsSystem.ButtonList
{
    public class RestButtonsList : BaseButtonsList
    {
        
        /**<sumary>The data of the buttons.</sumary>*/
        public AbilityButtonData[] buttonsData;
        /**<sumary>The list of buttons. It marks what button is select.
        <para>It'll get all entity Button.</para></sumary>*/ 
        private RestButton[] _restButtons;
        /**<sumary>The button prefab.</sumary>*/
        public RestButton prefab;

        public static string Option = "";
        
        protected void Awake()
        {
            foreach (Character character in SavesFiles.GetParty())
            {
                character.RestPoints = 5;
            }
            foreach (AbilityButtonData b in buttonsData)
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
            if(Input.GetKeyDown(ControlsKeys.Ok)) Option = _restButtons[position].messageToSend;
        }
    }
}