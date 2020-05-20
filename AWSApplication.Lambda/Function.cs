using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using ThirdParty.BouncyCastle.Asn1;


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
            var fileTransferUtility =
                new TransferUtility(s3Client);

            foreach (var message in evnt.Records)
            {
                await ProcessMessageAsync(message, context, fileTransferUtility);
            }
        }

        private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context, TransferUtility fileTransferUtility)
        {

            byte[] bytes = Encoding.ASCII.GetBytes(message.Body);
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(bytes);
                await fileTransferUtility.UploadAsync(ms, "vaksbusket", message.MessageId + ".txt");
            }

        }
    }
}
