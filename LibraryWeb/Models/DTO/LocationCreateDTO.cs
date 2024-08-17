using System.ComponentModel.DataAnnotations;

namespace LibraryWeb.Models.DTO
{
    public class LocationCreateDTO
    {
        [Required]
        public int Floor { get; set; }
        [Required]
        public string Shelf { get; set; } = null!;
    }
}
