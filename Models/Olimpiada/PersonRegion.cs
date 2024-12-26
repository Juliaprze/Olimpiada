using System;
using System.Collections.Generic;

namespace Olimpiada.Models.Olimpiada;

public partial class PersonRegion
{
    public int? PersonId { get; set; }

    public int? RegionId { get; set; }

    public virtual Person? Person { get; set; }

    public virtual NocRegion? Region { get; set; }
}
