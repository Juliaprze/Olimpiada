using System;
using System.Collections.Generic;

namespace Olimpiada.Models.Olimpiada;

public partial class GamesCompetitor
{
    public int Id { get; set; }

    public int? GamesId { get; set; }

    public int? PersonId { get; set; }

    public int? Age { get; set; }

    public virtual Game? Games { get; set; }

    public virtual Person? Person { get; set; }
}
