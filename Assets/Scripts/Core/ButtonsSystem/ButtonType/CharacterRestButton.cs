using System.Linq;
using Core.ButtonsSystem.ButtonList;
using Core.Saves;
using Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Core.ButtonsSystem.ButtonType
{
    /**<summary>The buttons for the characters in the rest system.</summary>*/
    public class CharacterRestButton : GenericButton
    {
        /**<summary>The prefab of the member interface.</summary>*/
        public MemberHUDButton prefab;
        
        /**<summary>Rest points of the character.</summary>*/
        private Text _restPoints;
        /**<summary>The image of the button.</summary>*/
        private Image _image;
        /**<summary>The border of the image of the button.</summary>*/
        private Image _formationMark;
        /**<summary>Get all texts of the prefab.</summary>*/
        private Text[] _textsOfPrefab;
        /**<summary>Get all images of the prefab.</summary>*/
        private Image[] _imagesOfPrefab;
        
        private new void Awake()
        {
            _image = transform.GetChild(0).GetComponentInChildren<Image>();
            _restPoints = transform.GetChild(0).GetChild(0).GetComponentInChildren<Text>();
            _restPoints.text = 5.ToString();
            base.Awake();
            prefab.GetComponent<RectTransform>().anchorMax = new Vector2(1,1);
            prefab.transform.gameObject.SetActive(true);
            prefab.transform.GetComponent<BoxCollider2D>().enabled = false;
            prefab = Instantiate(prefab, gameObject.transform.GetChild(1));
            _textsOfPrefab = prefab.GetComponentsInChildren<Text>();
            _imagesOfPrefab = prefab.GetComponentsInChildren<Image>();
            _formationMark = transform.Find("NoBlink").GetComponent<Image>();
            itemName = prefab.itemName;
        }

        private new void Update()
        {
            if (SavesFiles.GetSave().GetCharacter(MemberID).RestPoints == 0 && !RestSystem.RestSystem.ToChange(MemberID)) CanPress(false);
            else CanPress(true);
            _restPoints.text = SavesFiles.GetSave().Characters[MemberID].RestPoints.ToString();
            if (RestButtonsList.Option.Equals("Party")) ToMark(CharacterButtonsList.Pos == MemberID, Color.red);
            else ToMark(SavesFiles.GetParty()
                .Take(Mathf.Min(3, SavesFiles.GetParty().Length))
                .Any(b => b.ID == MemberID), Color.magenta);
            base.Update();
        }

        /**<summary>Set the interface if can or not press and indicates if it can press or not.</summary>*/
        private new void CanPress(bool can)
        {
            base.CanPress(can);
            
            foreach (Image image in _imagesOfPrefab) { image.color = can ? Color.white : new Color(C, C, C, 1); }
            
            foreach (Text text in _textsOfPrefab) { text.color = can ? Color.white : new Color(C, C, C, 1); }
            
            _image.color = can ? Color.white : new Color(C, C, C, 1);
            _restPoints.color = can ? Color.white : new Color(C, C, C, 1);

            _imagesOfPrefab[6].color = Color.black;
            _imagesOfPrefab[9].color = Color.black;
            
            _imagesOfPrefab[5].color = can ? Color.red : Color.red - new Color(C, C, C, 0);
            _imagesOfPrefab[8].color = can ? Color.magenta : Color.magenta - new Color(C, C, C, 0);

        }

        /**<summary>Init the button data with the character.</summary>*/
        public void SetUp(Character character)
        {
            prefab.SetUp(character);
            prefab.onlyHUD = false;
        }

        /**<sumary>Marks with red border if true, white if false.</sumary>*/
        public new void ToMark(bool toMark, Color color) => _formationMark.color = toMark ? color : Color.white;
        
        /**<summary>Get the character IDs.</summary>*/
        public int MemberID => prefab.character.ID;

    }
}