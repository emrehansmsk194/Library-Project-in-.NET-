using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models.DTO
{
    public class LoginRequestDTO
    {
        [EmailAddress]
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
