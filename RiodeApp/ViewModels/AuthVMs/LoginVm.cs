using System.ComponentModel.DataAnnotations;

public record LoginVM
{
    [Required]
    public string UsernameOrEmail { get; set; }
    [Required, DataType(DataType.Password), MinLength(8)]
    public string Password { get; set; }
    [Required]
    public bool RememberMe { get; set; }
}