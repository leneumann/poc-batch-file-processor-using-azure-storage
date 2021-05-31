using Microsoft.Extensions.DependencyInjection;
using Worker.App;
using Worker.App.Email;
using Worker.Infra.AzureStorage.BlobStorage;
using Worker.Infra.AzureStorage.Queue;

namespace Worker.Infra.IoC
{
    public static class IoCExtensions
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            return services.AddSingleton<IAzureBlobConnectionFactory, AzureBlobConnectionFactory>()
            .AddSingleton<IAzureBlobStorage, AzureBlobStorage>()
            .AddSingleton<IAzureQueueConnectionFactory, AzureQueueConnectionFactory>()
            .AddSingleton<IBlobProcessor, BlobProcessor>()
            .AddSingleton<ILoadEmailsHandler, LoadEmailslHandler>()
            .AddSingleton<IPublisher,AzureQueue>();
        }
    }
}
