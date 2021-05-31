using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Worker.App;

namespace Worker.Infra
{
   public class InMemoryCommandProducer : IPublisher
    {
        private readonly ILogger<InMemoryCommandProducer> _logger;

        public InMemoryCommandProducer(ILogger<InMemoryCommandProducer> logger)
        {
            _logger = logger;
        }
        public async Task PublishAsync(ICommand command)
        {
            var json = JsonSerializer.Serialize(command, command.GetType());
            var message = $"Command - {command.GetType().Name } published - {json}";

            _logger.LogInformation(message);

            await Task.CompletedTask;
        }
    }
}
