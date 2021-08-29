using System;
using System.Data;
using System.Globalization;
using System.Linq;
using Data;
using Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities
{
    
    /**<summary>The Ability that the character can use.</summary>*/
    [Serializable]
    public class Ability : AbilityStatsGenerator
    {
        
        #region ATTRIBUTES
        
        [SerializeField] private string formula = "4*{a.atk} - 2*{a.def} < 0 ? 1 : 4*{a.atk} - 2*{a.def}"; 
        [SerializeField] private AttackType type = AttackType.Blood;
        [SerializeField] private TargetRange range = TargetRange.One;
        [SerializeField] private TargetType target = TargetType.Enemy;
        [SerializeField] private Sprite icon; 
        [SerializeField] private int hits = 1; 
        [SerializeField] private int numberOfTarget = 1; 
        [SerializeField] private int cost = 100; 
        [SerializeField] private int elementID;

        //power increment/down interval/upper interval
        [SerializeField] private float[] stats = {1f, 0.8f, 1.2f};
        [SerializeField] private bool canRepeatRandomTarget; 
        
        [SerializeField] private StatusOf[] statusesToDo = {};
        
        private bool UseCharacterElement  => Element == null;

        #endregion
        
        #region CONSTRUCTORS
        
        /**<summary>Empty ability constructor</summary>*/ 
        public Ability(int id): base(id) { }
      
        /**<summary>Ability clone constructor</summary>*/ 
        public Ability(Ability ability): base(ability)
        { 
            formula = ability.formula; 
            type = ability.type; 
            icon = ability.icon; 
            hits = ability.hits; 
            range = ability.range; 
            numberOfTarget = ability.numberOfTarget; 
            target = ability.target;
            cost = ability.cost; 
            elementID = ability.elementID; 
            canRepeatRandomTarget = ability.canRepeatRandomTarget;
            statusesToDo = new StatusOf[ability.statusesToDo.Length];
            for (int i = 0; i < ability.statusesToDo.Length; i++)
            {
                statusesToDo[i] = ability.statusesToDo[i];
            }
        }

        /**<summary>Ability clone constructor with level.
        <para>The level change the "stats" values.</para></summary>*/ 
        public Ability(Ability ability, int level = 1, int maxLevel = 10) : this(ability)
        {
            for (int i = 0; i < stats.Length; i++)
            {
                stats[i] = Calculate(i, level, maxLevel);
            }
            UpdateAllStatus(level);
            UpdateExperience(level, maxLevel);
        }
        
        #endregion
        
        #region GETTERS & SETTERS
        
        /**<summary>The type of the ability if it damages Karma or Blood points.</summary>*/
        public AttackType Type { get => type; set => type = value; }
        /**<summary>The icon of the ability.</summary>*/
        public Sprite Icon { get => icon; set => icon = value; }
        /**<summary>The number of hits for the target.</summary>*/
        public int Hits { get=> hits; set => hits = value > 0? value : 1; }
        /**<summary>It indicates if you attack random, yourself or your group.</summary>*/
        public TargetRange Range { get => range; set => range = value; }
        /**<summary>It indicates how much targets you can have.</summary>*/
        public int NumberOfTarget { get => numberOfTarget; set => numberOfTarget = value > 0? value : 1; }
        /**<summary>It indicates who you can attack.</summary>*/
        public TargetType Target { get => target; set => target = value; }
        /**<summary>The max interval of the attack.</summary>*/
        public float UpperInterval { get => stats[2]; set => stats[2] = value; }
        /**<summary>The min interval of the attack.</summary>*/
        public float DownInterval { get => stats[1]; set => stats[1] = value; }
        /**<summary>The power up of the attack (extra power).</summary>*/
        public float PowerIncrement { get => stats[0]; set => stats[0] = value; }
        /**<summary>Set the element.</summary>*/
        public int ElementID { set => elementID = value; }
        /**<summary>Set the element of the ability. If it's null,
        then the ability element is being of the character.</summary>*/
        public Element Element => GameData.ElementDB.FindByID(elementID);
        /**<summary>Cost for to do the ability.</summary>*/
        public int Cost { get => cost; set => cost = value > 0? value : 0; }
        /**<summary>Can repeat the target.</summary>*/
        public bool CanRepeatRandomTarget { get => canRepeatRandomTarget; set => canRepeatRandomTarget = value; }
        /**<summary>Formula that indicates the final damage.</summary>*/
        public string Formula { get => formula; set => formula = value; }
        /**<summary>The stats in one array.
        <para>0 -> power increment.</para>
        1 -> down interval.
        <para>2 -> up interval.</para></summary>*/
        public float[] Stats => stats;

        /**<summary>Get all statuses to do with the possibility.</summary>*/
        public StatusOf[] Statuses => statusesToDo;


        /**<summary>
        Return a pair of Icons that represent the attack and element of ability. 
        <para>If the ability hasn't a element, then return empty image.</para>
        </summary>*/
        public Tuple<Sprite, Sprite> AbilityIcons()
        {
            return UseCharacterElement
                ? new Tuple<Sprite, Sprite>(icon, null)
                : new Tuple<Sprite, Sprite>(icon, Element.Icon);
        }
        
        #endregion
        
        #region METHODS
        
        /**<summary>Apply or not the Statuses of tha ability by level to the character "destiny".
        <param name="destiny">Character that received the statuses.</param></summary>*/ 
        public void ApplyAllStatusTo(Character destiny)
        {
            foreach (StatusOf s in Statuses)
            {
                s.ApplyStatusToCharacter(destiny);
            }
            
        }

        /**<summary>
        Add Multiplicity has 2 functionalities:
        <para>- Add new Type with its multiplicity multiplicity.</para>
        - Change the value of existing Type multiplicity.
        <param name="statusID">The ID of the status that the ability can do.</param>
        <param name="possibility">Possibility of that the attack can do it.</param>
        <param name="toChange">Default yes. If it's true, it'll change the value of key if it exits</param>
        </summary>*/ 
        public void AddStatusToDo(int statusID, float possibility, int level, int duration, float[] intervalPlus = null, bool toChange = true)
        {
            if (statusesToDo.Select(s => s.Status.ID).Contains(statusID))
            {
                StatusOf of = statusesToDo.First(s => s.Status.ID == statusID);
                if (!toChange) return;
                of.Possibility = possibility;
                of.Level = level;
                of.Duration = duration;
                if (intervalPlus == null) return;
                for (int i = 0; i < of.IncrementPowerPlus[i]; i++)
                {
                    of.IncrementPowerPlus[i] = intervalPlus[i];
                }
            }
            else
            {
                intervalPlus ??= new float[] {1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1};
                Array.Resize(ref statusesToDo, statusesToDo.Length + 1);
                statusesToDo[statusesToDo.Length-1] = new StatusOf(statusID, level, duration, possibility, intervalPlus);
            } 
        }
     
        /**<summary>
        Delete Multiplicity.
        <para>Remove from the Dictionary the type and the multiplicity passing by parameter</para>
        <param name="statusID">The ID of the status to remove</param>
        </summary>*/ 
        public void RemoveStatus(int statusID) 
        {
            if (!statusesToDo.Select(s => s.Status.ID).Contains(statusID)) return;
            for (int i = Array.IndexOf(statusesToDo, 
                statusesToDo.First(s => s.Status.ID == statusID)); i < statusesToDo.Length-1; i++) 
            { 
                statusesToDo[i] = statusesToDo[i+1]; 
            } 
            Array.Resize(ref statusesToDo, statusesToDo.Length - 1);
        }

        public void UpdateAllStatus(int level)
        {
            foreach (StatusOf status in statusesToDo)
            {
                status.Level = level;
            }
        }
        
        /**<summary>Obtain the statuses.</summary>*/ 
        public StatusOf[] GetAllStatuses()
        {
            return statusesToDo.ToArray();
        }
        
        /**<summary>Obtain the damage value without the intervals or power extra.
        <param name="user">Character that to do the attack.</param>
        <param name="destiny">Character that received the damage.</param></summary>*/ 
        public int BaseDamage(Character user, Character destiny) 
        {
            #region Init
            
            string damage = formula; 
            string[] cTarget = {"a", "b"}; 
            string[] statsOf = {"mbp", "mkp", "atk", "def", "spi", "men", "agi", "cbp", 
                "ckp", "kg", "reb", "rek", "rxb", "rxk"};
            Character[] characters = {user, destiny};
            
            #endregion
            
            #region Replace
            
            for (int i = 0; i < 2; i++) 
            { 
                for (int j = 0; j < user.Main.Length; j++) 
                { 
                    damage = damage.Replace("{" + 
                                            cTarget[i] + "." + 
                                            statsOf[j] + "}", 
                        characters[i].Main[j].ToString());
                        
                        
                } 
                for (int j = 0; j < user.Special.Length; j++) 
                { 
                    damage = damage.Replace("{" + 
                                            cTarget[i] + "." + 
                                            statsOf[j + user.Main.Length] + "}", 
                        characters[i].Special[j].ToString(CultureInfo.InvariantCulture));
                }
                
                damage = damage.Replace("{" + cTarget[i] + ".lv}", characters[i].Level.ToString()); 
                damage = damage.Replace("{" + cTarget[i] + ".cha}", characters[i].Charge.ToString(CultureInfo.InvariantCulture));
            }
            
            #endregion
            
            DataTable dt = new DataTable();
            
            #region Split
            
            string[] booleans = damage.Split(new []{ '?' }, 2);
            
            while (booleans.Length>1) 
            { 
                string[] result = booleans[1].Split(new []{ ':' }, 2); 
                damage = Convert.ToBoolean(dt.Compute(booleans[0], "")) ? result[0] : result[1]; 
                booleans = damage.Split(new []{ '?' }, 2);
            }
            
            #endregion

            int finalDamage = Convert.ToInt32(dt.Compute(damage, ""));

            return finalDamage;

        }

        /**<summary>Get and do the final damage to the character.
        /*<param name="user">Character that to do the attack.</param>
        <param name="destiny">Character that received the damage.</param></summary>*/ 
        public int Damage(Character user, Character destiny)
        {
            int damage = Convert.ToInt32(BaseDamage(user, destiny)
                                         *Random.Range(
                                             DownInterval, 
                                             UpperInterval)*
                                         PowerIncrement);
            damage = damage < 999999999 ? damage > -999999999 ? damage : -999999999 : 999999999;
            ToDoADamage(damage, destiny, user);
            return damage;
        }
            
        /**<summary>Apply the damage to the character.</summary>*/
        public void ToDoADamage(int damage, Character destiny, Character user)
        {
            ApplyAllStatusTo(destiny);
            destiny.Reduce(type, damage);
            if(type == AttackType.AbsorbBlood || type == AttackType.AbsorbKarma)
            {
                user.Reduce(type, Convert.ToInt32(-damage*0.5f));
            }
        }
        
        /**<summary>Get the final value of parameter of "index". </summary>*/ 
        public float Calculate(int index, int level, int maxLevel) 
        {
            return Stats[index]*Mathf.Pow(rate[index],level)*LearningRate(index, level, maxLevel);
        }
        
        /**<summary>Get the learning rate.</summary>*/
        private float LearningRate(int index, int level, int maxLevel)
        { 
            if (level <= 2) return 1; 
            return learning[index] + (1 - learning[index]) * 
                Mathf.Pow(level-1 - Convert.ToSingle((maxLevel-1) / 2), 2) / 
                Mathf.Pow(Convert.ToSingle((maxLevel-1) / 2), 2);
        }
        
        #endregion
    }
}