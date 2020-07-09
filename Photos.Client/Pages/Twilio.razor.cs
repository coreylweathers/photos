using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Photos.Shared.Extensions;
using Photos.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Photos.Client.Pages
{
    public partial class Twilio
    {
        [Inject]
        public ILogger<Twilio> Logger { get; set; }
        [Inject]
        public HttpClient Http { get; set; }
        public string PhoneNumber { get; set; }
        public string SearchResult { get; set; }
        public string AreaCode { get; set; }
        public string PhoneNumberPurchaseSid { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            Logger.LogInformationWithDate("Starting the oninitialized method");
            Logger.LogInformationWithDate("Searching for existing phone numbers for this user");
            // Call the api to see if we have phone numbers
            var response = await Http.GetFromJsonAsync<TwilioApiPhoneNumber>($"api/twilio/phonenumbers");

            // If we have phone number, update the PhoneNumber property
            PhoneNumber = response?.Response;
            Logger.LogInformationWithDate("DONE!");
        }

        public async Task SearchPhoneNumber()
        {
            //TODO: Update this method to do some error handling

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
