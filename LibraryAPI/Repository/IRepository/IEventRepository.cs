using LibraryAPI.Models;

namespace LibraryAPI.Repository.IRepository
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<Event> UpdateAsync(Event entity);
    }
}
