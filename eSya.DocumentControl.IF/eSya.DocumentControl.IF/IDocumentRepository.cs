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
        

        #region Define Business - Document Link -New
        Task<List<DO_FormBusinessLink>> GetMenuFormslinkwithLocation(int businesskey);
        Task<List<DO_DocumentControlStandard>> GetDocumentControlStandard(int formId, int businesskey);
        Task<DO_ReturnParameter> InsertOrUpdateBusinesswiseDocumentControlLink(List<DO_BusinessDocument_Link> obj);

        #endregion
    }
}
