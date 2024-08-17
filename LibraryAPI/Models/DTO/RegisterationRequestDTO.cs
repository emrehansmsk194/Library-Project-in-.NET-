using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models.DTO
{
    public class RegisterationRequestDTO
    {
        [EmailAddress]
        public string UserName { get; set; }
        public string Name { get; set; }
        [MinLength(4)]
        public string Password { get; set; }
        public string Role { get; set; }
        public string AdminPassword { get; set; }
    }
}
