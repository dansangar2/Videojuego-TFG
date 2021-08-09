using Data;
using Entities;
using UnityEngine.UI;

namespace Core.HUDs
{
    public class Member: BaseHUD
    {
        public Text bloodPoints;
        public Text karmaPoints;
        public Text bloodMax;
        public Text karmaMax;
        public Text next;
        public Text level;
        public Slider bp;
        public Slider kp;
        public bool onlyHUD;

        private new void Start()
        {
            if (onlyHUD) return;
            base.Start();
        }

        private new void Update()
        {
            if (onlyHUD) return;
            base.Update();
        }
        
        public new void SetUp(int nId) 
        { 
            base.SetUp(nId);
            UpdateUI();
        }
        
        public void UpdateUI()
        {
            Character character = GameData.CharacterDB.FindByID(id); 
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