using System;
using System.Collections.Generic;

namespace Library_API.Models;

public partial class DanhMuc
{
    public int DmId { get; set; }

    public string? DmTenDanhMuc { get; set; }

    public virtual ICollection<TheLoai>? TheLoais { get; set; } = new List<TheLoai>();
}
