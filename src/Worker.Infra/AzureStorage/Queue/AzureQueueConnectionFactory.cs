using Azure.Storage.Queues;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Worker.Infra.AzureStorage.Queue
{
    public class AzureQueueConnectionFactory : IAzureQueueConnectionFactory
    {
        private readonly ILogger<AzureQueueConnectionFactory> _logger;
        private readonly IOptions<AzureQueueOptions> _options;

        public AzureQueueConnectionFactory(ILogger<AzureQueueConnectionFactory> logger, IOptions<AzureQueueOptions> options)
        {
            this._logger = logger;
            this._options = options;
        }

        public async Task<QueueClient> GetQueueClient(string queueName)
        {
            try
            {
                QueueClient client = new QueueClient(_options.Value.ConnectionString, queueName);
                await client.CreateIfNotExistsAsync().ConfigureAwait(false);
                return client;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

    }
}
