using LibraryAPI.Models;

namespace LibraryAPI.Repository.IRepository
{
    public interface IBookRepository : IRepository <Book>
    {
        Task<Book> UpdateAsync(Book book);
        Task BorrowBook(int bookId, string userId);
    }
}
