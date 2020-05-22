using Amazon.DynamoDBv2.DataModel;
using System;

namespace AWSApplication.Models
{
    [DynamoDBTable("Book")]
    public class Book
    {
        [DynamoDBHashKey]
        public string ISBN
        {
            get;
            set;
        }
        public string Title
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
    }
}
