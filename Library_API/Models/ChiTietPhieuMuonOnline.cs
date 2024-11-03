using System;
using System.Collections.Generic;

namespace Library_API.Models;

public partial class ChiTietPhieuMuonOnline
{
    public int PmoId { get; set; }

    public int SId { get; set; }

    public int? CtpmoSoLuongSachMuon { get; set; }

    public virtual PhieuMuonOnline Pmo { get; set; } = null!;

    public virtual Sach SIdNavigation { get; set; } = null!;
}
