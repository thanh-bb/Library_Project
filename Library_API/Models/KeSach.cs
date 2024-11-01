using System;
using System.Collections.Generic;

namespace Library_API.Models;

public partial class KeSach
{
    public int KsId { get; set; }

    public string? KsTenKe { get; set; }

    public virtual ICollection<Sach>? Saches { get; set; } = new List<Sach>();
}
