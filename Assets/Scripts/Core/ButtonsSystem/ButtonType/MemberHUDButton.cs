using Core.Saves;
using Entities;
using UnityEngine.UI;

namespace Core.ButtonsSystem.ButtonType
{
    /**<summary>The HUD and data of the characters.</summary>*/
    public class MemberHUDButton: BaseHUDButton
    {
        #region ATTRIBUTES

        //All attributes are the stats of the character.
        public Text bloodPoints;
        public Text karmaPoints;
        public Text bloodMax;
        public Text karmaMax;
        public Text next;
        public Text level;
        public Slider bp;
        public Slider kp;
        /**<summary>It indicates if is a button or only a HUD.</summary>*/
        public bool onlyHUD;

        #endregion

        #region SYSTEM

        private new void Awake()
        {
            if (onlyHUD) return;
            base.Awake();
        }

        private new void Update()
        {
            if (onlyHUD) return;
            base.Update();
        }

        #endregion
        
        /**<summary>Set up the data of this Character HUD with the new ID.</summary>*/
        public new void SetUp(int nId) 
        { 
            base.SetUp(nId);
            UpdateUI();
        }
        
        /**<summary>Update the data of the character HUD with the current data of the CharacterID.</summary>*/
        public void UpdateUI()
        {
            Character character = SavesFiles.GetSave().Characters[id]; 
            itemIcon.sprite = character.Face; 
            elementIcon.sprite = character.Element.Icon;
            bloodPoints.text = character.CurrentBloodPoints.ToString();
            level.text = character.Level.ToString();
            bloodMax.text = character.MaxBloodPoints.ToString();
            karmaPoints.text = character.CurrentKarmaPoints.ToString();
            karmaMax.text = character.MaxKarmaPoints.ToString();
            itemName.text = character.Name;
            next.text = (character.NedExp - character.ActExp).ToString();
            
            bp.maxValue = character.MaxBloodPoints;
            bp.value = character.CurrentBloodPoints;

            kp.maxValue = character.MaxKarmaPoints;
            kp.value = character.CurrentKarmaPoints;

        }
        
    }
}