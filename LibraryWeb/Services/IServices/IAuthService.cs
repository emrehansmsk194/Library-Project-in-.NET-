using LibraryWeb.Models.DTO;

namespace LibraryWeb.Services.IServices
{
    public interface IAuthService
    {
        Task<T> LoginAsync<T>(LoginRequestDTO objToCreate);
        Task<T> RegisterAsync<T>(RegisterationRequestDTO objToCreate);
        Task<T> GetAllUsersAsync<T>(string token);
        Task<T> GetAllUserNamesAsync<T>(string token);
    }
}
