using System;
using System.Linq;
using Data;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Battle.TurnsUI
{
    /**<summary>Show the 5 next turns in the battle.</summary>*/
    public class TurnUI : MonoBehaviour
    {
        public Image slot1;
        public Image slot2;
        public Image slot3;
        public Image slot4;
        public Image slot5;

        /**<summary>Current Fighters.</summary>*/
        private Fighter[] _character;

        public void Update()
        {
            try
            {
                slot1.gameObject.SetActive(true);
                slot1.sprite = _character[0].isEnemy ? 
                    GameData.EnemyDB.FindByID(_character[0].character.ID).Face : 
                    GameData.CharacterDB.FindByID(_character[0].character.ID).Face;

            }
            catch (Exception)
            {
                slot1.gameObject.SetActive(false);
            }

            try
            {
                slot2.gameObject.SetActive(true);
                slot2.sprite = _character[1].isEnemy ? 
                    GameData.EnemyDB.FindByID(_character[1].character.ID).Face : 
                    GameData.CharacterDB.FindByID(_character[1].character.ID).Face;
            }
            catch (Exception)
            {
                slot2.gameObject.SetActive(false);
            }
            
            try
            {
                slot3.gameObject.SetActive(true);
                slot3.sprite = _character[2].isEnemy ? 
                    GameData.EnemyDB.FindByID(_character[2].character.ID).Face : 
                    GameData.CharacterDB.FindByID(_character[2].character.ID).Face;


            }
            catch (Exception)
            {
                slot3.gameObject.SetActive(false);
            }
            
            try
            {
                slot4.gameObject.SetActive(true);
                slot4.sprite = _character[3].isEnemy ? 
                    GameData.EnemyDB.FindByID(_character[3].character.ID).Face : 
                    GameData.CharacterDB.FindByID(_character[3].character.ID).Face;
            }
            catch (Exception)
            {
                slot4.gameObject.SetActive(false);
            }
            
            try
            {
                slot5.gameObject.SetActive(true);
                slot5.sprite = _character[4].isEnemy ? 
                    GameData.EnemyDB.FindByID(_character[4].character.ID).Face : 
                    GameData.CharacterDB.FindByID(_character[4].character.ID).Face;
                
            }
            catch (Exception)
            {
                slot5.gameObject.SetActive(false);
            }
        }

        /**<summary>Set new 5 fighters by character IDs.</summary>
        <param name="characters">The fighters of the battle.</param>
        <param name="turns">The IDs of the character.</param>*/
        public void SetUp(Fighter[] characters, int[] turns)
        {
            _character = new Fighter[turns.Length];
            for (int i = 0; i < turns.Length; i++)
            {
                _character[i] = characters[turns[i]];
            }
        }

        /**<summary>Remove the first element.</summary>*/
        public void QuitOld()
        {
            _character = _character.Skip(1).Where(c => !c.character.IsKo()).ToArray();
        }
    }
}