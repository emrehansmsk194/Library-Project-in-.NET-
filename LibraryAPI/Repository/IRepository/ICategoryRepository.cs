using LibraryAPI.Models;

namespace LibraryAPI.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> UpdateAsync(Category category);
    }
}
