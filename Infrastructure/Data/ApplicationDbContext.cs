using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<StorageUnit> StorageUnits { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StorageUnit>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Name).IsRequired().HasMaxLength(100);
                entity.Property(r => r.Type).IsRequired().HasMaxLength(50);
                entity.Property(r => r.Price).HasColumnType("decimal(18,2)");
                entity.Property(r => r.Description).HasMaxLength(500);
                entity.Property(r => r.Area).IsRequired();
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.UserId).IsRequired();
                entity.Property(b => b.UserName).IsRequired().HasMaxLength(100);
                entity.Property(b => b.UserEmail).IsRequired().HasMaxLength(100);

                entity.Property(b => b.StartDate)
                      .HasConversion(v => v.ToUniversalTime(), v => v);
                entity.Property(b => b.EndDate)
                      .HasConversion(v => v.ToUniversalTime(), v => v);
                entity.Property(b => b.CreatedAt)
                      .HasConversion(v => v.ToUniversalTime(), v => v);

                entity.HasOne(b => b.StorageUnit)
                      .WithMany(r => r.Bookings)
                      .HasForeignKey(b => b.StorageUnitId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(b => new { b.StorageUnitId, b.StartDate, b.EndDate });
            });
        }
    }
}