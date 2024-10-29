using System;
using System.Collections.Generic;

namespace Library_API.Models;

public partial class PhieuMuonOnline
{
    public int PmoId { get; set; }

    public int? NdId { get; set; }

    public DateTime? PmoNgayDat { get; set; }

    public string? PmoLoaiGiaoHang { get; set; }

    public string? PmoPhuongThucThanhToan { get; set; }

    public int? DcghId { get; set; }

    public string? PmoTrangThai { get; set; }

    public DateTime? PmoHanTra { get; set; }

    public virtual ICollection<ChiTietPhieuMuonOnline>? ChiTietPhieuMuonOnlines { get; set; } = new List<ChiTietPhieuMuonOnline>();

    public virtual DiaChiGiaoHang? Dcgh { get; set; }

    public virtual NguoiDung? Nd { get; set; }

    public virtual ICollection<PhieuTraOnline> PhieuTraOnlines { get; set; } = new List<PhieuTraOnline>();

    public virtual ICollection<ThanhToan> ThanhToans { get; set; } = new List<ThanhToan>();
}
