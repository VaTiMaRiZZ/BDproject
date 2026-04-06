using System;
using System.Collections.Generic;

namespace App3.ModelsPostgre;

public partial class Partisipant
{
    public int? IdGrant { get; set; }

    public int? IdScient { get; set; }

    public virtual Grant? IdGrantNavigation { get; set; }

    public virtual Scientist? IdScientNavigation { get; set; }
}
