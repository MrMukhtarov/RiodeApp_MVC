using Microsoft.Build.Framework;

namespace RiodeApp.Models;

public class Color : BaseEntity
{
    [Required]
    public string ColorCode { get; set; }
    public ICollection<ProductColor>? ProductColors { get; set; }
}
