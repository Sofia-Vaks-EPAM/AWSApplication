using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using AWSApplication.Data.Contracts;
using AWSApplication.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace AWSApplication.Data
{
    public class DataAccessDynamoDBProvider : IDataAccessProvider
    {
        private readonly ILogger _logger;
        public DataAccessDynamoDBProvider(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("DataAccessDynamoDBProvider");
        }
        public async Task AddBookRecord(Book book)
        {
            //Set a local DB context  
            var context = new DynamoDBContext(DynamoDBInitializer.client);
            await context.SaveAsync<Book>(book);
        }
        public async Task UpdateBookRecord(Book book)
        {
            //Set a local DB context  
            var context = new DynamoDBContext(DynamoDBInitializer.client);
            //Getting an book object  
            List<ScanCondition> conditions = new List<ScanCondition>();
            conditions.Add(new ScanCondition("ISBN", ScanOperator.Equal, book.ISBN));
            var allDocs = await context.ScanAsync<Book>(conditions).GetRemainingAsync();
            var editedState = allDocs.FirstOrDefault();
            if (editedState != null)
            {
                editedState = book;
                //Save an book object  
                await context.SaveAsync<Book>(editedState);
            }
        }
        public async Task DeleteBookRecord(string bookId)
        {
            const string tableName = "Book";
            var request = new DeleteItemRequest
            {
                TableName = tableName,
                Key = new Dictionary<string, AttributeValue>() {
                        {
                            "ISBN",
                            new AttributeValue {
                                S = bookId
                            }
                        }
                    }
            };
            var response = await DynamoDBInitializer.client.DeleteItemAsync(request);
        }
        public async Task<Book> GetBookSingleRecord(string id)
        {
            var context = new DynamoDBContext(DynamoDBInitializer.client);
            //Getting an book object  
            List<ScanCondition> conditions = new List<ScanCondition>();
            conditions.Add(new ScanCondition("ISBN", ScanOperator.Equal, id));
            var allDocs = await context.ScanAsync<Book>(conditions).GetRemainingAsync();
            var book = allDocs.FirstOrDefault();
            return book;
        }

        public async Task<IEnumerable<Book>> GetBookRecords()
        {
            var context = new DynamoDBContext(DynamoDBInitializer.client);
            //Getting an book object  
            List<ScanCondition> conditions = new List<ScanCondition>();
            var allDocs = await context.ScanAsync<Book>(conditions).GetRemainingAsync();
            return allDocs;
        }
    }
}