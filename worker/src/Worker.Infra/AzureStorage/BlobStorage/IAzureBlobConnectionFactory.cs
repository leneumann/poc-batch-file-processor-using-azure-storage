using Azure.Storage.Blobs;

namespace Worker.Infra.AzureStorage.BlobStorage
{
    public interface IAzureBlobConnectionFactory
    {
        BlobContainerClient GetContainerClient(string containerName);
    }
}
