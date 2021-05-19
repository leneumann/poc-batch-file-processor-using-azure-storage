using System.Collections.Generic;

namespace Worker.App.Email
{
    public class LoadEmailsCommand:ICommand
    {
        public IReadOnlyList<string> Emails { get; }

        public LoadEmailsCommand(IReadOnlyList<string> emails)
        {
            Emails = emails;
        }
    }
}
