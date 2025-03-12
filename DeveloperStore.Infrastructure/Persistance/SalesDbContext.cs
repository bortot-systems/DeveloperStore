using DeveloperStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Infrastructure.Persistence
{
    public class SalesDbContext : DbContext
    {
        public SalesDbContext(DbContextOptions<SalesDbContext> options) : base(options) { }

        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(s => s.SaleId);

                entity.OwnsOne(s => s.Customer, customer =>
                {
                    customer.Property(c => c.CustomerId).IsRequired();
                });
            });

            modelBuilder.Entity<SaleItem>(entity =>
            {
                entity.HasKey(si => si.SaleItemId);

                entity.OwnsOne(si => si.Product, product =>
                {
                    product.Property(p => p.ProductId).IsRequired();
                    product.Property(p => p.ProductName).IsRequired().HasMaxLength(100);
                });

                entity.HasOne<Sale>().WithMany(s => s.Items).HasForeignKey(si => si.SaleId);

                entity.Property(si => si.UnitPrice).HasPrecision(18, 4);
                entity.Property(si => si.Discount).HasPrecision(18, 4);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
