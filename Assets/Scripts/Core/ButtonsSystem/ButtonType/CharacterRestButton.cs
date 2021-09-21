using Core.Saves;
using UnityEngine;
using UnityEngine.UI;

namespace Core.ButtonsSystem.ButtonType
{
    public class CharacterRestButton : GenericButton
    {
        //[SerializeField] protected int id;
        public MemberHUDButton prefab;
        
        private Text _restPoints;
        private Image _image;
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
        }

        private new void Update()
        {
            if (SavesFiles.GetSave().GetCharacter(MemberID).RestPoints == 0) CanPress(false);
            else CanPress(true);
            _restPoints.text = SavesFiles.GetSave().Characters[MemberID].RestPoints.ToString();
            base.Update();
        }

        /**<summary>Set the interface if can or not press and indicates if it can press or not.</summary>*/
        private new void CanPress(bool can)
        {
            base.CanPress(can);
            
            prefab.bp.image.color = can ? Color.white : new Color(C, C, C, 1);
            prefab.kp.image.color = can ? Color.white : new Color(C, C, C, 1);
            /*prefab.level.color = can ? Color.white : new Color(C, C, C, 1);
            prefab.next.color = can ? Color.white : new Color(C, C, C, 1);
            prefab.bloodMax.color = can ? Color.white : new Color(C, C, C, 1);
            prefab.bloodPoints.color = can ? Color.white : new Color(C, C, C, 1);
            prefab.karmaMax.color = can ? Color.white : new Color(C, C, C, 1);
            prefab.karmaPoints.color = can ? Color.white : new Color(C, C, C, 1);
            prefab.itemIcon.color = can ? Color.white : new Color(C, C, C, 1);
            prefab.elementIcon.color = can ? Color.white : new Color(C, C, C, 1);
*/
            foreach (Image image in _imagesOfPrefab)
            {
                image.color = can ? Color.white : new Color(C, C, C, 1);
            }
            
            foreach (Text text in _textsOfPrefab)
            {
                text.color = can ? Color.white : new Color(C, C, C, 1);
            }
            
            _image.color = can ? Color.white : new Color(C, C, C, 1);
            _restPoints.color = can ? Color.white : new Color(C, C, C, 1);
        }

        public void SetUp(int character)
        {
            prefab.SetUp(character);
            prefab.onlyHUD = false;
        }

        public int MemberID => prefab.MemberID;

    }
}