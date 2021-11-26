using System;
using System.Linq;
using Core.ButtonsSystem.ButtonType;
using Core.Controls;
using UnityEngine;

namespace Core.ButtonsSystem.ButtonList
{
    /**<sumary>The base for lists.</sumary>*/
    public class BaseButtonsList : MonoBehaviour
    {

        #region ATTRIBUTES
        
        /**<sumary>Marks if the buttons will blink.</sumary>*/
        public bool buttonsBlinks;
        /**<summary>If blink is marks, then it'll the velocity of the blinks.</summary>*/
        public float velocity = 0.5f;
        /**<summary>It marks The color when it's marked.</summary>*/
        public Color buttonColor = Color.green;
        /**<summary>It indicates the number of columns</summary>*/
        public int numsOfCol = 1;

        /**<summary>The buttons of the list.</summary>*/
        public GenericButton[] baseButtons;
        
        #region Size

        /**<summary>It depends of the nums of columns value.
        <para>It indicates the number of rows</para></summary>*/
        protected int NumsOfRows = 1;
        /**<summary>It indicates the number of row at the moment.</summary>*/
        protected int CurrentNumOfRows;

        #endregion

        #region Position

        /**<sumary>The current position of the list.</sumary>*/
        public int position;
        /**<summary>The current column index.</summary>*/
        protected int CurrentColumn;
        /**<summary>The current row index.</summary>*/
        protected int CurrentRow;

        #endregion

        #region Colliders

        /**<summary>Show in a text, what key must press for one action.</summary>*/
        public ControlsToShow controls;
        /**<summary>The main camera in use.</summary>*/
        protected Camera Camera;

        /**<summary>Buttons that can click.</summary>*/
        protected ClickButton[] ButtonsCanClick;
        /**<summary>Get the ID of the button that have been clicked of the buttons that can click.</summary>*/
        protected int Select = -1;
        /**<summary>Get the ID of the button that have been selected, after pressed.</summary>*/
        protected int Press = -1;

        #endregion
        
        #endregion

        protected void OnEnable()
        {
            baseButtons = transform.GetComponentsInChildren<GenericButton>()
                .Where(b => b.GetComponentsInParent<GenericButton>()
                    .Where(c => b.transform!=c.transform).ToArray()
                    .Length==0).ToArray();
            //baseButtons = transform.GetComponentsInChildren<GenericButton>()
                //.GroupBy(x => x.itemName.text)
                //.Select(y => y.First())
            //    .ToArray();
        }
        
        #region MOVEMENT

        /**<sumary>Set the columns and rows.</sumary>*/
        protected void SetColumnsAndRows(GenericButton[] buttons)
        {
            NumsOfRows = buttons.Count(b => b
                    .gameObject.activeInHierarchy && b.enabled) / numsOfCol + 
                          (buttons.Count(b => b
                                                  .gameObject.activeInHierarchy 
                          ) % numsOfCol == 0 ? 0 : 1);
            CurrentRow = position/numsOfCol;
            CurrentColumn = position % Mathf.Min(numsOfCol, buttons.Count(b => b.gameObject.activeInHierarchy));
            if ((NumsOfRows-1) * numsOfCol + CurrentColumn >= 
                buttons.Count(b => b.gameObject.activeInHierarchy)) 
                CurrentNumOfRows = NumsOfRows - 1;
            else CurrentNumOfRows = NumsOfRows;
            position = CurrentRow * numsOfCol + CurrentColumn;
        }
        
        /**<sumary>The button directional movement.</sumary>*/
        protected void Move(GenericButton[] buttons)
        {
            if (buttons.Length == 0) return;
    
            //Check if some button is Down
            if (!ControlsKeys.DirectionalKeyIsDown()) return;
            
            buttons[position].IsSelect = false;
            
            //Check the current Column of the button list
            CurrentColumn = CurrentColumn 
                             + Convert.ToInt32(Input.GetKeyDown(ControlsKeys.MoveRight))
                             - Convert.ToInt32(Input.GetKeyDown(ControlsKeys.MoveLeft));

            CurrentColumn %= Mathf.Min(numsOfCol, buttons.Count(b => b.gameObject.activeInHierarchy));
            if (CurrentColumn < 0) CurrentColumn = Mathf.Min(numsOfCol-1, buttons.Count(b => b.gameObject.activeInHierarchy)-1);

            //Update the row size with the number of the buttons
            if ((NumsOfRows-1) * numsOfCol + CurrentColumn >= 
                buttons.Count(b => b.gameObject.activeInHierarchy)) 
                CurrentNumOfRows = NumsOfRows - 1;
            else CurrentNumOfRows = NumsOfRows;
            
            //Check the current Column of the button list
            if (CurrentRow >= CurrentNumOfRows) CurrentRow = CurrentNumOfRows-1;
            
            //Check the current row of the button list
            CurrentRow = CurrentRow 
                          + Convert.ToInt32(Input.GetKeyDown(ControlsKeys.MoveDown))
                          - Convert.ToInt32(Input.GetKeyDown(ControlsKeys.MoveUp));

            if (CurrentNumOfRows != 0) CurrentRow %= CurrentNumOfRows;
            else CurrentRow = buttons.Count(b => b.gameObject.activeInHierarchy) - 1;
            if (CurrentRow < 0) CurrentRow = CurrentNumOfRows-1;
            
            //With the row and column pos I get the position. I mean where is the button now. 
            position = CurrentRow * numsOfCol + CurrentColumn;
            
            buttons[position].IsSelect = true;

        }
        
        #endregion

        #region SELECT OPTIONS

        /**<sumary>Quit all Selected buttons.</sumary>*/
        public void SelectNone(GenericButton[] buttons)
        {
            foreach (GenericButton button in buttons)
            {
                button.IsSelect = false;
            }
        }

        /**<sumary>Select the current position button.</sumary>*/
        public void SelectCurrent(GenericButton[] buttons)
        {
            SetColumnsAndRows(buttons);
            buttons[position].IsSelect = true;
        }

        #endregion
        
    }
}
