using Amazon.SQS;
using Amazon.SQS.Model;
using AWSApplication.MessageQueues.Contracts;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace AWSApplication.MessageQueues
{
    public class SQSService : IMessageQueueService
    {
        private string queueName = "awsqueue";
        private readonly IAmazonSQS _sqsClient;

        public SQSService(IAmazonSQS sqsClient)
        {
            _sqsClient = sqsClient;
        }

        public async Task Send(BookQueueRequest bookRequest)
        {
            //Create the request to send
            var response = await _sqsClient.GetQueueUrlAsync(queueName);
            var sendRequest = new SendMessageRequest();
            sendRequest.QueueUrl = response.QueueUrl;
            sendRequest.MessageBody = JsonConvert.SerializeObject(bookRequest);
            await _sqsClient.SendMessageAsync(sendRequest);
        }
    }
}
