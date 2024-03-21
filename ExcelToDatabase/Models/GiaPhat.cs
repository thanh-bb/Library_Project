using System;
using System.Collections.Generic;

namespace ExcelToDatabase.Models;

public partial class GiaPhat
{
    public int GpId { get; set; }

    public DateTime? GpNgayPhat { get; set; }

    public double? GpTienPhat { get; set; }

    public virtual ICollection<LoaiSach> LoaiSaches { get; set; } = new List<LoaiSach>();
}
