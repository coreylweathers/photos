using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Photos.Shared.Models.Entities;
using Photos.Shared.Models.Options;
using Photos.Shared.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Photos.Shared.Services
{
    public class TableStorageService : IStorageService
    {
        private readonly TwilioOptions _twilioOptions;
        private readonly TableOptions _tableOptions;
        private readonly ILogger<TableStorageService> _logger;
        private CloudTableClient _tableClient;

        public TableStorageService(IOptionsMonitor<TableOptions> tableOptionsMonitor, IOptionsMonitor<TwilioOptions> twilioOptionsMonitor, ILogger<TableStorageService> logger)
        {
            _tableOptions = tableOptionsMonitor.CurrentValue;
            _twilioOptions = twilioOptionsMonitor.CurrentValue;
            _logger = logger;
            InitializeTableStorage();
        }

        private void InitializeTableStorage()
        {
            _logger.LogInformation("Initializing our table storage connection settings");
            try
            {
                var storageAccount = CloudStorageAccount.Parse(_tableOptions.ConnectionString);
                _tableClient = storageAccount.CreateCloudTableClient();

                _logger.LogInformation("Completed initializing our table storage connection settings");

            }
            catch (FormatException)
            {
                _logger.LogError("The connectionstring is not properly formatted");
            }
            catch (ArgumentException)
            {
                _logger.LogError("There is something wrong with the connecting string setting", _tableOptions.ConnectionString);
            }
            catch (Exception ex)
            {
                _logger.LogError("An exception has occurred", ex);
                throw;
            }
        }

        public async Task<string> GetData(string phoneNumberSid)
        {
            _logger.LogInformation("Looking into table storage for a phone number", phoneNumberSid);
            try
            {
                var table = _tableClient.GetTableReference(_tableOptions.TableName);
                var retrieveOperation = TableOperation.Retrieve<PhoneEntity>(_twilioOptions.AccountSid, phoneNumberSid);
                var result = await table.ExecuteAsync(retrieveOperation);
                var entity = result.Result as PhoneEntity;
                if (entity == null)
                {
                    throw new IOException($"Entity lookup for {phoneNumberSid} was not successful.");
                }
                else
                {
                    _logger.LogInformation("Completed looking into table storage for a phone number", phoneNumberSid);
                    return entity.PhoneNumber;
                }
            }
            catch (IOException ex)
            {
                _logger.LogError(ex.Message, ex);
            }

            return null;
        }

        public Task SaveData(object data)
        {
            throw new NotImplementedException();
        }
    }
}
