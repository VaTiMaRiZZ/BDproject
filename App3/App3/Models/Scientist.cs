using System;
using System.Collections.Generic;

namespace App3.Models;

public partial class Scientist
{
    public int IdScient { get; set; }

    public string? FioScient { get; set; }

    public DateOnly? Date { get; set; }

    public string? Degree { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<Grant> Grants { get; set; } = new List<Grant>();

    public virtual ICollection<Partisipant> Partisipants { get; set; } = new List<Partisipant>();
}
