using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Photos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwilioController : ControllerBase
    {
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
    }
}
