using System;
using System.Collections.Generic;

namespace Library_API.Models;

public partial class Quyen
{
    public string QId { get; set; } = null!;

    public string? QTenQuyen { get; set; }

    public virtual ICollection<NguoiDung>? NguoiDungs { get; set; } = new List<NguoiDung>();
}
