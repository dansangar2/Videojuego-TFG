using Core.Battle.StatusesUI;
using Core.ButtonsSystem.ButtonType;
using Core.Saves;
using Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Battle
{
    /**<summary>Item that represent one character in battle with other necessary items.</summary>*/
    public class Fighter : MonoBehaviour
    {
        
        #region ATTRIBUTES

        /**<summary>The ID of the fighter.</summary>*/
        public int id;
        /**<summary>The ref of the character that represents.</summary>*/
        public Character character;
        //public MeshFilter meshFilter;
        //public MeshRenderer meshRenderer;
        //public MeshCollider meshCollider;
        /**<summary>Indicates if it's enemy.</summary>*/
        public bool isEnemy;
        /**<summary>The member slot that shown the current status.</summary>*/
        public MemberHUDButton member;

        /**<summary>The collider for click.</summary>*/
        public new BoxCollider2D collider;
        
        //**<summary>The colors that the character uses for blink the render color.</summary>*/
        //private Color[] _marksColors;
        //**<summary>The time that the colors wait to change.</summary>*/
        //private const float Wait = 0.9f;
        //**<summary>The index with the color that is using.</summary>*/
        //private int _i;

        //public static float Velocity = 1f;

        /*private void Awake()
        {
            float sec = 0.2f;
            float main = 0.9f;
            /*_marksColors = new []{
                new Color(sec, sec, main), 
                new Color(sec, main, sec), 
                new Color(main, sec, sec)
            };*/
        //}*/
        
        #endregion
        
        #region CONSTRUCTORS
        
        /**<summary>Character Fighter constructor individual.</summary>*/
        public Fighter(int pos, MemberHUDButton member, bool isEnemy
            //, MeshFilter meshFilter, MeshRenderer meshRenderer, MeshCollider meshCollider
            )
        {
            id = pos;
            this.isEnemy = isEnemy;
            character = SavesFiles.GetCharacterOfParty(pos);
            member.SetUp(character, isEnemy);
            this.member = Instantiate(member, GetComponentInChildren<GridLayoutGroup>().gameObject.transform);
            this.member.gameObject.SetActive(false);
            //this.meshFilter = meshFilter;
            //this.meshRenderer = meshRenderer;
            //this.meshCollider = meshCollider;
            //this.meshFilter.mesh = character.Model;
        }

        #region old constructors

        //**<summary>Character Fighter constructor group.</summary>*/
        /*public Fighter(int pos, MemberHUDButton memberBase, GridLayoutGroup set 
            //, MeshFilter[] meshFilters, MeshRenderer[] meshRenderers, MeshCollider[] meshColliders
            )
        {
            id = pos;
            isEnemy = false;
            character = SavesFiles.GetCharacterOfParty(pos);
            memberBase.SetUp(character, isEnemy);
            member = Instantiate(memberBase, set.gameObject.transform);
            collider = member.GetComponent<BoxCollider2D>();
            //meshFilter = meshFilters[pos];
            //meshRenderer = meshRenderers[pos];
            //meshCollider = meshColliders[pos];
            //meshFilter.mesh = character.Model;
        }*/
        
        //**<summary>Enemy Fighter constructor individual.</summary>*/
        /*public Fighter(int enemyId
            //, MeshFilter meshFilter, MeshRenderer meshRenderer, MeshCollider meshCollider
            )
        {
            isEnemy = true;
            member.SetUp(character, isEnemy);
            character = SavesFiles.GetSave().GetEnemy(enemyId);
            collider = member.GetComponent<BoxCollider2D>();
            //this.meshFilter = meshFilter;
            //this.meshRenderer = meshRenderer;
            //this.meshCollider = meshCollider;
            //this.meshFilter.mesh = character.Model;
        }*/

        //**<summary>Enemy Fighter constructor group.</summary>*/
        /*public Fighter(int pos, int[] enemiesId, MemberHUDButton member
            //, MeshFilter[] meshFilters, MeshRenderer[] meshRenderers, MeshCollider[] meshColliders
            )
        {
            id = pos;
            isEnemy = true;
            member.SetUp(character, isEnemy);
            character = SavesFiles.GetSave().GetEnemy(enemiesId[pos % enemiesId.Length]);
            collider = member.GetComponent<BoxCollider2D>();
            //meshFilter = meshFilters[pos];
            //meshRenderer = meshRenderers[pos];
            //meshCollider = meshColliders[pos];
            //meshFilter.mesh = character.Model;
        }*/

        #endregion

        #endregion
        
        #region SETTERS METHODS
        
        /**<summary>Enemy Fighter setter group.</summary>*/
        public void SetData(int pos, int[] enemiesId, StatusUI statuses, MemberHUDButton memberBase, Transform panel
            //, MeshFilter[] meshFilters, MeshRenderer[] meshRenderers, MeshCollider[] meshColliders
            )
        {
            id = pos;
            isEnemy = true;
            character = new Character(SavesFiles.GetSave().GetEnemy(enemiesId[pos % enemiesId.Length]));
            statuses.SetUp(this);
            member = Instantiate(memberBase, panel.transform);
            member.SetUp(character, isEnemy);
            var transform1 = member.transform;
            var position = transform1.position;
            Instantiate(statuses, new Vector3(
                    position.x + 125,
                    position.y - 165, 
                    position.z), Quaternion.identity, 
                transform1);
            collider = member.GetComponent<BoxCollider2D>();
            /*Instantiate(statuses, new Vector3(
                meshFilters[pos].transform.position.x + 300,//- (pos % (meshFilters.Length-enemiesId.Length))*100,
                meshFilters[pos].transform.position.y, 
                meshFilters[pos].transform.position.z - 100), Quaternion.identity);*/
            //Instantiate(statuses, member.transform);
            //meshFilter = meshFilters[pos];
            //meshRenderer = meshRenderers[pos];
            //meshCollider = meshColliders[pos];
            //meshFilter.mesh = character.Model;
        }

        /**<summary>Character Fighter setter.</summary>*/
        public void SetData(int pos, StatusUI statuses, MemberHUDButton memberBase, Transform panel
            //, MeshFilter[] meshFilters, MeshRenderer[] meshRenderers, MeshCollider[] meshColliders
            )
        {
            id = pos;
            isEnemy = false;
            character = SavesFiles.GetCharacterOfParty(pos);
            member = Instantiate(memberBase, panel.transform);
            member.SetUp(character, isEnemy);
            member.gameObject.SetActive(false);
            statuses.SetUp(this);
            var transform1 = member.transform;
            var position = transform1.position;
            Instantiate(statuses, new Vector3(
                position.x + 125,
                position.y + 165, 
                position.z), Quaternion.identity, 
                transform1);
            collider = member.GetComponent<BoxCollider2D>();
            /*Instantiate(statuses, new Vector3(
                meshFilters[pos].transform.position.x - 300,//+ (pos % 3)*100,
                meshFilters[pos].transform.position.y, 
                meshFilters[pos].transform.position.z - 100), Quaternion.identity);*/
            //Instantiate(statuses, member.transform);
            //meshFilter = meshFilters[pos];
            //meshRenderer = meshRenderers[pos];
            //meshCollider = meshColliders[pos];
            //meshFilter.mesh = character.Model;
            
        }

        #endregion
        
        #region METHODS
        
        //private float _lerp;
        /**<summary>Mark in red the color of the member border if is pressed.</summary>*/
        public void CharacterBlink()
        {
            if(!Input.GetMouseButton(0)) member.ToMark(true, Color.red);
            //float vel = Velocity / 60f;
            //Material render = meshRenderer.material;
            //render.color = Color.Lerp(render.color, _marksColors[_i], vel);
            
            //_lerp = Mathf.Lerp(_lerp, 1f, vel);
            //if (_lerp <= Wait) return;
            //_lerp = 1 - Wait;
            //_i++;
            //_i %= _marksColors.Length;

        }

        /**<summary>Set the border of the member slot if it isn't KO.</summary>*/
        public void CharacterMark(bool isMark = false)
        {
            //Material render = meshRenderer.material;
            //if(!isEnemy) member.UpdateUI();
            if(character.IsKo()) return;
            //render.color = isMark ? Color.black : Color.white;
            member.ToMark(isMark, Color.red);
        }

        /**<summary>Set to 0 de BP of the character. It means that dead and cannot choose.</summary>*/
        public void SetKo()
        {
            character.SetKo();
            //meshRenderer.material.color = Color.red;
            member.ToMark(true, Color.grey);
            member.CanPress(false);
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