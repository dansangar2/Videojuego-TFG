using UnityEngine;
using UnityEngine.UI;
using Button = Core.Buttons.Button;

namespace Core.HUDs
{
    public class BaseHUD : Button
    {
        [SerializeField] protected int id;
        public Text itemName;
        public Image itemIcon;
        public Image elementIcon;

        public new void Start()
        {
            IsBlink();
        }
        
        public int GetMemberID()
        { 
            return id;
        }
        public void SetUp(int member) 
        { 
            id = member;
        }

        public void Remove()
        {
            Destroy(gameObject);
            Destroy(this);
        }
        
    }
}