
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Amazon.SQS;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace AWSApplication.Data
{
    public static class SQSInitializer
    {
        public const string queueUrl = "https://sqs.us-east-2.amazonaws.com/718240643545/awsqueue";
        private
        const string accessKey = "AKIA2OOT2PHM3XCBPZEN";
        private
        const string secretKey = "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";
        public static AmazonSQSClient sqsClient;
        public static async Task InitializeSQS()
        {
            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            sqsClient = new AmazonSQSClient(credentials, RegionEndpoint.USEast2);
        }
    }
}