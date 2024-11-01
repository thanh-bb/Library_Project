using System;
using System.Collections.Generic;

namespace Library_API.Models;

public partial class NguoiDung
{
    public int NdId { get; set; }

    public string? NdUsername { get; set; }

    public string? NdCccd { get; set; }

    public string? NdSoDienThoai { get; set; }

    public string? NdHinhThe { get; set; }

    public string? NdPassword { get; set; }

    public string? NdHoTen { get; set; }

    public DateTime? NdNgaySinh { get; set; }

    public string? NdGioiTinh { get; set; }

    public string? NdEmail { get; set; }

    public string? NdDiaChi { get; set; }

    public DateTime? NdNgayDangKy { get; set; }

    public string? NdThoiGianSuDung { get; set; }

    public bool? NdActive { get; set; }

    public string? QId { get; set; }

    public int? LndLoaiNguoiDung { get; set; }

    public virtual ICollection<BinhLuan>? BinhLuans { get; set; } = new List<BinhLuan>();

    public virtual ICollection<DiaChiGiaoHang>? DiaChiGiaoHangs { get; set; } = new List<DiaChiGiaoHang>();

    public virtual ICollection<GioHang>? GioHangs { get; set; } = new List<GioHang>();

    public virtual LoaiNguoiDung? LndLoaiNguoiDungNavigation { get; set; }

    public virtual ICollection<PhieuMuonOnline>? PhieuMuonOnlines { get; set; } = new List<PhieuMuonOnline>();

    public virtual ICollection<PhieuMuon>? PhieuMuons { get; set; } = new List<PhieuMuon>();

    public virtual ICollection<PhieuTraOnline>? PhieuTraOnlines { get; set; } = new List<PhieuTraOnline>();

    public virtual ICollection<PhieuTra>? PhieuTras { get; set; } = new List<PhieuTra>();

    public virtual Quyen? QIdNavigation { get; set; }
}
