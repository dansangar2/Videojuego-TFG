using System;
using System.Linq;
using Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Battle.StatusesUI
{
    /**<summary>The status interface that shown 3 statuses.</summary>*/
    public class StatusUI : MonoBehaviour
    {
        
        /**<summary>Image on the left.</summary>*/
        public Image image1;
        /**<summary>Image on the center.</summary>*/
        public Image image2;
        /**<summary>Image on the right.</summary>*/
        public Image image3;
        
        /**<summary>Name on the left.</summary>*/
        public Text text1;
        /**<summary>Name on the center.</summary>*/
        public Text text2;
        /**<summary>Name on the right.</summary>*/
        public Text text3;

        /**<summary>The list of sprites of the statuses of the character.</summary>*/
        [SerializeField] private Status[] statuses = {};

        /**<summary>The character that has the statuses.</summary>*/
        [SerializeField] private Fighter character;

        /**<summary>The fragment of the statuses list that will show.</summary>*/
        private int _i;

        /**<summary>The seconds for the next 3 statuses.</summary>*/
        private float _seconds;

        private void Awake()
        {
            image1 = transform.GetChild(0).Find("Status1").GetComponent<Image>();
            image2 = transform.GetChild(0).Find("Status2").GetComponent<Image>();
            image3 = transform.GetChild(0).Find("Status3").GetComponent<Image>();
            //Vector3 forward = Camera.main.transform.forward;
            
            //transform.LookAt(Camera.main.transform);
            //transform.rotation = Quaternion.LookRotation(forward);
        }

        public void Update()
        {
            UpdateUI();
            
            try
            {
                image1.sprite = statuses[_i].Icon;
                image1.gameObject.SetActive(true);
                text1.text = statuses[_i].Name;
            }
            catch(Exception) 
            { 
                image1.gameObject.SetActive(false);
                text1.text = "";
            }
            try 
            { 
                image2.sprite = statuses[_i+1].Icon;
                image2.gameObject.SetActive(true);
                text2.text = statuses[_i + 1].Name;
            }
            catch(Exception) 
            { 
                if(statuses.Length<1)
                {
                    image2.gameObject.SetActive(false);
                    text2.text = "";
                }
            }
            
            try 
            {
                image3.sprite = statuses[_i+2].Icon;
                image3.gameObject.SetActive(true);
                text3.text = statuses[_i + 2].Name;
            }
            catch(Exception) 
            { 
                if(statuses.Length<2)
                {
                    image3.gameObject.SetActive(false);
                    text3.text = "";
                }
            }
            
            _seconds += Time.deltaTime;
            if (_seconds <= 1) return;
            
            _i += 3; 
            if (_i >= statuses.Length) _i = 0;
            _seconds = 0;

        }

        /**<summary>Set the character who will get the statuses.</summary>*/
        public void SetUp(Fighter fighter)
        {
            character = fighter;
            UpdateUI();
        }

        /**<summary>Update the UI.</summary>*/
        public void UpdateUI()
        {
            if (character?.character.Statuses == null) return;
            statuses = character.character.Statuses.Select(s => s.Status).ToArray();
        }
    }
}