using Core.ButtonsSystem.ButtonType;
using Core.Saves;
using Data;
using Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Battle
{
    public class Fighter : MonoBehaviour
    {
        public int id;
        public Character character;
        public MeshFilter meshFilter;
        public MeshRenderer meshRenderer;
        public MeshCollider meshCollider;
        public bool isEnemy;
        public MemberHUDButton member;

        private bool _isTurn;
        
        #region CONSTRUCTORS

        public Fighter(int enemyId, MeshFilter meshFilter, MeshRenderer meshRenderer, MeshCollider meshCollider)
        {
            isEnemy = true;
            character = GameData.EnemyDB.FindByID(enemyId);
            this.meshFilter = meshFilter;
            this.meshRenderer = meshRenderer;
            this.meshCollider = meshCollider;
            this.meshFilter.mesh = character.Model;
        }

        public Fighter(int pos, int[] enemiesId, MeshFilter[] meshFilters, MeshRenderer[] meshRenderers, MeshCollider[] meshColliders)
        {
            id = pos;
            isEnemy = true;
            character = GameData.EnemyDB.FindByID(enemiesId[pos % enemiesId.Length]);
            meshFilter = meshFilters[pos];
            meshRenderer = meshRenderers[pos];
            meshCollider = meshColliders[pos];
            meshFilter.mesh = character.Model;
        }
        
        public Fighter(int pos, MemberHUDButton member, MeshFilter meshFilter, MeshRenderer meshRenderer, MeshCollider meshCollider)
        {
            id = pos;
            isEnemy = false;
            character = SavesFiles.GetCharacterOfParty(pos);
            member.SetUp(character.ID);
            this.member = Instantiate(member, GetComponentInChildren<GridLayoutGroup>().gameObject.transform);
            this.member.gameObject.SetActive(false);
            this.meshFilter = meshFilter;
            this.meshRenderer = meshRenderer;
            this.meshCollider = meshCollider;
            this.meshFilter.mesh = character.Model;
        }

        public Fighter(int pos, MemberHUDButton memberBase, GridLayoutGroup set, MeshFilter[] meshFilters, MeshRenderer[] meshRenderers, MeshCollider[] meshColliders)
        {
            id = pos;
            isEnemy = false;
            character = SavesFiles.GetCharacterOfParty(pos);
            memberBase.SetUp(character.ID);
            member = Instantiate(memberBase, set.gameObject.transform);
            meshFilter = meshFilters[pos];
            meshRenderer = meshRenderers[pos];
            meshCollider = meshColliders[pos];
            meshFilter.mesh = character.Model;
        }

        #endregion
        
        #region METHODS

        public void SetData(int enemyId, MeshFilter nMeshFilter, MeshRenderer nMeshRenderer, MeshCollider nMeshCollider)
        {
            isEnemy = true;
            character = GameData.EnemyDB.FindByID(enemyId);
            meshFilter = nMeshFilter;
            meshRenderer = nMeshRenderer;
            meshCollider = nMeshCollider;
            meshFilter.mesh = character.Model;
        }

        public void SetData(int pos, int[] enemiesId, MeshFilter[] meshFilters, MeshRenderer[] meshRenderers, MeshCollider[] meshColliders)
        {
            id = pos;
            isEnemy = true;
            character = new Character(GameData.EnemyDB.FindByID(enemiesId[pos % enemiesId.Length]));
            meshFilter = meshFilters[pos];
            meshRenderer = meshRenderers[pos];
            meshCollider = meshColliders[pos];
            meshFilter.mesh = character.Model;
        }
        
        public void SetData(int pos, MemberHUDButton nMember, MeshFilter nMeshFilter, MeshRenderer nMeshRenderer, MeshCollider nMeshCollider)
        {
            id = pos;
            isEnemy = false;
            character = SavesFiles.GetCharacterOfParty(pos);
            member.SetUp(character.ID);
            member = Instantiate(nMember, GetComponentInChildren<GridLayoutGroup>().gameObject.transform);
            meshFilter = nMeshFilter;
            meshRenderer = nMeshRenderer;
            meshCollider = nMeshCollider;
            meshFilter.mesh = character.Model;
        }

        public void SetData(int pos, MemberHUDButton memberBase, GridLayoutGroup set, MeshFilter[] meshFilters, MeshRenderer[] meshRenderers, MeshCollider[] meshColliders)
        {
            id = pos;
            isEnemy = false;
            character = SavesFiles.GetCharacterOfParty(pos);
            memberBase.SetUp(character.ID);
            member = Instantiate(memberBase, set.gameObject.transform);
            member.gameObject.SetActive(false);
            meshFilter = meshFilters[pos];
            meshRenderer = meshRenderers[pos];
            meshCollider = meshColliders[pos];
            meshFilter.mesh = character.Model;
        }

        public void CharacterBlink(float velocity = 0.003f)
        {
            Material render = meshRenderer.material;
            if (CheckColorChange()) _isTurn = !_isTurn;

            switch (_isTurn) 
            { 
                case true:
                    render.color = new Color(
                        render.color.r - velocity, 
                        render.color.g - velocity, 
                        render.color.b - velocity, 
                        render.color.a);
                    break;
                case false: 
                    render.color = new Color(
                        render.color.r + velocity, 
                        render.color.g + velocity, 
                        render.color.b + velocity, 
                        render.color.a);
                    break;
            }
        }

        public void CharacterMark(bool isMark = false)
        {
            Material render = meshRenderer.material;
            _isTurn = false;
            render.color = isMark ? Color.black
                : Color.white;
        }
        
        private bool CheckColorChange()
        {
            Material render = meshRenderer.material;
            return (render.color.r >= 0.9 || render.color.g >= 0.9 || render.color.b >= 0.9) && !_isTurn
                   || (render.color.r <= 0.2 || render.color.g <= 0.2 || render.color.b <= 0.2) && _isTurn;
        }
        
        #endregion
        
    }
}