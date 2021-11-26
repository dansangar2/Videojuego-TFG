using System;
using Core.Controls;
using Entities;
using UnityEngine;

namespace Core.RestSystem.Actions
{
    public static class TrainingAction
    {
        
        /**<summary>Gain experience with the rest points.</summary>*/
        public static void Training(Character character, UseRestPointsInterface rest)
        {
            if (!(Input.GetKeyDown(ControlsKeys.Ok)|| ClickButton.KeyUsed.Equals("Ok"))) return;
            character.RestPoints -= rest.pointsToUse;
            character.GainExperience(Formula(character, rest.pointsToUse));
            rest.transform.parent.gameObject.SetActive(false);
        }
        
        public static int Formula(Character character, int points)
        {
            float v = points switch
            {
                1 => 0.1f,
                2 => 0.25f,
                3 => 0.45f,
                4 => 0.7f,
                _ => 1f
            };
            return Convert.ToInt32(
                Mathf.Round(
                    character.NedExp * v
                )
            );
        }
    }
}