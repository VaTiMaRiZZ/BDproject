using System;
using System.Collections.Generic;

namespace App3.ModelsPostgre;

public partial class Grant
{
    public int IdGrants { get; set; }

    public int? IdScient { get; set; }

    public string? IdDirection { get; set; }

    public string? NameTheme { get; set; }

    public DateOnly? DateStart { get; set; }

    public DateOnly? DateEnd { get; set; }

    public string? Organization { get; set; }

    public int? Summa { get; set; }

    public virtual Direction? IdDirectionNavigation { get; set; }

    public virtual Scientist? IdScientNavigation { get; set; }
}
