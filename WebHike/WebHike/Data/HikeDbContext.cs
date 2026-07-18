using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebHike.Data.Entities;
using WebHike.Data.Entities.Identity;


namespace WebHike.Data;

public class HikeDbContext : IdentityDbContext<UserEntity, RoleEntity, int>
{
    public HikeDbContext(DbContextOptions<HikeDbContext> options)
        : base(options)
    {
    }

    public DbSet<CategoryEntity> Categories { get; set; }

    public DbSet<ProductEntity> Products { get; set; }

    public DbSet<ProductImageEntity> ProductImages { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //identity
        modelBuilder.Entity<UserRoleEntity>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);
        
        modelBuilder.Entity<UserRoleEntity>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);
    }
}