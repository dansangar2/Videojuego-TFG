using System.Linq;
using Core.ButtonsSystem.ButtonList;
using Core.Controls;
using Core.RestSystem.Actions;
using Core.Saves;
using Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Core.RestSystem
{
    /**<summary>System to choose the points to use.</summary>*/
    public class UseRestPointsInterface : MonoBehaviour
    {
        /**<summary>The ID of the character.</summary>*/
        [SerializeField] private int id;
        /**<summary>The text that show the rest points that the character has.</summary>*/
        public Text restPoints;
        /**<summary>Current points that the character would use.</summary>*/
        public int pointsToUse;
        /**<summary>Experience or BP/KP to gain.</summary>*/
        public Text points;
        
        #region ClickSystem

        /**<summary>Get the buttons that can click.</summary>*/
        private ClickButton[] _buttonsCanClick;
        
        /**<summary>Camera that check where are the colliders.</summary>*/
        private Camera _camera;

        /**<summary>Get the ID of the button that have been clicked of the buttons that can click.</summary>*/
        private int _pressed = -1;
        /**<summary>Get the ID of the button that have been selected, after pressed.</summary>*/
        private int _selected = -1;

        #endregion
        
        private void OnEnable()
        {
            _camera = Camera.main;
            Text legend = points.transform.parent.Find("Legend").GetComponent<Text>();
            legend.text = RestButtonsList.Option switch
            {
                "Training" => "Exp. To Gain",
                "Nursing" => "BP/KP To Recover",
                _ => legend.text
            };
            BoxCollider2D[] t = transform.GetComponentsInChildren<BoxCollider2D>();
            _buttonsCanClick = new ClickButton[4];
            for (int i = 0; i < 4; i++)
            {
                _buttonsCanClick[i] = new ClickButton(i,
                    t[i].GetComponent<Image>(), t[i],
                    t[i].name);
            }
        }

        public void Update()
        {
            #region Click System

            //Click button system.
            if (Input.GetMouseButton(0))
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

                //Check if it's selected after it had been pressed and released upper the same button.
                if (!hit && _selected > -1)
                {
                    _selected = -1;
                }
                else if (hit && _pressed == -1)
                {
                    _selected =
                        _buttonsCanClick.FirstOrDefault(b => b.collider == hit.collider && b.can)?.id ?? -1;
                    _pressed = _selected;
                }
                else if (!hit && _pressed == -1)
                {
                    _pressed = -2;
                }
                else if (_pressed>-1) 
                    if(hit && hit.collider == _buttonsCanClick[_pressed].collider) 
                        _selected = _pressed;
            }
            _buttonsCanClick = _buttonsCanClick.Select(s => {s.image.color 
                = s.id==_selected? Color.green : s.id == _pressed ? Color.yellow 
                : Color.white; return s; }).ToArray();

            #endregion
            
            //================================================
            
            if (Input.GetMouseButtonUp(0) && _selected>-1)
            {
                ClickButton.KeyUsed = _buttonsCanClick[_selected].key;
                _pressed = -1;
                _selected = -1;
            }
            else if(Input.GetMouseButtonUp(0) || ClickButton.KeyUsed!="")
            {
                ClickButton.KeyUsed = "";
                _pressed = -1;
                _selected = -1;
            }

            if (Input.GetKeyDown(ControlsKeys.MoveUp) || ClickButton.KeyUsed.Equals("MvU"))
            {
                pointsToUse++;
                if (pointsToUse > Character.RestPoints) pointsToUse = 1;
                if (pointsToUse < 1) pointsToUse = Character.RestPoints;
            }else if (Input.GetKeyDown(ControlsKeys.MoveDown) || ClickButton.KeyUsed.Equals("MvD"))
            {
                pointsToUse--;
                if (pointsToUse > Character.RestPoints) pointsToUse = 1;
                if (pointsToUse < 1) pointsToUse = Character.RestPoints;
                
            }

            restPoints.text = pointsToUse.ToString();

            points.text = RestButtonsList.Option switch
            {
                "Training" => TrainingAction.Formula(Character, pointsToUse).ToString(),
                "Nursing" => NursingAction.Formula(Character, pointsToUse) 
                             + "/" 
                             + NursingAction.Formula(Character, pointsToUse, true),
                _ => points.text
            };

            if (Input.GetKeyDown(ControlsKeys.Back) || ClickButton.KeyUsed.Equals("Back"))
            {
                transform.parent.gameObject.SetActive(false);
            }

            switch(RestButtonsList.Option)
            {
                case "Training" : 
                    TrainingAction.Training(Character, this);
                    break;
                case "Nursing" :
                    NursingAction.Nursing(Character, this);
                    break;
            }
        }

        /**<summary>Set the points window.</summary>*/
        public void SetUp(int nId)
        {
            id = nId;
            pointsToUse = 1;
            restPoints.text = pointsToUse.ToString();
        }

        /**<summary>Get the character who use the points.</summary>*/
        public Character Character => SavesFiles.GetParty().First(p => p.ID == id);
    }
}