using System;
using System.Collections.Generic;

namespace Library_API.Models;

public partial class Sach
{
    public int SId { get; set; }

    public string? STenSach { get; set; }

    public int? SSoLuong { get; set; }

    public string? SMoTa { get; set; }

    public string? STrongLuong { get; set; }

    public DateTime? SNamXuatBan { get; set; }

    public bool? STrangThaiMuon { get; set; }

    public bool? SChiDoc { get; set; }

    public int? TgId { get; set; }

    public int? NxbId { get; set; }

    public int? TlId { get; set; }

    public int? LsId { get; set; }

    public int? KsId { get; set; }

    public int? OsId { get; set; }

    public virtual ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();

    public virtual ICollection<ChiTietPhieuMuon> ChiTietPhieuMuons { get; set; } = new List<ChiTietPhieuMuon>();

    public virtual ICollection<ChiTietPhieuTra> ChiTietPhieuTras { get; set; } = new List<ChiTietPhieuTra>();

    public virtual ICollection<HinhMinhHoa> HinhMinhHoas { get; set; } = new List<HinhMinhHoa>();

    public virtual KeSach? Ks { get; set; }

    public virtual LoaiSach? Ls { get; set; }

    public virtual NhaXuatBan? Nxb { get; set; }

    public virtual Osach? Os { get; set; }

    public virtual TacGium? Tg { get; set; }

    public virtual TheLoai? Tl { get; set; }
}
