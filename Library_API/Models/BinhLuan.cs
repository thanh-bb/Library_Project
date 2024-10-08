using System;
using System.Collections.Generic;

namespace Library_API.Models;

public partial class BinhLuan
{
    public int BlId { get; set; }

    public int? NdId { get; set; }

    public int? SId { get; set; }

    public string? BlNoiDung { get; set; }

    public DateTime? BlNgayDang { get; set; }

    public virtual NguoiDung? Nd { get; set; }

    public virtual Sach? SIdNavigation { get; set; }
}
