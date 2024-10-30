using System;
using System.Collections.Generic;

namespace Library_API.Models;

public partial class ChiTietPhieuTra
{
    public int SId { get; set; }

    public int PtId { get; set; }

    public int? CtptSoLuongSachTra { get; set; }

    public virtual PhieuTra? Pt { get; set; } = null!;

    public virtual Sach? SIdNavigation { get; set; } = null!;
}
