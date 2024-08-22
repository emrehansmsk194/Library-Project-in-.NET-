using LibraryWeb.Models.DTO;

namespace LibraryWeb.Services.IServices
{
    public interface ILocationService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(LocationCreateDTO dto, string token);
        Task<T> UpdateAsync<T>(LocationUpdateDTO dTO, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }
}
