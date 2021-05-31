namespace Worker.Infra.AzureStorage.BlobStorage
{ 
    public class BlobStorageOptions
    {
        public const string BlobStorage = "BlobStorage";
        public string ContainerName { get; set; }
        public string Prefix { get; set; }
        public string ProcessedBlobs { get; set; }
        public string RejectedBlobs { get; set; }
        public string StorageConnectionString { get; set; }
    }
}
