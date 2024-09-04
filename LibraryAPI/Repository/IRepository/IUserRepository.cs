using LibraryAPI.Models.DTO;

namespace LibraryAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        bool isUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<UserDTO> Register(RegisterationRequestDTO registerationRequestDTO);
        Task<List<string>> GetAllUsernames();
        Task<List<UserDTO>> GetAllUsersAsync();
    }
}
