using LibraryAPI.Models;
using LibraryAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repository
{
    public class LocationRepository : Repository<Location>, ILocationRepository
    {
        private readonly LibraryexdbContext _db;
        internal DbSet<Location> dbSet;
        public LocationRepository(LibraryexdbContext db):base(db)
        {
            _db = db;
            this.dbSet = db.Set<Location>();
        }
        public async Task<Location> UpdateAsync(Location location)
        {
            _db.Locations.Update(location); 
            await _db.SaveChangesAsync();
            return location;
        }
    }
}
