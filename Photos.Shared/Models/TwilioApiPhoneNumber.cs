using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Photos.Shared.Models
{
    public class TwilioApiPhoneNumber
    {
        [JsonProperty("response")]
        public string Response { get; set; }
    }
}
