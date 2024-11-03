using System;
using System.Collections.Generic;

namespace Library_API.Models;

public partial class PhieuTraOnline
{
    public int PtoId { get; set; }

    public int? PmoId { get; set; }

    public int? NdId { get; set; }

    public DateTime? PtoNgayTra { get; set; }

    public string? PtoHinhThucTra { get; set; }

    public string? PtoTrangThai { get; set; }

    public virtual ICollection<ChiTietPhieuTraOnline> ChiTietPhieuTraOnlines { get; set; } = new List<ChiTietPhieuTraOnline>();

    public virtual NguoiDung? Nd { get; set; }

    public virtual PhieuMuonOnline? Pmo { get; set; }
}
