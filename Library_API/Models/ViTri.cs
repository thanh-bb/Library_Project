using System;
using System.Collections.Generic;

namespace Library_API.Models;

public partial class ViTri
{
    public int VtId { get; set; }

    public int? KsId { get; set; }

    public int? OsId { get; set; }

    public virtual KeSach? Ks { get; set; }

    public virtual Osach? Os { get; set; }

    public virtual ICollection<Sach>? Saches { get; set; } = new List<Sach>();
}
