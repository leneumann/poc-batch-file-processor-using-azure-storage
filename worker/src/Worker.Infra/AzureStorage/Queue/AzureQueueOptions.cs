using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Worker.Infra.AzureStorage.Queue
{
    public class AzureQueueOptions
    {
        public const string AzureQueue = "AzureQueue";
        public string ConnectionString { get; set; }
        public Dictionary<string, QueueOptions> Queues { get; set; }
    }

    public class QueueOptions
    {
        public string Name { get; set; }
    }
}
