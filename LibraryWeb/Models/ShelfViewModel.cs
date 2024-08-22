using LibraryWeb.Models.DTO;

namespace LibraryWeb.Models
{
    public class ShelfViewModel
    {
        public string ShelfCode { get; set; }
        public List<BookDTO> Books { get; set; }  = new List<BookDTO>();
    }
}
