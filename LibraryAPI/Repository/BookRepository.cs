
using LibraryAPI.Models;
using LibraryAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repository
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        private readonly LibraryexdbContext _db;
        internal DbSet<Book> dbSet;
        public BookRepository(LibraryexdbContext db) : base(db)
        {
            _db = db;
            this.dbSet = db.Set<Book>();
        }
        public async Task<Book> UpdateAsync(Book book)
        {
           _db.Books.Update(book);
           await _db.SaveChangesAsync();
           return book;
        }
        public async Task BorrowBook(int bookId, string userId)
        {
            var book = await _db.Books.FindAsync(bookId);
            if (book != null && !book.IsBorrowed)
            {
                book.IsBorrowed = true;
                book.BorrowedDate = DateTime.Now;
                book.ReturnDate = DateTime.Now.AddDays(7);
                book.UserId = userId;
                await _db.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Book>> GetBooksByUser(string userId)
        {
            return await _db.Books.Where(b => b.UserId == userId && b.IsBorrowed).ToListAsync();
        }
    }
}
