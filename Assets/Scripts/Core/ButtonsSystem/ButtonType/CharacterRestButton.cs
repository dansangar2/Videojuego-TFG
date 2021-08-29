using Core.Saves;
using UnityEngine;
using UnityEngine.UI;

namespace Core.ButtonsSystem.ButtonType
{
    public class CharacterRestButton : MemberHUDButton
    {
        private Text _restPoints;
        private Image _image;
        
        private new void Awake()
        {
            _image = transform.GetChild(0).GetComponentInChildren<Image>();
            _restPoints = transform.GetChild(0).GetChild(0).GetComponentInChildren<Text>();
            _restPoints.text = 5.ToString();
            base.Awake();
        }

        private new void Update()
        {
            if (SavesFiles.GetSave().GetCharacter(MemberID).RestPoints == 0) CanPress(false);
            else CanPress(true);
            _restPoints.text = SavesFiles.GetSave().Characters[id].RestPoints.ToString();
            base.Update();
        }

        /**<summary>Set the interface if can or not press and indicates if it can press or not.</summary>*/
        private new void CanPress(bool can)
        {
            base.CanPress(can);
            _image.color = can ? Color.white : new Color(C, C, C, 1);
            _restPoints.color = can ? Color.white : new Color(C, C, C, 1);
        }

    }
}