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
    public class UseRestPointsInterface : MonoBehaviour
    {
        [SerializeField] private int id;
        public Text restPoints;
        public int pointsToUse;
        public Text points;

        public void Awake()
        {
            Text legend = points.transform.parent.Find("Legend").GetComponent<Text>();
            legend.text = RestButtonsList.Option switch
            {
                "Training" => "Exp. To Gain",
                "Nursing" => "BP To Recover",
                _ => legend.text
            };
        }

        public void Update()
        {
            if (Input.GetKeyDown(ControlsKeys.MoveUp))
            {
                pointsToUse++;
                if (pointsToUse > Character.RestPoints) pointsToUse = 1;
                if (pointsToUse < 1) pointsToUse = Character.RestPoints;
            }else if (Input.GetKeyDown(ControlsKeys.MoveDown))
            {
                pointsToUse--;
                if (pointsToUse > Character.RestPoints) pointsToUse = 1;
                if (pointsToUse < 1) pointsToUse = Character.RestPoints;
                
            }

            restPoints.text = pointsToUse.ToString();

            points.text = RestButtonsList.Option switch
            {
                "Training" => TrainingAction.Formula(Character, pointsToUse).ToString(),
                "Nursing" => NursingAction.Formula(Character, pointsToUse).ToString(),
                _ => points.text
            };

            if (Input.GetKeyDown(ControlsKeys.Back))
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