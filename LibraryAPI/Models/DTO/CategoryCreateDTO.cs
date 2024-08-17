using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models.DTO
{
    public class CategoryCreateDTO
    {
   
        [Required]
        public string CategoryName { get; set; } = null!;
    }
}
