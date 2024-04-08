using eSya.DocumentControl.DO;
using eSya.DocumentControl.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.DocumentControl.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BusinessCalendarController : ControllerBase
    {
        private readonly IBusinessCalendarRepository _businessCalendarRepository;
        public BusinessCalendarController(IBusinessCalendarRepository businessCalendarRepository)
        {
            _businessCalendarRepository = businessCalendarRepository;
        }
        #region Business Calendar
        [HttpGet]
        public async Task<IActionResult> GetCalendarHeaders(int businesskey)
        {
            var ds = await _businessCalendarRepository.GetCalendarHeaders(businesskey);
            return Ok(ds);
        }
        [HttpPost]
        public async Task<IActionResult> InsertBusinessKeyIntoCalendar(DO_BusinessCalendarLink obj)
        {
            var msg = await _businessCalendarRepository.InsertBusinessKeyIntoCalendar(obj);
            return Ok(msg);

        }
        #endregion

        #region Document Calendar Business Link
        //[HttpGet]
        //public async Task<IActionResult> GetBusinesslinkedCalendarkeys(int businessKey)
        //{
        //    var ds = await _businessCalendarRepository.GetBusinesslinkedCalendarkeys(businessKey);
        //    return Ok(ds);
        //}
        //[HttpGet]
        //public async Task<IActionResult> GetActiveDocuments()
        //{
        //    var ds = await _businessCalendarRepository.GetActiveDocuments();
        //    return Ok(ds);
        //}
        //[HttpGet]
        //public async Task<IActionResult> GetBusinessCalendarBYBusinessKey(int businessKey)
        //{
        //    var ds = await _businessCalendarRepository.GetBusinessCalendarBYBusinessKey(businessKey);
        //    return Ok(ds);
        //}
        //[HttpPost]
        //public async Task<IActionResult> InsertOrUpdateBusinessCalendar(DO_BusinessCalendar obj)
        //{
        //    var msg = await _businessCalendarRepository.InsertOrUpdateBusinessCalendar(obj);
        //    return Ok(msg);

        //}
        #endregion
    }
}
