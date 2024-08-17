using System;
using System.Collections.Generic;

namespace LibraryAPI.Models;

public partial class Book
{
    public int BookId { get; set; }

    public string Name { get; set; } = null!;

    public int PageCount { get; set; }

    public string Author { get; set; } = null!;

    public int LocationId { get; set; }

    public int CategoryId { get; set; }

    public string? CoverImageUrl { get; set; }

    public bool IsBorrowed { get; set; } = false;

    public DateTime? BorrowedDate { get; set; }

    public DateTime? ReturnDate { get; set; }
    public string? UserId { get; set; }
    public bool IsAdminOnly { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Location Location { get; set; } = null!;
    public virtual ApplicationUser User { get; set; }
}
