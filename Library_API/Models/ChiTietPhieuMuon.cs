using System;
using System.Collections.Generic;

namespace Library_API.Models;

public partial class ChiTietPhieuMuon
{
    public int SId { get; set; }

    public int PmId { get; set; }

    public int? CtpmSoLuongSachMuon { get; set; }

    public virtual PhieuMuon? Pm { get; set; } = null!;

    public virtual Sach? SIdNavigation { get; set; } = null!;
}
