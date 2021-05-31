using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Worker.Utils;
using Worker.Utils.Result;

namespace Worker.App.Email
{
    public class LoadEmailslHandler : ILoadEmailsHandler
    {
        private readonly IPublisher _publisher;

        public LoadEmailslHandler(IPublisher publisher)
        {
            _publisher = publisher;
        }
        public async Task<IResult> HandleAsync(string blobName, Stream stream)
        {
            try
            {
                using (var streamReader = new StreamReader(stream))
                {
                    while (!streamReader.EndOfStream)
                    {
                        var emails = await CreateChunkOfEmails(streamReader).ConfigureAwait(false);
                        await SendCommand(emails).ConfigureAwait(false);
                    }
                }
                return new SuccessResult();
            }
            catch (Exception e)
            {
                return new ExceptionResult<Exception>(e.Message, e.GetType());
            }
        }

        private static async Task<List<string>> CreateChunkOfEmails(StreamReader streamReader)
        {
            var emails = new List<string>();
            while (emails.Count < 100 && !streamReader.EndOfStream)
            {
                var email = await streamReader.ReadLineAsync().ConfigureAwait(false);
                if (email.IsValidEmail())
                    emails.Add(email);
            }

            return emails;
        }

        private async Task SendCommand(List<string> emails)
        {
            if (emails.Any())
                await _publisher.PublishAsync(new LoadEmailsCommand(emails)).ConfigureAwait(false);
        }
    }
}
