using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace Worker.Infra.AzureStorage.BlobStorage
{
    public class AzureBlobConnectionFactory : IAzureBlobConnectionFactory
    {
        private readonly ILogger<AzureBlobConnectionFactory> _logger;
        private readonly BlobStorageOptions _options;

        public AzureBlobConnectionFactory(ILogger<AzureBlobConnectionFactory> logger, IOptions<BlobStorageOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        public BlobContainerClient GetContainerClient(string containerName)
        {
            BlobContainerClient client = null;
            try
            {
                client = new BlobContainerClient(_options.StorageConnectionString, containerName);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            return client;
        }
    }
}
