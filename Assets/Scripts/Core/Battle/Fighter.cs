using System.Linq;
using Core.Battle.StatusesUI;
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
        
        #region ATTRIBUTES

        public int id;
        public Character character;
        public MeshFilter meshFilter;
        public MeshRenderer meshRenderer;
        public MeshCollider meshCollider;
        public bool isEnemy;
        /**<summary>The member.</summary>*/
        public MemberHUDButton member;
        
        /**<summary>The colors that the character uses for blink the render color.</summary>*/
        private Color[] _marksColors;
        /**<summary>The time that the colors wait to change.</summary>*/
        private const float Wait = 0.9f;
        /**<summary>The index with the color that is using.</summary>*/
        private int _i;

        public static float Velocity = 1f;

        private void Awake()
        {
            float sec = 0.2f;
            float main = 0.9f;
            _marksColors = new []{
                new Color(sec, sec, main), 
                new Color(sec, main, sec), 
                new Color(main, sec, sec)
            };
        }
        
        #endregion
        
        #region CONSTRUCTORS

        public Fighter(int enemyId, MeshFilter meshFilter, MeshRenderer meshRenderer, MeshCollider meshCollider)
        {
            isEnemy = true;
            character = SavesFiles.GetSave().GetEnemy(enemyId);
            this.meshFilter = meshFilter;
            this.meshRenderer = meshRenderer;
            this.meshCollider = meshCollider;
            this.meshFilter.mesh = character.Model;
        }

        public Fighter(int pos, int[] enemiesId, MeshFilter[] meshFilters, MeshRenderer[] meshRenderers, MeshCollider[] meshColliders)
        {
            id = pos;
            isEnemy = true;
            character = SavesFiles.GetSave().GetEnemy(enemiesId[pos % enemiesId.Length]);
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
        
        #region SETTERS METHODS

        public void SetData(int pos, int[] enemiesId, StatusUI statuses, MeshFilter[] meshFilters, MeshRenderer[] meshRenderers, MeshCollider[] meshColliders)
        {
            id = pos;
            isEnemy = true;
            character = new Character(SavesFiles.GetSave().GetEnemy(enemiesId[pos % enemiesId.Length]));
            statuses.SetUp(this);
            Instantiate(statuses, new Vector3(
                meshFilters[pos].transform.position.x + 300,//- (pos % (meshFilters.Length-enemiesId.Length))*100,
                meshFilters[pos].transform.position.y, 
                meshFilters[pos].transform.position.z - 100), Quaternion.identity);
            meshFilter = meshFilters[pos];
            meshRenderer = meshRenderers[pos];
            meshCollider = meshColliders[pos];
            meshFilter.mesh = character.Model;
        }

        public void SetData(int pos, StatusUI statuses, MemberHUDButton memberBase, GridLayoutGroup set, MeshFilter[] meshFilters, MeshRenderer[] meshRenderers, MeshCollider[] meshColliders)
        {
            id = pos;
            isEnemy = false;
            character = SavesFiles.GetCharacterOfParty(pos);
            memberBase.SetUp(character.ID);
            member = Instantiate(memberBase, set.gameObject.transform);
            member.gameObject.SetActive(false);
            statuses.SetUp(this);
            Instantiate(statuses, new Vector3(
                meshFilters[pos].transform.position.x - 300,//+ (pos % 3)*100,
                meshFilters[pos].transform.position.y, 
                meshFilters[pos].transform.position.z - 100), Quaternion.identity);
            meshFilter = meshFilters[pos];
            meshRenderer = meshRenderers[pos];
            meshCollider = meshColliders[pos];
            meshFilter.mesh = character.Model;
            
        }

        #endregion
        
        #region METHODS
        
        private float _lerp;
        public void CharacterBlink()
        {
            float vel = Velocity / 60f;
            Material render = meshRenderer.material;
            render.color = Color.Lerp(render.color, _marksColors[_i], vel);
            
            _lerp = Mathf.Lerp(_lerp, 1f, vel);
            if (_lerp <= Wait) return;
            _lerp = 1 - Wait;
            _i++;
            _i %= _marksColors.Length;

        }

        public void CharacterMark(bool isMark = false)
        {
            Material render = meshRenderer.material;
            if(!isEnemy) member.UpdateUI();
            if(character.IsKo()) return;
            render.color = isMark ? Color.black : Color.white;
        }

        public void SetKo()
        {
            character.SetKo();
            meshRenderer.material.color = Color.red;
        }
        
        /*private bool CheckColorChange()
        {
            Material render = meshRenderer.material;
            return (render.color.r >= 0.9 || render.color.g >= 0.9 || render.color.b >= 0.9) && !_isTurn
                   || (render.color.r <= 0.2 || render.color.g <= 0.2 || render.color.b <= 0.2) && _isTurn;
        }*/
        
        #endregion
        
    }
}