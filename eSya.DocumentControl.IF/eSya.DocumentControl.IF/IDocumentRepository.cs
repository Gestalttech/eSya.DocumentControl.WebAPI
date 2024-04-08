using eSya.DocumentControl.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.DocumentControl.IF
{
    public interface IDocumentRepository
    {
        #region Define Business - Document Link
        Task<List<DO_BusinessCalendarLink>> GetBusinesslinkedCalendarkeys();
        Task<List<DO_BusinessLocation>> GetBusinessLocationbyCalendarkeys(string calendarkey);
        Task<List<DO_BusinessDocument_Link>> GetDocumentFormlinkwithLocation(string calendarkey, int businesskey);
        Task<List<DO_DocumentControlMaster>> GetDocumentControlMaster(int formId);

        #endregion
    }
}
