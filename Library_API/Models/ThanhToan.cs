using System;
using System.Collections.Generic;

namespace Library_API.Models;

public partial class ThanhToan
{
    public int TtId { get; set; }

    public int? PmoId { get; set; }

    public string? TtPhuongThuc { get; set; }

    public string? TtTrangThai { get; set; }

    public DateTime? TtNgayThanhToan { get; set; }

    public double? TtSoTien { get; set; }

    public virtual PhieuMuonOnline? Pmo { get; set; }
}
