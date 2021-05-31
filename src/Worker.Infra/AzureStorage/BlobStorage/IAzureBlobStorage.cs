using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Worker.Infra.AzureStorage.BlobStorage
{
    public interface IAzureBlobStorage
    {
        BlobClient GetBlobClient(string containerName, BlobItem blob);
        Task<string> LeaseBlobAsync(string containerName, BlobItem blob, CancellationToken token);
        Task<List<BlobItem>> ListAsync(string containerName, string prefix, CancellationToken token);
        Task MoveBlobAsync(string containerName, BlobItem blobSource, string leaseId, string destinationFolder, CancellationToken token);
        Task ReleaseBlobAsync(string containerName, BlobItem blob, string leaseId, CancellationToken token);
    }
}
