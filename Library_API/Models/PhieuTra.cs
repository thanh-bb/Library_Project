using System;
using System.Collections.Generic;

namespace Library_API.Models;

public partial class PhieuTra
{
    public int PtId { get; set; }

    public DateTime? PtNgayTra { get; set; }

    public int? NdId { get; set; }

    public virtual ICollection<ChiTietPhieuTra> ChiTietPhieuTras { get; set; } = new List<ChiTietPhieuTra>();

    public virtual NguoiDung? Nd { get; set; }
}
