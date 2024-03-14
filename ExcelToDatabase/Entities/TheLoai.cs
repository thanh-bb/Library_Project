using System;
using System.Collections.Generic;

namespace ExcelToDatabase.Entities;

public partial class TheLoai
{
    public int TlId { get; set; }

    public string? TlTenTheLoai { get; set; }

    public int? DmId { get; set; }

    public virtual DanhMuc? Dm { get; set; }

    public virtual ICollection<Sach> Saches { get; set; } = new List<Sach>();
}
