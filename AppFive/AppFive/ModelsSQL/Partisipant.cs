using System;
using System.Collections.Generic;

namespace AppFive.ModelsSQL;

public partial class Partisipant
{
    public int IdGrant { get; set; }

    public int IdScient { get; set; }

    public virtual Scientist IdScientNavigation { get; set; } = null!;
}
