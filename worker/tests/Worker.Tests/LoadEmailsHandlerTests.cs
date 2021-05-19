using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Worker.App.Email;
using Worker.Utils.Result;
using Xunit;

namespace Worker.Tests
{
    [Collection("Tests collection")]
    public class LoadEmailsHandlerTests
    {
        private readonly ILoadEmailsHandler _handler;

        public LoadEmailsHandlerTests(CustomApplicationFactory<Startup> factory)
        {
            _handler = factory.Services.GetService<ILoadEmailsHandler>();
        }

        [Fact]
        public async Task Given_List_Of_Emails_When_Emails_Are_Valid_Then_Publish_Command()
        {
            //Arrange
            IResult result;
            var memoryStream = new MemoryStream();

            //Act
            using (var stream = new StreamWriter(memoryStream))
            {
                stream.WriteLine("luke.skywalker@gmail.com");
                stream.WriteLine("chewbacca@gmail.com");
                stream.WriteLine("han.solo@gmail.com");
                stream.Flush();
                memoryStream.Seek(0, SeekOrigin.Begin);
                var blobName = "mailListing";
                result = await _handler.HandleAsync(blobName, memoryStream).ConfigureAwait(false);
            }

            //Assert
            Assert.True(result.HasSucceeded);
        }

        [Fact]
        public async Task Given_List_Of_Emails_When_Emails_Are_Invalid_Then_Publish_Command()
        {
            //Arrange
            IResult result;
            var memoryStream = new MemoryStream();

            //Act
            using (var stream = new StreamWriter(memoryStream))
            {
                stream.WriteLine("luke.skywalker@gfake");
                stream.WriteLine("chewbacca@.com");
                stream.WriteLine("han.solo@gmail.");
                stream.Flush();
                memoryStream.Seek(0, SeekOrigin.Begin);
                var blobName = "mailListing";
                result = await _handler.HandleAsync(blobName, memoryStream).ConfigureAwait(false);
            }

            //Assert
            Assert.True(result.HasSucceeded);
        }
    }
}
