using System;
using Core.Saves;
using Data;
using Entities;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

namespace Core.ButtonsSystem.ButtonType
{
    /**<summary>The button for save or load, and show the game data.</summary>*/
    public class SaveButton : GenericButton
    {
        /**<summary>Where the character faces will be.</summary>*/
        public Transform characterHud;
        /**<summary>The game level.</summary>*/
        public Text level;
        /**<summary>The prefab of the character face.</summary>*/
        public Image image;
        /**<summary>The party that the data has.</summary>*/
        public Character[] party;
        /**<summary>Check if is empty.</summary>*/
        public bool isEmpty = true;
        
        /**<summary>The images of the button.</summary>*/
        private Image[] _images = {};
        /**<summary>Max number of character face.</summary>*/
        private int _max = 14;

        public new void Awake()
        {
            base.Awake();
            image.gameObject.SetActive(true);
            
            for (int i = 0; i < _max; i++)
            {
                if (characterHud.childCount >= _max) break;
                Instantiate(image, characterHud.transform);
            }
            _images = characterHud.GetComponentsInChildren<Image>();
        }

        public new void Update()
        {
            base.Update();
            if (isEmpty)
            {
                itemName.text = "EMPTY";
                level.text = "";
                characterHud.gameObject.SetActive(false);
                return;
            }
            itemName.text = "";
            characterHud.gameObject.SetActive(true);
            for (int i = 0; i < _max; i++)
            {
                try
                {
                    _images[i].gameObject.SetActive(true);
                    _images[i].sprite = GameData.CharacterDB.FindByID(party[i].ID).Face;
                }
                catch (Exception)
                {
                    _images[i].gameObject.SetActive(false);
                }
            }
        }

        /**<summary>Set the data of this button with the data passed.</summary>*/
        public void SetUp(Save data)
        {
            blink = true;
            canSendMessage = true;
            isEmpty = data.IsEmpty;
            sceneRedirect = "";
            level.text = "Current Level " + data.Level;
            party = data.Party;
            //Array.Copy(data.Party, party, data.Party.Length);
            /*for (int i = 0; i < data.Party.Length; i++)
            {
                party[i] = new Character(data.Party[i]);
            }*/
            characterHud.gameObject.SetActive(true);
        }
    }
}