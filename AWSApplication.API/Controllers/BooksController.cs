using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SQS.Model;
using AWSApplication.Data.Contracts;
using AWSApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AWSApplication.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : Controller
    {
        private readonly IDataAccessProvider _dataAccessProvider;
        public BooksController(IDataAccessProvider dataAccessProvider)
        {
            _dataAccessProvider = dataAccessProvider;
        }

        [HttpGet]
        public async Task<IEnumerable<Book>> Get()
        {
            return await _dataAccessProvider.GetBookRecords();
        }
        [HttpPost]
        public async Task Create([FromBody] Book book)
        {
            if (ModelState.IsValid)
            {
                await _dataAccessProvider.AddBookRecord(book);
            }
        }
        [HttpGet]
        [Route("Details/{id}")]
        public async Task<Book> Details(string id)
        {
            return await _dataAccessProvider.GetBookSingleRecord(id);
        }
        [HttpPut]
        [Route("Edit")]
        public async Task Edit([FromBody] Book book)
        {
            if (ModelState.IsValid)
            {
                await _dataAccessProvider.UpdateBookRecord(book);
            }
        }
        [HttpDelete]
        [Route("Delete/{bookId}")]
        public async Task DeleteConfirmed(string bookId)
        {
            await _dataAccessProvider.DeleteBookRecord(bookId);
        }
    }
}
