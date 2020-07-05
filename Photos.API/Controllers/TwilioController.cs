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
        private readonly IStorageService _storageService;

        public TwilioController(ITwilioService twilioService, IStorageService storageService)
        {
            _twilioService = twilioService;
            _storageService = storageService;
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

        [HttpGet("phonenumbers")]
        public async Task<IActionResult> SearchPhoneNumber([FromQuery]string areaCode = "")
        {
            var response = "{{\"response\":\"{0}\"}}";
            // Search in storage first.
            var phoneNumberSid = (await _twilioService.GetPhoneNumberSids()).FirstOrDefault();
            var data = await _storageService.GetData(phoneNumberSid);
            if (data != null)
            {             
                return new ContentResult { Content = string.Format(response, data), ContentType = "application/json", StatusCode = 200 };
            }
            // if no result, then search the twilioservice
            var result = await _twilioService.SearchPhoneNumber(areaCode);
            return new ContentResult { Content = string.Format(response, result), ContentType = "application/json", StatusCode = 200 };
        }

        [HttpPost("phonenumber")]
        public async Task<IActionResult> PurchasePhoneNumber([FromForm]string phoneNumber)
        {
            var result = await _twilioService.PurchasePhoneNumber(phoneNumber);
            return new JsonResult(new { sid = result });
        }
    }
}
