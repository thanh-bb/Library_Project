using System;
using System.Collections.Generic;

namespace ExcelToDatabase.Models;

public partial class TacGium
{
    public int TgId { get; set; }

    public string? TgTenTacGia { get; set; }

    public virtual ICollection<Sach> Saches { get; set; } = new List<Sach>();
}
