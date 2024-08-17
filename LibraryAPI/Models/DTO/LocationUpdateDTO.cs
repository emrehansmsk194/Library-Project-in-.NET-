using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models.DTO
{
    public class LocationUpdateDTO
    {
        [Required]
        public int LocationId { get; set; }
        [Required]
        public int Floor { get; set; }

        public string Shelf { get; set; } = null!;
    }
}
