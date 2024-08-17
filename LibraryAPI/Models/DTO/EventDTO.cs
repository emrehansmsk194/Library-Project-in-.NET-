namespace LibraryAPI.Models.DTO
{
    public class EventDTO
    {
        public int EventId { get; set; }

        public string EventName { get; set; } = null!;

        public DateTime Date { get; set; }

        public string? Description { get; set; }
    }
}
