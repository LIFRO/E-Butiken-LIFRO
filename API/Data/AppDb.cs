using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> Options) : base(Options){}

    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<ProductModel> Products => Set<ProductModel>();
    public DbSet<OrderModel> Orders => Set<OrderModel>();
    public DbSet<OrderItemModel> OrderItems => Set<OrderItemModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ProductModel>().HasData(
            new ProductModel { Id = 1, Name = "SKU-001", Description = "Eco cotton t-shirt", Price = 19.99m, Stock = 150 },
            new ProductModel { Id = 2, Name = "SKU-002", Description = "Insulated stainless bottle 750ml", Price = 24.50m, Stock = 90 },
            new ProductModel { Id = 3, Name = "SKU-003", Description = "Everyday canvas backpack 20L", Price = 49.00m, Stock = 60 }
        );
    }
}
