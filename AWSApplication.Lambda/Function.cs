using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.S3;
using Amazon.S3.Model;
using AWSApplication.MessageQueues.Contracts;
using Newtonsoft.Json;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWSApplication.Lambda
{
    public class Function
    {
        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast2;
        private static IAmazonS3 s3Client;
        private static readonly string bucketName = "aws-app-bucket";

        public Function()
        {
            s3Client = new AmazonS3Client(bucketRegion);
        }


        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an SQS event object and can be used 
        /// to respond to SQS messages.
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
        {
            foreach (var message in evnt.Records)
            {
                await ProcessMessageAsync(message, context);
            }
        }

        private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context)
        {

            byte[] bytes = Encoding.ASCII.GetBytes(message.Body);
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(bytes);
                var bookRequest = JsonConvert.DeserializeObject<BookQueueRequest>(message.Body);
                switch(bookRequest.OperationType)
                {
                    case OperationType.Add:
                    case OperationType.Update:
                        var putObjectRequest = new PutObjectRequest
                        {
                            BucketName = bucketName,
                            Key = bookRequest.ISBN + ".txt",
                            InputStream = ms
                        };
                        await s3Client.PutObjectAsync(putObjectRequest);
                        break;
                    case OperationType.Delete:
                        var deleteObjectRequest = new DeleteObjectRequest
                        {
                            BucketName = bucketName,
                            Key = bookRequest.ISBN + ".txt"
                        };
                        await s3Client.DeleteObjectAsync(deleteObjectRequest);
                        break;
                }
            }

        }
    }
}
