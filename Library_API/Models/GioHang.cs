using System;
using System.Collections.Generic;

namespace Library_API.Models;

public partial class GioHang
{
    public int GhId { get; set; }

    public int? NdId { get; set; }

    public virtual ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();

    public virtual NguoiDung? Nd { get; set; }
}
