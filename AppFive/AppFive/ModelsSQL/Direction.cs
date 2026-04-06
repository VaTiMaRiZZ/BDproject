using System;
using System.Collections.Generic;

namespace AppFive.ModelsSQL;

public partial class Direction
{
    public string IdDirection { get; set; } = null!;

    public string? NameDirection { get; set; }

    public virtual ICollection<Grant> Grants { get; set; } = new List<Grant>();
}
