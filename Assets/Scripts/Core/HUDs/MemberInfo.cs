using System;
using UnityEngine;

namespace Core.HUDs
{
    [Serializable]
    public class MemberInfo
    {

        [SerializeField] private int id;
        [SerializeField] private int characterID;

        public MemberInfo(int id) { this.id = id; }

        public void SetMember(int characterId)
        {
            characterID = characterId;
        }

        public int GetCharacterID() { return characterID; }

        public void SetID(int nId) { id = nId; }
        public int GetID() { return id; }

    }
}