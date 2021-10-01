using System;
using Entities;
using UnityEngine;

namespace Core.RestSystem.Actions
{
    public static class RestAction
    {
        /**<summary>Rest action.</summary>*/
        public static void ToRest(Character character)
        {
            float percentage = 0f;
            
            switch (character.RestPoints)
            {
                case 0:
                    percentage = 0.1f;
                    break;
                case 1:
                    percentage = 0.2f;
                    break;
                case 2:
                    percentage = 0.4f;
                    break;
                case 3:
                    percentage = 0.6f;
                    break;
                case 4:
                    percentage = 0.8f;
                    break;
                case 5:
                    percentage = 1f;
                    break;
            }
            
            character.RestPoints = 0;
            
            character.ReduceCurrentBlood(-Convert.ToInt32(
                Mathf.Floor(character.MaxBloodPoints*percentage)));
            
            //character.ReduceCurrentKarma(-Convert.ToInt32(
            //    Mathf.Floor(character.MaxKarmaPoints*percentage)));
            
        }
    }
}