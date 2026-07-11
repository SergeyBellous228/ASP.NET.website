using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebHike.Data.Entities;

namespace WebHike.Data;

public class HikeDbContext : DbContext
{
    public HikeDbContext(DbContextOptions<HikeDbContext> options)
        : base(options)
    {
    }

    public DbSet<CategoryEntity> Categories { get; set; }

    public DbSet<ProductEntity> Products { get; set; }

    public DbSet<ProductImageEntity> ProductImages { get; set; }
}