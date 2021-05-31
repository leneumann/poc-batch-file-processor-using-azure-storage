using System.IO;
using System.Threading.Tasks;
using Worker.Utils.Result;

namespace Worker.App
{
    public interface IStreamHandler
    {
        Task<IResult> HandleAsync(string blobName, Stream stream);
    }
}
