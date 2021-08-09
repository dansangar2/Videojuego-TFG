using Core.Controls;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Buttons
{/*
    public class ButtonKey : Button
    {
        
        private KeyCode? _key;
        public int keyIndex;
        public bool tooChange;
        private Text _text;

        new void Start()
        {
            _text = GetComponentInChildren<Text>();
            base.Start();
            _key = ControlsKeys.GetKey(keyIndex);
            GetComponentInChildren<Text>().text = _key.ToString();
        }
        
        private new void Update()
        {
            
            if (tooChange)
            {
                Stop = true;
                foreach(KeyCode k in System.Enum.GetValues(typeof(KeyCode))){
                    if(Input.GetKeyDown(k)){
                        ControlsKeys.SetKey(keyIndex, k);
                        tooChange = false;
                    }
                }
            }
            else
            {
                Stop = false;
                base.Update();
                if (Input.GetKeyDown(ControlsKeys.Ok) && IsSelect)
                {
                    tooChange = true;
                    ButtonImage.color = Color.red;
                }
            }
            
            _key = ControlsKeys.GetKey(keyIndex);
            _text.text = _key.ToString();
        }
    }*/
}