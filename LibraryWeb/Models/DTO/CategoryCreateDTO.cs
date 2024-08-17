using System.ComponentModel.DataAnnotations;

namespace LibraryWeb.Models.DTO
{
    public class CategoryCreateDTO
    {
   
        [Required]
        public string CategoryName { get; set; } = null!;
    }
}
