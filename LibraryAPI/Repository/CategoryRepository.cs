using LibraryAPI.Models;
using LibraryAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly LibraryexdbContext _db;
        internal DbSet<Category> dbSet;
        public CategoryRepository(LibraryexdbContext db) : base(db)
        {
            _db = db;
            this.dbSet = db.Set<Category>();
        }
        public async Task<Category> UpdateAsync(Category category)
        {
            _db.Categories.Update(category);
            await _db.SaveChangesAsync();
            return category;
        }
    }
}
