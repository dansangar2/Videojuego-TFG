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
            TestParty();
        }
        
        [ContextMenu("Init Party1")] 
        public void TestInit1() 
        { 
            SavesFiles.GetSave().AddCharacter(0, 1, 2, 3, 4);
        }
        
        [ContextMenu("Init Party2")] 
        public void TestInit2() 
        { 
            SavesFiles.GetSave().AddCharacter(1);
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
        
        [ContextMenu("Kill Current")]
        public void KillCurrent() => CurrentTurn.CurrentBloodPoints = 0;
        
        [ContextMenu("Kill All")]
        public void DestroyAll(){
            
            bool t = true;
            foreach (Fighter f in FighterFighting)
            {
                if (!(t && !f.isEnemy)) f.character.CurrentBloodPoints = 0;
                else t = false;
            }

            _state = BattleState.Win;

        }

        [ContextMenu("Set 1BP")]
        public void Sey1All(){
            
            foreach (Fighter f in FighterFighting)
            {
                f.character.CurrentBloodPoints = 1;
            }
            _state = BattleState.Win;

        }

        [ContextMenu("Add status")]
        public void AddStatuses()
        {
            float[] s = new float[]{1,1,1,1,1,1,1,1,1,1,1,1,1,1};
            StatusOf s1 = new StatusOf(0, 1, 5, 1f, s);
            StatusOf s2 = new StatusOf(1, 1, 5, 1f, s);
            StatusOf s3 = new StatusOf(2, 1, 5, 1f, s);
            StatusOf s4 = new StatusOf(3, 1, 5, 1f, s);
            foreach (Fighter f in FighterFighting)
            {
                f.character.AddStatus(s1);
                f.character.AddStatus(s2);
                f.character.AddStatus(s3);
                f.character.AddStatus(s4);
            }
        }
        #endregion
    }
}