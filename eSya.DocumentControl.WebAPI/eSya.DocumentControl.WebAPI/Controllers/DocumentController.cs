using eSya.DocumentControl.DL.Repository;
using eSya.DocumentControl.DO;
using eSya.DocumentControl.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.DocumentControl.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentRepository _documentRepository;
        public DocumentController(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }
        

        #region Define Business - Document Link -New
        [HttpGet]
        public async Task<IActionResult> GetMenuFormslinkwithLocation(int businesskey)
        {
            var ds = await _documentRepository.GetMenuFormslinkwithLocation(businesskey);
            return Ok(ds);
        }
        [HttpGet]
        public async Task<IActionResult> GetDocumentControlStandard(int formId, int businesskey)
        {
            var ds = await _documentRepository.GetDocumentControlStandard(formId, businesskey);
            return Ok(ds);
        }
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateBusinesswiseDocumentControlLink(List<DO_BusinessDocument_Link> obj)
        {
            var msg = await _documentRepository.InsertOrUpdateBusinesswiseDocumentControlLink(obj);
            return Ok(msg);

        }

        
        #endregion
    }
}
