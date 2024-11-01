using System;
using System.Collections.Generic;

namespace Library_API.Models;

public partial class LoaiNguoiDung
{
    public int LndId { get; set; }

    public string? LndTenLoaiNguoiDung { get; set; }

    public virtual ICollection<NguoiDung>? NguoiDungs { get; set; } = new List<NguoiDung>();
}
