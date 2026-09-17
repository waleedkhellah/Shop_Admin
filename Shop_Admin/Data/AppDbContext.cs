using Microsoft.EntityFrameworkCore;
using Shop_Admin.Models;

namespace Shop_Admin.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<AdminUser> AdminUsers { get; set; }

    public DbSet<Teacher> Teachers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // =========================
        // Admin Users
        // =========================
        modelBuilder.Entity<AdminUser>(entity =>
        {
            entity.ToTable("admin_users");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(x => x.FirstName)
                .HasColumnName("first_name")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.LastName)
                .HasColumnName("last_name")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Username)
                .HasColumnName("username")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Email)
                .HasColumnName("email")
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.Password)
                .HasColumnName("password")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(x => x.Gender)
                .HasColumnName("gender")
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(x => x.CreatedAt)
                .HasColumnName("created_at");

            entity.Property(x => x.Status)
                .HasColumnName("status")
                .HasMaxLength(20)
                .IsRequired();
        });


        // =========================
        // Teachers
        // =========================
        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.ToTable("teachers");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(x => x.FirstName)
                .HasColumnName("first_name")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.LastName)
                .HasColumnName("last_name")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Email)
                .HasColumnName("email")
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.Phone)
                .HasColumnName("phone")
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.Gender)
                .HasColumnName("gender")
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(x => x.CreatedAt)
                .HasColumnName("created_at");

            entity.Property(x => x.Status)
                .HasColumnName("status")
                .HasMaxLength(20)
                .IsRequired();
        });
    }
}