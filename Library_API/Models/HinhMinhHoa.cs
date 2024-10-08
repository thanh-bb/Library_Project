using System;
using System.Collections.Generic;

namespace Library_API.Models;

public partial class HinhMinhHoa
{
    public int HmhId { get; set; }

    public int? SId { get; set; }

    public string? HmhHinhAnhMaHoa { get; set; }

    public virtual Sach? SIdNavigation { get; set; }
}
