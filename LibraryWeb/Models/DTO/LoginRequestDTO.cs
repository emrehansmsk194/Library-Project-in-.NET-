using System.ComponentModel.DataAnnotations;

namespace LibraryWeb.Models.DTO
{
    public class LoginRequestDTO
    {
        [EmailAddress]
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
