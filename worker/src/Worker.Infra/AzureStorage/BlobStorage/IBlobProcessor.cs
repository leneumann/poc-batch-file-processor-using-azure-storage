using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using Worker.App;

namespace Worker.Infra.AzureStorage.BlobStorage
{
    public interface IBlobProcessor
    {
        Task AddProcessor(IStreamHandler handler, IOptions<BlobStorageOptions> options, CancellationToken stoppingToken);
    }
}
