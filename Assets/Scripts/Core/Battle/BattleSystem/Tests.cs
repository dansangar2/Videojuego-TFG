using Core.Saves;
using Entities;
using UnityEngine;

namespace Core.Battle.BattleSystem
{
    public partial class BattleSystem
    {
        #region TESTS
        
        [ContextMenu("Set Up")] 
        public void TestSetUp() 
        { 
            TestDelete(); 
            TestInit1(); 
            TestLoad(); 
            //TestParty();
        }
        
        [ContextMenu("Init Party1")] 
        public void TestInit1() 
        { 
            SavesFiles.GetSave().AddCharacter(3, 0, 2, 1); 
            SavesFiles.SaveData();
        }
        
        [ContextMenu("Init Party2")] 
        public void TestInit2() 
        { 
            SavesFiles.GetSave().AddCharacter(1); 
            SavesFiles.SaveData();
        }
                
        [ContextMenu("Load data")] 
        public void TestLoad() 
        {
            SavesFiles.LoadData();
        }
        
        [ContextMenu("Delete")] 
        public void TestDelete() 
        { 
            SavesFiles.Init();
        }
                
        [ContextMenu("Print current party")] 
        public void TestParty() 
        { 
            foreach (Character cha in SavesFiles.GetSave().Party) 
            { 
                Debug.Log(cha);
            }
        }
                
        #endregion
    }
}