using System.ComponentModel.DataAnnotations;

namespace LibraryWeb.Models.DTO
{
    public class CategoryUpdateDTO
    {
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; } = null!;
    }
}
