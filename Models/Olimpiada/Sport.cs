using System;
using System.Collections.Generic;

namespace Olimpiada.Models.Olimpiada;

public partial class Sport
{
    public int Id { get; set; }

    public string? SportName { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
