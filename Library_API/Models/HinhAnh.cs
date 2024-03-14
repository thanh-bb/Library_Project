using System;
using System.Collections.Generic;

namespace Library_API.Models;

public partial class HinhAnh
{
    public int HaId { get; set; }

    public string? HaUrl { get; set; }

    public int? SId { get; set; }

    public virtual Sach? SIdNavigation { get; set; }
}
