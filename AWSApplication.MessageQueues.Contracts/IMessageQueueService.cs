using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AWSApplication.MessageQueues.Contracts
{
    public interface IMessageQueueService
    {
        Task Send(BookQueueRequest book);
    }
}
