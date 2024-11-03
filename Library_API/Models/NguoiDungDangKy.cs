using System;
using System.Collections.Generic;

namespace Library_API.Models;

public partial class NguoiDungDangKy
{
    public int NddkId { get; set; }

    public string? NddkHoTen { get; set; }

    public string? NddkCccd { get; set; }

    public string? NddkCccdMatTruoc { get; set; }

    public string? NddkCccdMatSau { get; set; }

    public string? NddkHinhThe { get; set; }

    public DateTime? NddkNgaySinh { get; set; }

    public string? NddkGioiTinh { get; set; }

    public string? NddkEmail { get; set; }

    public string? NddkSoDienThoai { get; set; }

    public string? NddkDiaChi { get; set; }

    public DateTime? NddkNgayDangKy { get; set; }

    public string? NddkThoiGianSuDung { get; set; }

    public string? NddkHinhThucTraPhi { get; set; }

    public string? NddkTrangThaiThanhToan { get; set; }

    public string? NddkTrangThaiDuyet { get; set; }

    public double? NddkSoTien { get; set; }
}
