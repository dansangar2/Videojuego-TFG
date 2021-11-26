using System;
using Core.ButtonsSystem.ButtonList;

namespace Core.ButtonsSystem
{
    /**<sumary>It set the total pages of the list.</sumary>*/
    public class ButtonsPages : BaseButtonsList
    {

        #region ATTRIBUTES

        /**<sumary>Set the max item for pages of the list.</sumary>*/
        public int maxInPage = 4;

        protected int NumberOfPages;
        protected int CurrentPage;

        #endregion
        
        /**<sumary>Generate the pages for "items" items.</sumary>*/
        protected void SetPages(int items)
        {
            NumberOfPages = items / maxInPage;
            NumberOfPages = items % maxInPage == 0 ? NumberOfPages : NumberOfPages + 1;
            CurrentPage = 0;
        }

        /**<sumary>Move of page.
        <param name="back">Check if move back page.</param>
        <param name="next">Check if move next page.</param>
        </sumary>*/
        protected void ChangePage(bool back, bool next)
        {
            CurrentPage = CurrentPage + Convert.ToInt32(next)
                          - Convert.ToInt32(back);
            if (CurrentPage < 0) CurrentPage = NumberOfPages-1;
            CurrentPage %= NumberOfPages;
        }
        
        /**<sumary>Move of page when you go next in the end or begin of the page.</sumary>*/
        protected void ChangePage()
        {
            if(maxInPage*CurrentPage > position) CurrentPage++;
            else if (maxInPage * CurrentPage < position) CurrentPage--;
            
            if (CurrentPage < 0) CurrentPage = NumberOfPages-1;
            CurrentPage %= NumberOfPages;
        }
        
    }
}