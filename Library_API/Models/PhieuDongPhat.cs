using System;
using System.Collections.Generic;

namespace Library_API.Models;

public partial class PhieuDongPhat
{
    public int PdpId { get; set; }

    public double? PdpTongTienPhat { get; set; }

    public DateTime? PdpNgayDong { get; set; }

    public bool? PdpTrangThaiDong { get; set; }

    public int? PmId { get; set; }

    public virtual PhieuMuon? Pm { get; set; }
}
