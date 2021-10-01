using System;
using Core.Saves;
using Entities;

namespace Core.RestSystem.Actions
{
    public static class HospitalAction
    {
        /**<summary>Check the characters in the hospital.</summary>*/
        public static int PeopleInHospital;
        
        /**<summary>Go to the hospital.</summary>*/
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