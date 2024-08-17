using LibraryAPI.Models;

namespace LibraryAPI.Repository.IRepository
{
    public interface ILocationRepository : IRepository<Location>
    {
        Task<Location> UpdateAsync(Location location);
    }
}
