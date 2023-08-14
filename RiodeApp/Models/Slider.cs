using System.ComponentModel.DataAnnotations;

namespace RiodeApp.Models;

public class Slider : BaseEntity
{
    [Required]
    public string ImageUrl { get; set; }
    [MaxLength(50)]
    public string Title { get; set; }
    [Required,MaxLength(200)]
    public string Description { get; set; }
    [Required, MaxLength(50)]
    public string ButtonText { get; set; }
    [Required]
    public bool iSright { get; set; }
}
