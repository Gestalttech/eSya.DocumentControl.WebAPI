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
        #region Define Business - Document Link
        [HttpGet]
        public async Task<IActionResult> GetBusinesslinkedCalendarkeys()
        {
            var ds = await _documentRepository.GetBusinesslinkedCalendarkeys();
            return Ok(ds);
        }
        [HttpGet]
        public async Task<IActionResult> GetBusinessLocationbyCalendarkeys(string calendarkey)
        {
            var ds = await _documentRepository.GetBusinessLocationbyCalendarkeys(calendarkey);
            return Ok(ds);
        }
        [HttpGet]
        public async Task<IActionResult> GetDocumentFormlinkwithLocation(string calendarkey, int businesskey)
        {
            var ds = await _documentRepository.GetDocumentFormlinkwithLocation(calendarkey, businesskey);
            return Ok(ds);
        }
        [HttpGet]
        public async Task<IActionResult> GetDocumentControlMaster(int formId)
        {
            var ds = await _documentRepository.GetDocumentControlMaster(formId);
            return Ok(ds);
        }
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateBusinesswiseDocumentControl(List<DO_BusinessDocument_Link> obj)
        {
            var msg = await _documentRepository.InsertOrUpdateBusinesswiseDocumentControl(obj);
            return Ok(msg);

        }
        #endregion
    }
}
