using System.Collections.Generic;

namespace Worker.App.Email
{
    public class LoadEmailsCommand:ICommand
    {
        public List<string> Emails { get; set; }
        public LoadEmailsCommand()
        {

        }
        public LoadEmailsCommand(List<string> emails)
        {
            Emails = emails;
        }
    }
}
