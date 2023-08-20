using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RiodeApp.Models;

namespace RiodeApp.DataAccess;

public class RiodeDbContext : IdentityDbContext
{
    public RiodeDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Slider> Sliders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<ProductColor> ProductColors { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<UserToken> UserTokens { get; set; }
    public DbSet<Setting> Settings { get; set; }
}
