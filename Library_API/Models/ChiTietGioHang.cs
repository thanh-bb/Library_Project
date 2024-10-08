using System;
using System.Collections.Generic;

namespace Library_API.Models;

public partial class ChiTietGioHang
{
    public int CtghId { get; set; }

    public int? CtghSoLuong { get; set; }

    public int? GhId { get; set; }

    public int? SId { get; set; }

    public virtual GioHang? Gh { get; set; }

    public virtual Sach? SIdNavigation { get; set; }
}
