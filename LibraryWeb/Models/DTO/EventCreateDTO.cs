using System.ComponentModel.DataAnnotations;

namespace LibraryWeb.Models.DTO
{
    public class EventCreateDTO
    {

        [Required]
        public string EventName { get; set; } = null!;
        [Required]

        public DateTime Date { get; set; }

        public string? Description { get; set; }
    }
}
