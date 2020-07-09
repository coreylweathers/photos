using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Photos.Shared.Services;
using System.Linq;
using System.Threading.Tasks;
using Photos.Shared.Extensions;

namespace Photos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwilioController : ControllerBase
    {
        private readonly ITwilioService _twilioService;
        private readonly IStorageService _storageService;
        private readonly ILogger<TwilioController> _logger;

        public TwilioController(ITwilioService twilioService, IStorageService storageService, ILogger<TwilioController> logger)
        {
            _twilioService = twilioService;
            _storageService = storageService;
            _logger = logger;
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
            // TODO: Fix the bug that stops users from searching for a number when they already have one!
            _logger.LogInformationWithDate($"Searching for the phone number with this area code: {areaCode}");
            var response = "{{\"response\":\"{0}\"}}";
            // Search in storage first.
            var phoneNumberSid = (await _twilioService.GetPhoneNumberSids()).FirstOrDefault();
            var data = await _storageService.GetData(phoneNumberSid);
            if (data != null)
            {
                _logger.LogInformationWithDate($"Found phone number result: {data}");
                return new ContentResult { Content = string.Format(response, data), ContentType = "application/json", StatusCode = 200 };
            }

            // if no result, then search the twilioservice
            var result = await _twilioService.SearchPhoneNumber(areaCode);
            // TODO: Add the phone number to storage after we get it from Twilio
            _logger.LogInformationWithDate($"Found phone number after searching Twilio: {result}");
            return new ContentResult { Content = string.Format(response, result), ContentType = "application/json", StatusCode = 200 };
        }

        [HttpPost("phonenumber")]
        public async Task<IActionResult> PurchasePhoneNumber([FromForm]string phoneNumber)
        {
            // TODO: Add some error handling to this application for all external systems
            _logger.LogInformationWithDate($"Checking the Twilio Service to purchase a phone number {phoneNumber}");
            var result = await _twilioService.PurchasePhoneNumber(phoneNumber);
            _logger.LogInformationWithDate($"Successfully purchased the phone number from Twilio");
            return new JsonResult(new { sid = result });
        }
    }
}
