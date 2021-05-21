using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using Worker.App;

namespace Worker.Infra.AzureStorage.BlobStorage
{
    public class BlobProcessor : IBlobProcessor
    {
        private readonly ILogger<BlobProcessor> _logger;
        private readonly IAzureBlobStorage _blobStorage;

        public BlobProcessor(ILogger<BlobProcessor> logger, IAzureBlobStorage blobStorage)
        {
            _logger = logger;
            _blobStorage = blobStorage;
        }
        public async Task AddProcessor(IStreamHandler handler, IOptions<BlobStorageOptions> options, CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                var blobs = await _blobStorage.ListAsync(options.Value.ContainerName, options.Value.Prefix, stoppingToken).ConfigureAwait(false);
                if (blobs != default && blobs.Count > 0)
                    Parallel.ForEach(blobs,
                    new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                    item =>
                    {
                        _ = Run(handler, options, item, stoppingToken);
                    });

                Thread.Sleep(10000);
            }
        }

        private async Task Run(IStreamHandler handler, IOptions<BlobStorageOptions> options, BlobItem item, CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation(item.Name);
                var leaseId = await _blobStorage.LeaseBlobAsync(options.Value.ContainerName, item, stoppingToken).ConfigureAwait(false);
                var blob = _blobStorage.GetBlobClient(options.Value.ContainerName, item);
                using var blobStream = await blob.OpenReadAsync(cancellationToken: stoppingToken).ConfigureAwait(false);
                var result = await handler.HandleAsync(blob.Name, blobStream).ConfigureAwait(false);
                
                if (result.HasSucceeded)
                    await _blobStorage.MoveBlobAsync(options.Value.ContainerName, item, leaseId, options.Value.ProcessedBlobs, stoppingToken).ConfigureAwait(false);
                else await _blobStorage.MoveBlobAsync(options.Value.ContainerName, item, leaseId, options.Value.RejectedBlobs, stoppingToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occurred while processing blob {item.Name}: {e.Message}");
            }
        }
    }
}
