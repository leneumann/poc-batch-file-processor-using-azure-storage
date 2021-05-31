using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Worker.Infra.AzureStorage.BlobStorage
{
    public class AzureBlobStorage : IAzureBlobStorage
    {
        private readonly IAzureBlobConnectionFactory _azureBlobConnection;
        private readonly ILogger<AzureBlobStorage> _logger;

        public AzureBlobStorage(IAzureBlobConnectionFactory azureBlobConnection, ILogger<AzureBlobStorage> logger)
        {
            _azureBlobConnection = azureBlobConnection;
            _logger = logger;
        }

        public async Task<List<BlobItem>> ListAsync(string containerName, string prefix, CancellationToken token)
        {
            try
            {
                var containerClient = _azureBlobConnection.GetContainerClient(containerName);
                var result = containerClient.GetBlobsAsync(BlobTraits.None, BlobStates.None, prefix, token);
                var blobs = new List<BlobItem>();
                await foreach (var item in result)
                {
                    if (item.Properties.BlobType == BlobType.Block &&
                        item.Properties.LeaseState != LeaseState.Leased)
                        blobs.Add(item);
                }
                return blobs;
            }
            catch (RequestFailedException e)
            {
                _logger.LogError(e.Message);
                return default;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }

        }

        public async Task<string> LeaseBlobAsync(string containerName, BlobItem blob, CancellationToken token)
        {
            try
            {
                var containerClient = _azureBlobConnection.GetContainerClient(containerName);
                var client = containerClient.GetBlobClient(blob.Name);
                var lease = await client.GetBlobLeaseClient(Guid.NewGuid().ToString())
                                    .AcquireAsync(BlobLeaseClient.InfiniteLeaseDuration, cancellationToken: token).ConfigureAwait(false);
                return lease.Value.LeaseId;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        public async Task ReleaseBlobAsync(string containerName, BlobItem blob, string leaseId, CancellationToken token)
        {
            try
            {
                var containerClient = _azureBlobConnection.GetContainerClient(containerName);
                var client = containerClient.GetBlobClient(blob.Name);
                await client.GetBlobLeaseClient(leaseId).ReleaseAsync(cancellationToken: token).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        public async Task MoveBlobAsync(string containerName, BlobItem blobSource, string leaseId, string destinationFolder, CancellationToken token)
        {
            try
            {
                var containerClient = _azureBlobConnection.GetContainerClient(containerName);
                var blobSourceClient = containerClient.GetBlobClient(blobSource.Name);
                var blobSourceName = string.Join("/", blobSource.Name.Split("/").Skip(1).ToArray());
                var destinationBlob = containerClient.GetBlobClient($"{destinationFolder}{blobSourceName}");
                var operation = await destinationBlob.StartCopyFromUriAsync(blobSourceClient.Uri, cancellationToken: token).ConfigureAwait(false);
                await operation.WaitForCompletionAsync(token).ConfigureAwait(false);
                await blobSourceClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, new BlobRequestConditions() { LeaseId = leaseId }, token).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        public BlobClient GetBlobClient(string containerName, BlobItem blob)
        {
            try
            {
                var containerClient = _azureBlobConnection.GetContainerClient(containerName);
                return containerClient.GetBlobClient(blob.Name);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }
    }
}
