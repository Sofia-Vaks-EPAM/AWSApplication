using System;
using System.Collections.Generic;
using System.Text;

namespace AWSApplication.MessageQueues.Contracts
{
    public class BookQueueRequest
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
