using LibraryWeb.Models.DTO;

namespace LibraryWeb.Services.IServices
{
    public interface IEventService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(EventCreateDTO dto, string token);
        Task<T> UpdateAsync<T>(EventUpdateDTO dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }
}
