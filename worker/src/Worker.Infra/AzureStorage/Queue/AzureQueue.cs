using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Worker.App;

namespace Worker.Infra.AzureStorage.Queue
{
    public class AzureQueue : IPublisher
    {
        private readonly ILogger<AzureQueue> _logger;
        private readonly IAzureQueueConnectionFactory _queueConnectionFactory;
        private readonly IOptions<AzureQueueOptions> _options;

        public AzureQueue(ILogger<AzureQueue> logger,
            IAzureQueueConnectionFactory queueConnectionFactory,
            IOptions<AzureQueueOptions> options)
        {
            _logger = logger;
            _queueConnectionFactory = queueConnectionFactory;
            _options = options;
        }
        public async Task PublishAsync(ICommand command)
        {
            try
            {
                string commandName = command.GetType().Name.Replace("Command", string.Empty, StringComparison.InvariantCultureIgnoreCase);
                _options.Value.Queues.TryGetValue(commandName, out var queueOptions);

                var queue = await _queueConnectionFactory.GetQueueClient(queueOptions.Name).ConfigureAwait(false);
                var json = JsonSerializer.Serialize(command, command.GetType());
                await queue.SendMessageAsync(json).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }

        }
    }
}
