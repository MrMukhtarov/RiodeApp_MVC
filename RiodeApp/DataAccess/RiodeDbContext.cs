using Microsoft.EntityFrameworkCore;
using RiodeApp.Models;

namespace RiodeApp.DataAccess;

public class RiodeDbContext : DbContext
{
    public RiodeDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Slider> Sliders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
}
