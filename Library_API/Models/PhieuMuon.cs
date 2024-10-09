using System;
using System.Collections.Generic;

namespace Library_API.Models;

public partial class PhieuMuon
{
    public int PmId { get; set; }

    public DateTime? PmNgayMuon { get; set; }

    public DateTime? PmHanTra { get; set; }

    public string? PmTrangThaiMuon { get; set; }

    public string? PmTrangThaiXetDuyet { get; set; }

    public int? NdId { get; set; }

    public virtual ICollection<ChiTietPhieuMuon>? ChiTietPhieuMuons { get; set; } = new List<ChiTietPhieuMuon>();

    public virtual NguoiDung? Nd { get; set; }

    public virtual ICollection<PhieuDongPhat>? PhieuDongPhats { get; set; } = new List<PhieuDongPhat>();

    public virtual ICollection<PhieuTra>? PhieuTras { get; set; } = new List<PhieuTra>();
}
