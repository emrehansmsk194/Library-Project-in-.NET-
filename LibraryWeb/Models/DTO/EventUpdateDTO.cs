using System.ComponentModel.DataAnnotations;

namespace LibraryWeb.Models.DTO
{
    public class EventUpdateDTO
    {
        [Required]
        public int EventId { get; set; }
        [Required]
        public string EventName { get; set; } = null!;
        [Required]
        public DateTime Date { get; set; }

        public string? Description { get; set; }
    }
}
