using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models.DTO
{
    public class CategoryUpdateDTO
    {
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; } = null!;
    }
}
