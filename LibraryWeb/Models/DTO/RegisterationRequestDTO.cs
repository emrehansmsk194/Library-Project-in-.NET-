using System.ComponentModel.DataAnnotations;

namespace LibraryWeb.Models.DTO
{
    public class RegisterationRequestDTO
    {
        [Required]
        public string UserName { get; set; }
  
        public string Name { get; set; }
        [MinLength(4)]
        public string Password { get; set; }
        public string Role { get; set; } = "user";
        public string? AdminPassword { get; set; }
    }
}
