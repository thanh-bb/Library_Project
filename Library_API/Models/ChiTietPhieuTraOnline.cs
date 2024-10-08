using System;
using System.Collections.Generic;

namespace Library_API.Models;

public partial class ChiTietPhieuTraOnline
{
    public int CtptoId { get; set; }

    public int? PtoId { get; set; }

    public int? SId { get; set; }

    public int? CtptoSoLuongSachTra { get; set; }

    public virtual PhieuTraOnline? Pto { get; set; }

    public virtual Sach? SIdNavigation { get; set; }
}
