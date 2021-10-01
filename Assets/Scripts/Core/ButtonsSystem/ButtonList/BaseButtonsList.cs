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
        
        #region Size

        /**<summary>It depends of the nums of columns value.
        <para>It indicates the number of rows</para></summary>*/
        protected int NumsOfRows = 1;
        /**<summary>It indicates the number of row at the moment it's generating</summary>*/
        protected int CurrentNumOfRows;

        #endregion

        #region Position

        /**<sumary>The current position.</sumary>*/
        public int position;
        /**<summary>The current column index.</summary>*/
        protected int CurrentColumn;
        /**<summary>The current row index.</summary>*/
        protected int CurrentRow;

        #endregion
        
        #endregion

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

        /**<sumary>Quit all Select.</sumary>*/
        protected void SelectNone(GenericButton[] buttons)
        {
            foreach (GenericButton button in buttons)
            {
                button.IsSelect = false;
            }
        }

        /**<sumary>Select the current position button.</sumary>*/
        protected void SelectCurrent(GenericButton[] buttons)
        {
            SetColumnsAndRows(buttons);
            buttons[position].IsSelect = true;
        }

        #endregion
        
    }
}
