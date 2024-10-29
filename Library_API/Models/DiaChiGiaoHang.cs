using System;
using System.Collections.Generic;

namespace Library_API.Models;

public partial class DiaChiGiaoHang
{
    public int DcghId { get; set; }

    public int? NdId { get; set; }

    public string? DcghTenNguoiNhan { get; set; }

    public string? DcghSoDienThoai { get; set; }

    public string? DcghDiaChi { get; set; }

    public virtual NguoiDung? Nd { get; set; }

    public virtual ICollection<PhieuMuonOnline> PhieuMuonOnlines { get; set; } = new List<PhieuMuonOnline>();
}
