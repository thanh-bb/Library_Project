using System;
using System.Collections.Generic;

namespace Library_API.Models;

public partial class Osach
{
    public int OsId { get; set; }

    public string? OsTenO { get; set; }

    public virtual ICollection<Sach> Saches { get; set; } = new List<Sach>();
}
