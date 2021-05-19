using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Worker.App.Email;
using Worker.Infra.AzureStorage.BlobStorage;

namespace Worker
{
    public class BlobWorker : BackgroundService
    {
        private readonly ILogger<BlobWorker> _logger;
        private readonly IOptions<BlobStorageOptions> _blobStorageOptions;
        private readonly IBlobProcessor _blobProcessor;
        private readonly ILoadEmailsHandler _loadEmailsHandler;

        public BlobWorker(ILogger<BlobWorker> logger,
            IOptions<BlobStorageOptions> blobStorageOptions,
            IBlobProcessor blobProcessor,
            ILoadEmailsHandler loadEmailsHandler)
        {
            _logger = logger;
            _blobStorageOptions = blobStorageOptions;
            _blobProcessor = blobProcessor;
            _loadEmailsHandler = loadEmailsHandler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await _blobProcessor.AddProcessor(_loadEmailsHandler, _blobStorageOptions,stoppingToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occurred: {e.Message}");
                throw;
            }
        }
    }
}
