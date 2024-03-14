using System;
using System.Collections.Generic;

namespace Library_API.Models;

public partial class LoaiSach
{
    public int LsId { get; set; }

    public string? LsTenLoaiSach { get; set; }

    public string? LsKichThuoc { get; set; }

    public string? LsGhiChu { get; set; }

    public int? GpId { get; set; }

    public virtual GiaPhat? Gp { get; set; }

    public virtual ICollection<Sach> Saches { get; set; } = new List<Sach>();
}
