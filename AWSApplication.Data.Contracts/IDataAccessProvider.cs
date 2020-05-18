
using AWSApplication.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AWSApplication.Data.Contracts
{
    public interface IDataAccessProvider
    {
        Task AddBookRecord(Book book);
        Task UpdateBookRecord(Book book);
        Task DeleteBookRecord(string bookId);
        Task<Book> GetBookSingleRecord(string bookId);
        Task<IEnumerable<Book>> GetBookRecords();
    }
}
