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
        private int _numsOfRows = 1;
        /**<summary>It indicates the number of row at the moment it's generating</summary>*/
        private int _currentNumOfRows;

        #endregion

        #region Position

        /**<sumary>The current position.</sumary>*/
        public int position;
        /**<summary>The current column index.</summary>*/
        private int _currentColumn;
        /**<summary>The current row index.</summary>*/
        private int _currentRow;

        #endregion
        
        #endregion

        #region MOVEMENT

        /**<sumary>Set the columns and rows.</sumary>*/
        protected void SetColumnsAndRows(GenericButton[] buttons)
        {
            _numsOfRows = buttons.Where(b => b
                                  .gameObject.activeInHierarchy).ToArray()
                              .Length/numsOfCol 
                          + buttons.Where(b => b
                                  .gameObject.activeInHierarchy).ToArray()
                              .Length%numsOfCol;
            _currentNumOfRows = _numsOfRows;
            _currentRow = position;
            
        }
        
        /**<sumary>The button directional movement.</sumary>*/
        protected void Move(GenericButton[] buttons)
        {
            if (buttons.Length == 0) return;
    
            //Check if some button is Down
            if (!ControlsKeys.DirectionalKeyIsDown()) return;
            
            buttons[position].IsSelect = false;
            
            //Check the current Column of the button list
            _currentColumn = _currentColumn 
                             + Convert.ToInt32(Input.GetKeyDown(ControlsKeys.MoveRight))
                             - Convert.ToInt32(Input.GetKeyDown(ControlsKeys.MoveLeft));

            _currentColumn %= numsOfCol;
            if (_currentColumn < 0) _currentColumn = numsOfCol-1;

            //Update the column size with the number of the values that it have
            if (_currentColumn == numsOfCol-1) _currentNumOfRows =
                _numsOfRows - buttons.Where(b => b
                    .gameObject.activeInHierarchy).ToArray().Length % numsOfCol;
            else _currentNumOfRows = _numsOfRows;

            //Check the current Column of the button list
            if (_currentRow >= _currentNumOfRows) _currentRow = _currentNumOfRows-1;
            
            //Check the current row of the button list
            _currentRow = _currentRow + Convert.ToInt32(Input.GetKeyDown(ControlsKeys.MoveDown))
                          - Convert.ToInt32(Input.GetKeyDown(ControlsKeys.MoveUp));

            _currentRow %= _currentNumOfRows;
            if (_currentRow < 0) _currentRow = _currentNumOfRows-1;
            
            //With the row and column pos I get the position. I mean where is the button now. 
            position = _currentRow + _numsOfRows * _currentColumn;
            
            
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
            buttons[position].IsSelect = true;
        }

        #endregion
        
    }
}
