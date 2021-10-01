using Core.ButtonsSystem.ButtonList;
using Core.Saves;
using UnityEngine;
using UnityEngine.UI;

namespace Core.ButtonsSystem.ButtonType
{
    public class CharacterRestButton : GenericButton
    {
        public MemberHUDButton prefab;
        
        private Text _restPoints;
        private Image _image;
        private Image _formationMark;
        private Text[] _textsOfPrefab;
        private Image[] _imagesOfPrefab;
        
        private new void Awake()
        {
            _image = transform.GetChild(0).GetComponentInChildren<Image>();
            _restPoints = transform.GetChild(0).GetChild(0).GetComponentInChildren<Text>();
            _restPoints.text = 5.ToString();
            base.Awake();
            prefab.GetComponent<RectTransform>().anchorMax = new Vector2(1,1);
            prefab.transform.gameObject.SetActive(true);
            prefab = Instantiate(prefab, gameObject.transform.GetChild(1));
            _textsOfPrefab = prefab.GetComponentsInChildren<Text>();
            _imagesOfPrefab = prefab.GetComponentsInChildren<Image>();
            _formationMark = transform.Find("NoBlink").GetComponent<Image>();
        }

        private new void Update()
        {
            if (SavesFiles.GetSave().GetCharacter(MemberID).RestPoints == 0 && !RestSystem.RestSystem.ToChange(MemberID)) CanPress(false);
            else CanPress(true);
            _restPoints.text = SavesFiles.GetSave().Characters[MemberID].RestPoints.ToString();
            if (RestButtonsList.Option.Equals("Party")) ToMark(CharacterButtonsList.Pos == MemberID);
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
            
            prefab.SliderBarBP.color = can ? Color.red : new Color(1, 0, 0, 0.5f);
            prefab.SliderBarKP.color = can ? Color.magenta : new Color(1, 0, 1, 0.5f);

        }

        /**<summary>Init the button data with the character.</summary>*/
        public void SetUp(int character)
        {
            prefab.SetUp(character);
            prefab.onlyHUD = false;
        }

        /**<sumary>Marks with red border if true, white if false.</sumary>*/
        public void ToMark(bool toMark) => _formationMark.color = toMark ? Color.red : Color.white;
        
        public int MemberID => prefab.MemberID;

    }
}