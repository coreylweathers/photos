using Microsoft.AspNetCore.Components;
using Newtonsoft.Json.Linq;
using Photos.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Photos.Client.Pages
{
    public partial class Twilio
    {
        [Inject]
        public HttpClient Http { get; set; }
        public string PhoneNumber { get; set; }
        public string AreaCode { get; set; }
        public string PhoneNumberPurchaseSid { get; private set; }

        public async Task SearchPhoneNumber()
        {
            // connect to the api to purchase the number
            
            var response = await Http.GetFromJsonAsync<TwilioApiPhoneNumber>("api/twilio/phonenumber?areaCode={AreaCode}");

            // set a string variable with the purchased number
            PhoneNumber = response.Response;
        }

        public async Task PurchasePhoneNumber()
        {
            var postData = new Dictionary<string, string> 
            {
               { "phoneNumber", PhoneNumber }
            };
            var content = new FormUrlEncodedContent(postData);
            var response = await Http.PostAsync("api/twilio/phoneNumber", content);
            response.EnsureSuccessStatusCode();
            var json = JObject.Parse(await response.Content.ReadAsStringAsync());
            PhoneNumberPurchaseSid = json["sid"].ToString();
        }
    }
}
