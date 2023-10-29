using Microsoft.AspNetCore.Mvc;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;

namespace RombookingApp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomBookingController : Controller
    {
        private IRoomBookingRequestProcessor _requestProcessor;

        public RoomBookingController(IRoomBookingRequestProcessor requestProcessor)
        {
            _requestProcessor = requestProcessor;
        }

        [HttpPost]
        public async Task<IActionResult> BookRoom(RoomBookingRequest request)
        {
            if(ModelState.IsValid)
            {
                var result=_requestProcessor.BookRoom(request);
                if(result.Flag == RoomBookingApp.Core.Enums.BookingResultFlag.Success)
                {
                    return Ok(result);
                }
                ModelState.AddModelError(nameof(RoomBookingRequest.Date), "No rooms available for given Date");
                
                

            }
            return BadRequest(ModelState);
        }
    }
}
