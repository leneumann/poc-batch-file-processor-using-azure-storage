using System.Threading.Tasks;

namespace Worker.App
{
    public interface IPublisher
    {
        Task PublishAsync(ICommand command);
    }
}
