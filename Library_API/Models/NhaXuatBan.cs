﻿using System;
using System.Collections.Generic;

namespace Library_API.Models;

public partial class NhaXuatBan
{
    public int NxbId { get; set; }

    public string? NxbTenNhaXuatBan { get; set; }

    public virtual ICollection<Sach> Saches { get; set; } = new List<Sach>();
}
