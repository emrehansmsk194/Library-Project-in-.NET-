using LibraryWeb.Models.DTO;

namespace LibraryWeb.Services.IServices
{
    public interface IBookService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(BookCreateDTO dto, string token);
        Task<T> UpdateAsync<T>(BookUpdateDTO dTO, string token);
        Task<T> DeleteAsync<T>(int id, string token);
        Task<T> GetByCategoryAsync<T>(int categoryId, string token);
        Task<T> GetByLocationAsync<T>(int locationId, string token);
        Task<T> BorrowBookForUserAsync<T>(int bookId, string userId, string token);
        Task<T> GetBorrowedBookAsync<T>( string token);
    }
}
