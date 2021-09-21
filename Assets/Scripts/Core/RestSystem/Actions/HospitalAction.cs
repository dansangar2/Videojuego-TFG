using System;
using Core.Saves;
using Entities;
using UnityEngine;

namespace Core.RestSystem.Actions
{
    public static class HospitalAction
    {
        public static int PeopleInHospital = 0;
        
        public static void ToHospital(Character character)
        {

            for (int i = Array.IndexOf(SavesFiles.GetSave().Party, character); 
                i < SavesFiles.GetSave().Party.Length - 1; i++)
            {
                SavesFiles.GetSave().Party[i] = SavesFiles.GetSave().Party[i + 1];
            }

            character.RecoveryAll();
            character.RestPoints = 0;
            
            PeopleInHospital++;
            
            SavesFiles.GetSave().MoveCharacter(character.ID, SavesFiles.GetSave().Party.Length);

        }
    }
}