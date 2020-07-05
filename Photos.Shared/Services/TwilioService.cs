using Microsoft.Extensions.Options;
using Photos.Shared.Extensions;
using Photos.Shared.Models;
using Photos.Shared.Models.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Base;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Api.V2010.Account.AvailablePhoneNumberCountry;
using Twilio.Rest.Messaging.V1.Service;
using Twilio.Types;

namespace Photos.Shared.Services
{
    public class TwilioService : ITwilioService
    {
        private readonly TwilioOptions _twilioOptions;

        public TwilioService(IOptionsMonitor<TwilioOptions> optionsMonitor)
        {
            _twilioOptions = optionsMonitor.CurrentValue;
            //Connect to Twilio
            TwilioClient.Init(_twilioOptions.AccountSid, _twilioOptions.AuthToken);
        }

        public async Task<string> PurchasePhoneNumber (string phoneNumber)
        {
            // Format the phone number
            // TODO: Allow bad chars to be specificed by caller into the service
            char[] badChars = { '(', ')', ' ', '-' };
            phoneNumber = RemoveBadChars(phoneNumber, badChars);

            // Purchase a phone number
            var result = await IncomingPhoneNumberResource.CreateAsync(
                voiceApplicationSid: _twilioOptions.AppSid,
                phoneNumber: new PhoneNumber(phoneNumber));

            return result.Sid;
        }

        private static string RemoveBadChars(string phoneNumber, params char[] characters)
        {
            // Phone Number formatted as '(ddd) ddd-dddd' 
            /*foreach (var character in characters)
            {
                if (phoneNumber.Contains(character))
                {
                    var index = phoneNumber.IndexOf(character);
                    phoneNumber = phoneNumber.Remove(index, 1);
                }
            }*/

            foreach (var character in characters)
            {
                //phoneNumber = phoneNumber.Replace(character, '\0');
                phoneNumber = phoneNumber.Replace(character.ToString(), "");
            }
            return phoneNumber;
        }

        public async Task<string> SearchPhoneNumber(string areaCode)
        {
            if (string.IsNullOrEmpty(areaCode))
            {
                await Task.FromException(new ArgumentNullException(nameof(areaCode)));
            }

            // Lookup phone number options
            var results = await GetLocalPhoneNumber(areaCode);

            // Select a phone number & return it
            var number = results?.Select(result => result.FriendlyName).FirstOrDefault();
            return number.ToString();
        }

        public async Task<IEnumerable<string>> GetPhoneNumberSids()
        {
            var numbers = await IncomingPhoneNumberResource.ReadAsync();

            return numbers.Select(x => x.Sid).AsEnumerable();
        }

        public virtual async Task<ResourceSet<LocalResource>> GetLocalPhoneNumber(string areaCode= null)
        {
            var code = areaCode.ToNullableInt();
            ResourceSet<LocalResource> result;
            try
            {
                result = await LocalResource.ReadAsync(pathCountryCode: "US", areaCode: code);                
            }
            catch (ApiException)
            {
                result = null;
            }
            return result;
        }
    }
}
