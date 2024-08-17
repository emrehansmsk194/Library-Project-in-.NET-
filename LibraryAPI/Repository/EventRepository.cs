using LibraryAPI.Models;
using LibraryAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repository
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        private readonly LibraryexdbContext _db;
        internal DbSet<Event> dbSet;
        public EventRepository(LibraryexdbContext db) : base(db)
        {
            _db = db;
            this.dbSet = db.Set<Event>();
        }
        public async Task<Event> UpdateAsync(Event entity)
        {
            _db.Events.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
