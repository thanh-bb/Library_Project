using System;
using System.Collections.Generic;

namespace Library_API.Models;

public partial class Refreshtoken
{
    public string UserId { get; set; } = null!;

    public string? TokenId { get; set; }

    public string? RefreshToken1 { get; set; }

    public bool? IsActive { get; set; }
}
