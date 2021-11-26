using System;
using System.Linq;
using Data;
using Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Core.ButtonsSystem.ButtonType
{
    /**<summary>The HUD with the data of the characters.</summary>*/
    public class MemberHUDButton: BaseHUDButton
    {
        #region ATTRIBUTES

        public Character character;

        //All attributes are the stats of the character.
        public Text bloodPoints;
        public Text karmaPoints;
        public Text bloodMax;
        public Text karmaMax;
        public Text next;
        public Text level;
        public Slider bp;
        public Slider kp;
        public Image status1;
        public Image status2;
        /**<summary>It indicates if is a button or only a HUD.</summary>*/
        public bool onlyHUD;
        
        private int _j;
        private float _seconds;

        //The images of the bars
        private Image _imageBarBp;
        private Image _imageBarKp;

        #endregion

        #region SYSTEM

        private new void Awake()
        {
            _imageBarBp = bp.fillRect.GetComponent<Image>();
            _imageBarKp = kp.fillRect.GetComponent<Image>();
            if (onlyHUD) return;
            base.Awake();
        }

        private new void Update()
        {
            level.text = character.Level.ToString();
            next.text = (character.NedExp - character.ActExp).ToString();
            
            bloodPoints.text = character.CurrentBloodPoints.ToString();
            bloodMax.text = character.MaxBloodPoints.ToString();
            karmaPoints.text = character.CurrentKarmaPoints.ToString();
            karmaMax.text = character.MaxKarmaPoints.ToString();
            
            bp.maxValue = character.MaxBloodPoints;
            bp.value = character.CurrentBloodPoints;

            kp.maxValue = character.MaxKarmaPoints;
            kp.value = character.CurrentKarmaPoints;
            
            //States
            Sprite[] sprites = character.Statuses.Select(s => s.Status.Icon).ToArray();
            try
            {
                status1.sprite = sprites[_j];
                status1.gameObject.SetActive(true);
            }
            catch(Exception) 
            { 
                status1.gameObject.SetActive(false);
            }
            try 
            { 
                status2.sprite = sprites[_j+1];
                status2.gameObject.SetActive(true);
            }
            catch(Exception) 
            { 
                if(sprites.Length<1) status2.gameObject.SetActive(false);
            }
            
            _seconds += Time.deltaTime;
            if (_seconds <= 1) return;

            _j += 2; 
            if (_j >= sprites.Length) _j = 0;
            _seconds = 0;
            
            if (onlyHUD) return;
            base.Update();
        }

        #endregion

        /**<summary>Set up the data of this Character HUD with the new ID.</summary>*/
        public void SetUp(Character chara, bool isEnemy = false)
        {
            character = chara; 
            UpdateUI(chara, isEnemy);
        }
        
        /**<summary>Update the data of the character HUD with the current data of the CharacterID.</summary>*/
        public void UpdateUI(Character chara, bool isEnemy = false)
        {
            character = chara;
            itemIcon.sprite = isEnemy ? GameData.EnemyDB.FindByID(chara.ID).Face : 
                GameData.CharacterDB.FindByID(chara.ID).Face;
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

        /**<summary>Get the bar image of the BP slider</summary>*/
        public Image SliderBarBP => _imageBarBp;

        /**<summary>Get the bar image of the KP slider</summary>*/
        public Image SliderBarKP => _imageBarKp;
        
        /**<summary>Set the interface if can or not press and indicates if it can press or not.</summary>*/
        public new void CanPress(bool can)
        {
            base.CanPress(can);
            itemIcon.color = can ? Color.white : new Color(C, C, C, 1);
            elementIcon.color = can ? Color.white : new Color(C, C, C, 1);
            bloodPoints.color = can ? Color.white : new Color(C, C, C, 1);
            level.color = can ? Color.white : new Color(C, C, C, 1);
            bloodMax.color = can ? Color.white : new Color(C, C, C, 1);
            karmaPoints.color = can ? Color.white : new Color(C, C, C, 1);
            karmaMax.color = can ? Color.white : new Color(C, C, C, 1);
            itemName.color = can ? Color.white : new Color(C, C, C, 1);
            next.color = can ? Color.white : new Color(C, C, C, 1);
            
            _imageBarBp.color = can ? Color.red : new Color(1, 0, 0, 0.5f);
            _imageBarKp.color = can ? Color.magenta : new Color(1, 0, 1, 0.5f);
        }

    }
}