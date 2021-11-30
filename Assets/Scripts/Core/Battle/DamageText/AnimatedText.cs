using Enums;
using UnityEngine;
using UnityEngine.UI;
//using Random = UnityEngine.Random;

namespace Core.Battle.DamageText
{
    /**<summary>Text that has a animation with "fallen" and "throw" effect.</summary>*/
    public class AnimatedText : Text
    {
        #region ATTRIBUTES

        /**<summary>The direction of the text throws.</summary>*/
        public Vector3 hitDirection;
        //public Vector3 position;
        /**<summary>It indicates what position will create the text.</summary>*/
        public Vector3 posDirection;
        /**<summary>It indicates that the text fall.</summary>*/
        public bool fall;
        /**<summary>Direction index.</summary>*/
        //**<summary>1 if the fighter is a enemy, -1 if it's a party member.</summary>*/
        public int direction = 1;
        /**<summary>The force of how much speed the text will throw.</summary>*/
        public float force = 1f;
        
        /**<summary>The current position of the object.</summary>*/
        private float _pos = 0.01f;
        /**<summary>The init position of the object.</summary>*/
        private float _initPos;
        /**<summary>The acceleration of the text.</summary>*/
        private float _a = 9.8f;
        /**<summary>The velocity of the text.</summary>*/
        private float _v = 10f;
        /**<summary>The time of the </summary>*/
        private float _t = 1f;

        #endregion

        protected override void Awake()
        {
            direction = 1;
            //Vector3 forward = Camera.main.transform.forward;
            float finalForce = Random.Range(//force / 2,
                -5f * direction, 5f * direction) * force;
            _initPos = _pos;
            
            //transform.LookAt(Camera.main.transform);
            //transform.rotation = Quaternion.LookRotation(forward);
            base.Awake();
            //If it's a damage, then the x pos will be a random number, it depends of force
            //Else, it won't have direction.
            posDirection = fall ? Vector3.Cross(new Vector3(
                        finalForce, finalForce, finalForce), 
                    //forward
                    transform.forward) 
                : new Vector3(0,0,0);
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }

        /**<summary>Destroy the item after some seconds.</summary>*/
        protected override void Start()
        {
            base.Start();
            float time = fall ? 3f : 1.5f;
            //Destroy(transform.parent.gameObject, time);
            Destroy(this, time);
        }
        
        /**<summary>The animation process.</summary>*/
        private void FixedUpdate()
        {
            Vector3 position1 = transform.position;
            //It move the text
            position1 = Vector3.MoveTowards(position1,
                position1 + hitDirection * (_pos * direction) + posDirection, 1000*Time.fixedDeltaTime);
            transform.position = position1;
            _t += 0.02f;
            if (fall || _pos > 0)
            {
                _pos = _a * Mathf.Pow(_t, 2) * (-1f / 2) + _v * force + _t + _initPos;
                if (_pos < 0f) hitDirection = transform.up;
            }
            //else if (_pos > 0) _pos = _a * Mathf.Pow(_t, 2)* (-1f / 2) + _v * force + _t + _initPos;
            else _pos =0;
            
        }

        #region ANIMATION

        /**<summary>Instance a new Text with the damage number.</summary>*/
        public void SetDamage(int damage, Fighter fighter, AttackType type)
        {
            Transform transform1 = fighter.member.transform;
            //Vector3 position = transform1.position;
            hitDirection = transform1.up;
            
            fall = damage >= 0;
    
            text = Mathf.Abs(damage).ToString();
            if(type == AttackType.Blood || type == AttackType.AbsorbBlood) 
            {
                if (fall) force = GetThrow(fighter.character.MaxKarmaPoints,  damage);
                else force = 1.25f;
                color = damage < 0 ? Color.green : Color.red;
            }
            else
            {
                if (fall) force = GetThrow(fighter.character.MaxKarmaPoints,  damage);
                else force = 1.25f;
                color = damage < 0 ? Color.magenta : Color.yellow;
            }
            //direction = fighter.isEnemy ? 1 : -1;

            transform.localScale = new Vector3(5, 5, 5);
            
            Instantiate(this, fighter.member.transform);
            //Instantiate(transform.parent.gameObject, position, Quaternion.identity);
        }
        
        /**<summary>Instance a new Text with the experience number.</summary>*/
        public void GainExp(int exp, Fighter fighter)
        {
            Transform transform1 = fighter.member.transform;
            //Transform transform1 = fighter.meshFilter.transform;
            //Vector3 position = transform1.position;
            //hitDirection = -transform1.right;
            hitDirection = transform1.up;
            
            fall = false;

            text = exp.ToString();
            force = 1.3f;
            color = Color.blue;
            
            Instantiate(this, fighter.member.transform);
            //Instantiate(transform.parent.gameObject, position, Quaternion.identity);
        }

        #endregion

        #region METHODS

        /**<summary>Get the throw force.</summary>*/
        private float GetThrow(float fighter, float damage) => 
            Mathf.Max(Mathf.Min((fighter + damage)/fighter, 1.3f), 0.8f);

        #endregion
    }
}