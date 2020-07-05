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
        public string SearchResult { get; set; }
        public string AreaCode { get; set; }
        public string PhoneNumberPurchaseSid { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            // Call the api to see if we have phone numbers
            var response = await Http.GetFromJsonAsync<TwilioApiPhoneNumber>($"api/twilio/phonenumbers");

            // If we have phone number, update the PhoneNumber property
            PhoneNumber = response?.Response;
        }

        public async Task SearchPhoneNumber()
        {
            // connect to the api to purchase the number
            var response = await Http.GetFromJsonAsync<TwilioApiPhoneNumber>($"api/twilio/phonenumbers?areaCode={AreaCode}");

            // set a string variable with the purchased number
            SearchResult = response.Response;
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

        public async Task Purchase415Number()
        {
            Console.WriteLine("Here is your number: 415-555-1234");
            PhoneNumber = "No. You are denied!";
            await Task.CompletedTask;
        }
    }
}
