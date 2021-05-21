using Azure.Storage.Queues;
using System.Threading.Tasks;

namespace Worker.Infra.AzureStorage.Queue
{
    public interface IAzureQueueConnectionFactory
    {
        Task<QueueClient> GetQueueClient(string queueName);
    }
}