using System;
using Core.Controls;
using Entities;
using UnityEngine;

namespace Core.RestSystem.Actions
{
    public static class NursingAction
    {
        /**<summary>Restore KP and BP to the character.</summary>*/
        public static void Nursing(Character character, UseRestPointsInterface rest)
        {
            if (!Input.GetKeyDown(ControlsKeys.Ok)) return;
            character.RestPoints -= rest.pointsToUse;
            character.ReduceCurrentBlood(-Formula(character, rest.pointsToUse));
            rest.transform.parent.gameObject.SetActive(false);
            character.ClearStatuses();
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
                            character.MaxBloodPoints * 0.75f * v
                            )
                    );
        }
    }
}