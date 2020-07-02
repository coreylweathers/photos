using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Photos.Shared.Services;

namespace Photos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwilioController : ControllerBase
    {
        private readonly ITwilioService _twilioService;

        public TwilioController(ITwilioService twilioService)
        {
            _twilioService = twilioService;
        }

        [HttpGet("authorize"), HttpPost("authorize")]
        public IActionResult Authorize([FromQuery,FromForm]string accountSid)
        {
            // TODO: SAVE THE ACCOUNT SID FOR THE LOGGED IN USER. IF USER NOT LOGGED IN,
            return NoContent();
        }

        [HttpGet("deauthorize")]
        public IActionResult Deauthorize()
        {
            // TODO: FILL THIS IN
            return NoContent();
        }

        [HttpGet("phonenumber")]
        public async Task<IActionResult> SearchPhoneNumber([FromQuery]string areaCode = "")
        {
            var result = await _twilioService.SearchPhoneNumber(areaCode);
            string response = $"{{\"response\":\"{result}\"}}";
            return new ContentResult { Content = response, ContentType = "application/json", StatusCode = 200 };
        }

        [HttpPost("phonenumber")]
        public async Task<IActionResult> PurchasePhoneNumber([FromForm]string phoneNumber)
        {
            var result = await _twilioService.PurchasePhoneNumber(phoneNumber);
            return new JsonResult(new { sid = result });
        }
    }
}
