using System;
using System.Collections.Generic;

namespace AppFive.ModelsSQL;

public partial class Grant
{
    public int IdGrants { get; set; }

    public int IdScient { get; set; }

    public string IdDirection { get; set; } = null!;

    public string? NameTheme { get; set; }

    public DateTime? DateStart { get; set; }

    public DateTime? DateEnd { get; set; }

    public string? Organization { get; set; }

    public int? Summa { get; set; }

    public virtual Direction IdDirectionNavigation { get; set; } = null!;

    public virtual Scientist IdScientNavigation { get; set; } = null!;
}
