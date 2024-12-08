using System;
using System.Collections.Generic;

namespace Library_API.Models;

public partial class TrangThaiMuon
{
    public int TtmId { get; set; }

    public string? TtmTenTrangThai { get; set; }

    public string? TtmMoTa { get; set; }

    public virtual ICollection<PhieuMuon>? PhieuMuons { get; set; } = new List<PhieuMuon>();
}
