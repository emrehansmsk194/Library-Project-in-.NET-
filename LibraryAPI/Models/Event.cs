using System;
using System.Collections.Generic;

namespace LibraryAPI.Models;

public partial class Event
{
    public int EventId { get; set; }

    public string EventName { get; set; } = null!;

    public DateTime Date { get; set; }

    public string? Description { get; set; }
}
