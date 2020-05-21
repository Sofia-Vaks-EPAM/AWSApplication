using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using AWSApplication.Data.Contracts;
using AWSApplication.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
namespace AWSApplication.Data
{
    public class DataAccessDynamoDBProvider : IDataAccessProvider
    {
        private readonly DynamoDBContext _context;

        public DataAccessDynamoDBProvider(IAmazonDynamoDB amazonDynamoDb, IAmazonSQS sqsClient)
        {
            _context = new DynamoDBContext(amazonDynamoDb);

        }
        public async Task AddBookRecord(Book book)
        {
            await _context.SaveAsync<Book>(book);
        }

        public async Task UpdateBookRecord(Book book)
        {
            //Getting an book object  
            List<ScanCondition> conditions = new List<ScanCondition>();
            conditions.Add(new ScanCondition("ISBN", ScanOperator.Equal, book.ISBN));
            var allDocs = await _context.ScanAsync<Book>(conditions).GetRemainingAsync();
            var editedState = allDocs.FirstOrDefault();
            if (editedState != null)
            {
                editedState = book;
                //Save an book object  
                await _context.SaveAsync<Book>(editedState);
            }
        }

        public async Task DeleteBookRecord(string bookId)
        {
            await _context.DeleteAsync<Book>(bookId);
        }

        public async Task<Book> GetBookSingleRecord(string id)
        {
            List<ScanCondition> conditions = new List<ScanCondition>();
            conditions.Add(new ScanCondition("ISBN", ScanOperator.Equal, id));
            var allDocs = await _context.ScanAsync<Book>(conditions).GetRemainingAsync();
            var book = allDocs.FirstOrDefault();
            return book;
        }

        public async Task<IEnumerable<Book>> GetBookRecords()
        { 
            List<ScanCondition> conditions = new List<ScanCondition>();
            var allDocs = await _context.ScanAsync<Book>(conditions).GetRemainingAsync();
            return allDocs;
        }
    }
}