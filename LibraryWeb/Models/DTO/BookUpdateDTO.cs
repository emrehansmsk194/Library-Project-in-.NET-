using System.ComponentModel.DataAnnotations;

namespace LibraryWeb.Models.DTO
{
    public class BookUpdateDTO
    {
        [Required]
        public int BookId { get; set; }
        [Required]

        public string Name { get; set; } = null!;

        public int PageCount { get; set; }

        public string Author { get; set; } = null!;

        public int LocationId { get; set; }

        public int CategoryId { get; set; }

        public string? CoverImageUrl { get; set; }
        public bool IsAdminOnly { get; set; }

    }
}
