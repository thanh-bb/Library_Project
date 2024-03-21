using System;
using System.Collections.Generic;

namespace ExcelToDatabase.Models;

public partial class NguoiDung
{
    public int NdId { get; set; }

    public string? NdUsername { get; set; }

    public string? NdPassword { get; set; }

    public string? NdHoTen { get; set; }

    public DateTime? NdNgaySinh { get; set; }

    public string? NdGioiTinh { get; set; }

    public string? NdDiaChi { get; set; }

    public DateTime? NdNgayDangKy { get; set; }

    public bool? NdActive { get; set; }

    public string? QId { get; set; }

    public virtual ICollection<PhieuMuon> PhieuMuons { get; set; } = new List<PhieuMuon>();

    public virtual ICollection<PhieuTra> PhieuTras { get; set; } = new List<PhieuTra>();

    public virtual Quyen? QIdNavigation { get; set; }
}
