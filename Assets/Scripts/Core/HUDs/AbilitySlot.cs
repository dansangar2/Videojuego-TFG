using System;
using Data;
using Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Core.HUDs
{
    public class AbilitySlot : BaseHUD
    {
        public Text necessaryKp;

        private const float C = 145f / 255f;

        //public Text typeTest;
        //public Text elementText;
        
        public void SetUp(int nId, bool can=true) 
        { 
            base.SetUp(nId);
            UpdateUI();
            if (can) return;
            itemName.color = new Color(C, C, C, 1);
            necessaryKp.color = new Color(C, C, C, 1);
            elementIcon.color = new Color(C, C, C, 1);
            itemIcon.color = new Color(C, C, C, 1);
        }

        public new void Update()
        {
            base.Update();
        }
        
        public void UpdateUI()
        {
            Ability ability = GameData.AbilityDB.FindByID(id);
            itemName.text = ability.Name;
            //buttonName = ability.Name;
            necessaryKp.text = ability.Cost.ToString();
            
            Tuple<Sprite, Sprite> sprites = ability.AbilityIcons();

            if (sprites.Item2 != null) elementIcon.sprite = sprites.Item2;
            itemIcon.sprite = sprites.Item1;
            itemName.color = new Color(1, 1, 1, 1);
            necessaryKp.color = new Color(1, 1, 1, 1);
            elementIcon.color = new Color(1, 1, 1, 1);
            itemIcon.color = new Color(1, 1, 1, 1);

        }
        
    }
}