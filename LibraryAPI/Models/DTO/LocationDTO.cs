namespace LibraryAPI.Models.DTO
{
    public class LocationDTO
    {
        public int LocationId { get; set; }
        public int Floor { get; set; }

        public string Shelf { get; set; } = null!;
    }
}
