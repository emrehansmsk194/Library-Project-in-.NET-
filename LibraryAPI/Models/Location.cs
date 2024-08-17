using System;
using System.Collections.Generic;

namespace LibraryAPI.Models;

public partial class Location
{
    public int LocationId { get; set; }

    public int Floor { get; set; }

    public string Shelf { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
