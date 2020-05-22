using System.Collections.Generic;
using System.Threading.Tasks;
using AWSApplication.Data.Contracts;
using AWSApplication.MessageQueues.Contracts;
using AWSApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace AWSApplication.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : Controller
    {
        private readonly IDataAccessProvider _dataAccessProvider;

        private readonly IMessageQueueService _queueService;

        public BooksController(IDataAccessProvider dataAccessProvider, IMessageQueueService queueService)
        {
            _dataAccessProvider = dataAccessProvider;
            _queueService = queueService;
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
                await _queueService.Send(new BookQueueRequest
                {
                    ISBN = book.ISBN,
                    Title = book.Title,
                    Description = book.Description,
                    OperationType = OperationType.Add
                });
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
                await _queueService.Send(new BookQueueRequest
                {
                    ISBN = book.ISBN,
                    Title = book.Title,
                    Description = book.Description,
                    OperationType = OperationType.Update
                });
            }
        }

        [HttpDelete]
        [Route("Delete/{bookId}")]
        public async Task DeleteConfirmed(string bookId)
        {
            await _dataAccessProvider.DeleteBookRecord(bookId);
            await _queueService.Send(new BookQueueRequest
            {
                ISBN = bookId,
                OperationType = OperationType.Delete
            });
        }
    }
}
