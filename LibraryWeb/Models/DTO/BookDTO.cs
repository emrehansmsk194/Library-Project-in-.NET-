using System.ComponentModel.DataAnnotations;

namespace LibraryWeb.Models.DTO
{
    public class BookDTO
    {
        public int BookId { get; set; }
        [Required]

        public string Name { get; set; } = null!;

        public int PageCount { get; set; }

        public string Author { get; set; } = null!;

        public int LocationId { get; set; }

        public int CategoryId { get; set; }

        public string? CoverImageUrl { get; set; }
        public DateTime? BorrowedDate { get; set; } 
        public DateTime? ReturnDate { get; set; }
       
        public bool IsAdminOnly { get; set; } = false;

        public int LocationFloor {  get; set; } 
        public string LocationShelf { get; set; }
        public string CategoryName { get; set; }
    }
}
