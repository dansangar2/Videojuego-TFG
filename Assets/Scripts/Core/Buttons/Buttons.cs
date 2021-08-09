using System;
using Core.Controls;
using UnityEngine;

namespace Core.Buttons
{
    public class Buttons : MonoBehaviour
    {

        #region ATTRIBUTES
        
        /**<sumary>Marks if the buttons will blink.</sumary>*/
        public bool buttonsBlinks;
        /**<summary>If blink is marks, then it'll the velocity of the blinks.</summary>*/
        public float velocity = 0.005f;
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
        protected int Position;
        /**<summary>The current column index.</summary>*/
        private int _currentColumn;
        /**<summary>The current row index.</summary>*/
        private int _currentRow;

        #endregion
        
        #endregion

        protected void SetColumnsAndRows(Button[] buttons)
        {
            _numsOfRows = buttons.Length/numsOfCol + buttons.Length%numsOfCol;
            _currentNumOfRows = _numsOfRows;
            _currentRow = Position;
            
        }

        // Update is called once per frame
        protected void Move(Button[] buttons)
        {
            
            if (buttons.Length == 0) return;

            //if (_buttons[_position].IsStopped) return;
            
            //Check if some button is Down
            if (!ControlsKeys.DirectionalKeyIsDown()) return;
            
            buttons[Position].IsSelect = false;
            
            //Check the current Column of the button list
            _currentColumn = _currentColumn 
                             + Convert.ToInt32(Input.GetKeyDown(ControlsKeys.MoveRight))
                             - Convert.ToInt32(Input.GetKeyDown(ControlsKeys.MoveLeft));

            _currentColumn %= numsOfCol;
            if (_currentColumn < 0) _currentColumn = numsOfCol-1;

            //Update the column size with the number of the values that it have
            if (_currentColumn == numsOfCol-1) _currentNumOfRows = _numsOfRows - buttons.Length % numsOfCol;
            else _currentNumOfRows = _numsOfRows;

            //Check the current Column of the button list
            if (_currentRow >= _currentNumOfRows) _currentRow = _currentNumOfRows-1;
            
            //Check the current row of the button list
            _currentRow = _currentRow + Convert.ToInt32(Input.GetKeyDown(ControlsKeys.MoveDown))
                       - Convert.ToInt32(Input.GetKeyDown(ControlsKeys.MoveUp));

            _currentRow %= _currentNumOfRows;
            if (_currentRow < 0) _currentRow = _currentNumOfRows-1;
            
            //With the row and column pos I get the position. I mean where is the button now. 
            Position = _currentRow + _numsOfRows * _currentColumn;
            
            
            buttons[Position].IsSelect = true;

        }
    }
}
