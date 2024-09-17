using Microsoft.EntityFrameworkCore;
using EshopApi.Domain.Entities;

namespace EshopApi.Infrastructure.Data
{
    public class EshopDbContext : DbContext
    {
        public EshopDbContext(DbContextOptions<EshopDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.Entity<User>()
        //          .HasKey(u => u.Id);

        //     modelBuilder.Entity<User>()
        //         .Property(u => u.Id)
        //         .ValueGeneratedOnAdd();

        //     modelBuilder.Entity<User>()
        //         .Property(u => u.Username)
        //         .IsRequired()
        //         .HasMaxLength(50);

        //     modelBuilder.Entity<User>()
        //         .Property(u => u.Role)
        //         .IsRequired();

        //     modelBuilder.Entity<User>()
        //         .Property(u => u.Password)
        //         .IsRequired()
        //         .HasMaxLength(256);

        //     modelBuilder.Entity<User>()
        //         .HasIndex(u => u.Username)
        //         .IsUnique();

        //     base.OnModelCreating(modelBuilder);
        // }
    }
}
